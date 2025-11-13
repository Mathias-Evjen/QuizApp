import "./Quiz.css";
import {useState, useEffect} from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import { Quiz } from "../types/quizCard";
import * as QuizService from "./QuizService";
import * as MultipleChoiceService from "../services/MultipleChoiceService";
import * as TrueFalseService from "../services/TrueFalseService";
import * as MatchingService from "../matching/MatchingService";
//import * as RankingService from "../ranking/RankingService";
//import * as SequenceService from "../sequence/SequenceService";
//import * as FillInTheBlankService from "../questions/FillInTheBlankService";

function QuizManagePage(){
    const navigate = useNavigate();

    const location = useLocation();
    const quiz:any = location.state;
    console.log(quiz);
    const allQuestions = [...(quiz.allQuestions || [])].sort(
        (a, b) => a.quizQuestionNum - b.quizQuestionNum
    );
    console.log(allQuestions);

    const [localQuestions, setLocalQuestions] = useState<any[]>([]);

    const handleAddQuestion = () => {
        const newQuestion = {
            quizQuestionNum: quiz.numOfQuestions + localQuestions.length + 1,
            questionText: "New question...",
            type: "unassigned",
            isNew: true,
            tempId: Date.now()
        };
        setLocalQuestions(prev => [...prev, newQuestion]);
    };

    const handleDeleteQuestion = async (q: any) => {
        if (q.isNew) {
            setLocalQuestions(prev => prev.filter(x => x.tempId !== q.tempId))
            return;
        }

        const id = q.questionId || q.multipleChoiceId || q.trueFalseId || q.fillInTheBlankId || q.matchingId || q.sequenceId || q.rankingId;

        if (!id) {
            console.error("Did not find the ID of the question")
            return;
        }

        try {
            switch (q.type || q.$type) {
                case "MultipleChoice":
                    await MultipleChoiceService.deleteMultipleChoice(id, q.quizQuestionNum, quiz.quizId);
                    break;

                case "TrueFalse":
                    await TrueFalseService.deleteTrueFalse(id, q.quizQuestionNum, quiz.quizId);

                case "Matching":
                    await MatchingService.deleteMatching(q.matchingId);
                    
                default:
                    console.error("Unknown questiontype", q);
            }

            quiz.allQuestions = quiz.allQuestions.filter((x: any) => x.quizQuestionNum !== q.quizQuestionNum)

        } catch (error) {
            console.error("Something went wrong when deleting question", error);
        }
    };

    const mergedQuestions = [...allQuestions, ...localQuestions];


    return(
        <div className="quiz-manage-wrapper">
            <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
            <div className="quiz-manage-header">
                <h3>{quiz.name}</h3>
                <p className="quiz-manage-description">"{quiz.description}"</p>
                <p className="quiz-manage-num-questions">Number of questions: {quiz.numOfQuestions}</p>
                <button className="quiz-manage-question-add-btn" onClick={handleAddQuestion}>Add Question</button><br /><hr /><br/>
            </div>
            <div className="quiz-manage-question-container">
                {mergedQuestions.length > 0 ? (
                    mergedQuestions.map((q:any) => (
                        <div className="quiz-manage-question-wrapper" key={q.questionId ?? q.tempId}>
                            <h3 className="quiz-manage-question-num">Question number: {q.quizQuestionNum}</h3>
                            <p className="quiz-manage-question-text">{q.questionText || q.question}</p>
                            <button className="quiz-manage-question-edit-btn">Edit</button>
                            <button className="quiz-manage-question-delete-btn" onClick={() => handleDeleteQuestion(q)}>Delete</button>
                        </div>
                    ))
                ) : (
                    <h3>No questions found!</h3>
                )}
            </div>
        </div>
    )
}
export default QuizManagePage