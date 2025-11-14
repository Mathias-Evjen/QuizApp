import "./Quiz.css";
import { useState, useEffect, useOptimistic } from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import * as QuizService from "./QuizService";
import * as MultipleChoiceService from "../services/MultipleChoiceService";
import * as TrueFalseService from "../services/TrueFalseService";
import * as MatchingService from "../matching/MatchingService";

function QuizManagePage() {
    const navigate = useNavigate();
    const location = useLocation();

    const incomingQuiz: any = location.state;
    const [quiz, setQuiz] = useState<any>(incomingQuiz);
    const [allQuestions, setAllQuestions] = useState([...(incomingQuiz.allQuestions || [])].sort((a, b) => a.quizQuestionNum - b.quizQuestionNum));

    const [localQuestions, setLocalQuestions] = useState<any[]>([]);
    const [error, setError] = useState<string | null>(null);

    const refreshQuizObjekt = async () => {
        console.log("HALALALALALAQLALAL");
        try {
            const data = await QuizService.fetchQuiz(quiz.quizId);
            console.log(data);
            setQuiz(data);

            setAllQuestions([...(data.allQuestions || [])].sort((a, b) => a.quizQuestionNum - b.quizQuestionNum));
            return data;

        } catch (error: unknown) {
            console.log("Error fetching data: ", error);
        }
    };

    const handleAddQuestion = () => {
        const newQuestion = {
            quizQuestionNum: allQuestions.length + localQuestions.length + 1,
            questionText: "New question...",
            isNew: true,
            tempId: Date.now()
        };
//        setLocalQuestions(prev => [...prev, newQuestion]);
        setAllQuestions([...allQuestions, newQuestion])
    };

    const handleQuestionNumbersAfterDelete = (deletedNum: number, list: any[]) => {
        return list.map((q) => q.quizQuestionNum > deletedNum ? { ...q, quizQuestionNum: q.quizQuestionNum - 1 } : q);
    };

    const handleDeleteQuestion = async (question: any, index: number) => {
        setError(null);
        const deletedNum = question.quizQuestionNum;

        if (question.isNew) {
            setAllQuestions(prev =>
                handleQuestionNumbersAfterDelete(
                    deletedNum,
                    prev.filter(q => q.tempId !== question.tempId)
                )
            );
            return;
        }

        try {
            if (question.trueFalseId) {
                await TrueFalseService.deleteTrueFalse(
                    question.trueFalseId,
                    deletedNum,
                    quiz.quizId
                );
            } else if (question.multipleChoiceId) {
                await MultipleChoiceService.deleteMultipleChoice(
                    question.multipleChoiceId,
                    deletedNum,
                    quiz.quizId
                );
            } else if (question.matchingId) {
                await MatchingService.deleteMatching(question.matchingId);
            }

   //         const data = await refreshQuizObjekt();
            //console.log(data);
          //  setQuiz(data);
            const updated = allQuestions.filter((_, i) => i !== index);
            setAllQuestions(updated)
            console.log(allQuestions);
            setAllQuestions(handleQuestionNumbersAfterDelete(deletedNum, [...allQuestions]));



        } catch (err) {
            console.error("Failed to delete question:", err);
            setError("Failed to delete question");
        }
    };

    const mergedQuestions = [...allQuestions, ...localQuestions].sort(
        (a, b) => a.quizQuestionNum - b.quizQuestionNum
    );

    return (
        <div className="quiz-manage-wrapper">
            <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
            <div className="quiz-manage-header">
                <h3>{quiz.name}</h3>
                <p className="quiz-manage-description">"{quiz.description}"</p>
                <p className="quiz-manage-num-questions">Number of questions: {quiz.numOfQuestions}</p>
                <button className="quiz-manage-question-add-btn" onClick={handleAddQuestion}>Add Question</button>
                <br /><hr /><br />
            </div>
            <div className="quiz-manage-question-container">
                {allQuestions.length > 0 ? (
                    allQuestions.map((q: any, index: number) => (
                        <div className="quiz-manage-question-wrapper" key={q.tempId ?? q.trueFalseId ?? q.multipleChoiceId ?? q.matchingId}>
                            <h3 className="quiz-manage-question-num">Question number: {q.quizQuestionNum}</h3>
                            <p className="quiz-manage-question-text">{q.questionText || q.question}</p>
                            <button className="quiz-manage-question-edit-btn">Edit</button>
                            <button className="quiz-manage-question-delete-btn" onClick={() => handleDeleteQuestion(q, index)}>Delete</button>
                        </div>
                    ))
                ) : (
                    <h3>No questions found!</h3>
                )}
            </div>
        </div>
    );
}

export default QuizManagePage;
