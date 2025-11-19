import "../style/Quiz.css";
import { Quiz } from "../../types/quiz";
import { FillInTheBlank } from "../../types/fillInTheBlank";
import { Matching } from "../../types/matching";
import { Ranking } from "../../types/ranking";
import { TrueFalse } from "../../types/trueFalse";
import { MultipleChoice } from "../../types/multipleChoice";
import { Sequence } from "../../types/sequence";
import { Question, QuestionType } from "../../types/Question";
import { useNavigate, useParams } from 'react-router-dom';
import { useEffect, useState } from "react";
import * as QuizService from "../services/QuizService";
import * as MatchingService from "../services/MatchingService";
import * as SequenceService from "../services/SequenceService";
import * as RankingService from "../services/RankingService";
import FillInTheBlankManageFrom from "./FillInTheBlankManageForm";
import SequenceManageForm from "./SequenceManageForm";
import RankingManageForm from "./RankingManageForm";
import MultipleChoiceManageForm from "./MultipleChoiceManageForm";
import TrueFalseManageForm from "./TrueFalseManageForm";

function QuizManagePage() {
    const navigate = useNavigate();
    
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [quiz, setQuiz] = useState<Quiz>();
    const [allQuestions, setAllQuestions] = useState<Question[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    console.log(quiz);
    const [selectedType, setSelectedType] = useState<QuestionType | "">("");
    console.log(allQuestions);

    const fetchQuiz = async () => {
        setLoading(true);
        setError(null);

        try {
            const data = await QuizService.fetchQuiz(quizId);
            setQuiz(data);
            handleSetAllQuestions(
                data.fillInTheBlankQuestions, 
                data.matchingQuestions, 
                data.rankingQuestions,
                data.sequenceQuestions,
                data.multipleChoiceQuestions,
                data.trueFalseQuestions);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setError("Failed to fetch quiz");
        } finally {
            setLoading(false);
        }
    };

    const handleSetAllQuestions = (
        fib: FillInTheBlank[], matching: Matching[], ranking: Ranking[], 
        sequence: Sequence[], multipleChoice: MultipleChoice[], trueFalse: TrueFalse[]
    ) => {

        const combined: Question[] = [
            ...fib.map(q => ({ ...q, questionType: "fillInTheBlank" as const })),
            ...matching.map(q => ({ ...q, questionType: "matching" as const })),
            ...ranking.map(q => ({ ...q, questionType: "ranking" as const })),
            ...sequence.map(q => ({ ...q, questionType: "sequence" as const })),
            ...multipleChoice.map(q => ({ ...q, questionType: "multipleChoice" as const })),
            ...trueFalse.map(q => ({ ...q, questionType: "trueFalse" as const }))
        ];

        combined.sort((a, b) => a.quizQuestionNum - b.quizQuestionNum);

        setAllQuestions(combined);
    };

    const handleAddQuestion = () => {
        if(selectedType !== ""){
            // const questionNum = (allQuestions[allQuestions.length-1].quizQuestionNum)+1;
            // const newQuestion: Question = {
            //     questionType: selectedType,
            //     questionText: "New question",
            //     question: "",
            //     correctAnswer: "",
            //     quizQuestionNum: questionNum,
            //     quizId: quiz?.quizId!,
            //     isNew: true
            // }
            // setAllQuestions([...allQuestions, newQuestion]);
            return;
        }
        console.log("Must choose a question type!")
    }

    const handleQuestionNum = (index:number) => {
        setAllQuestions(prev =>
        prev.map(q => {
            if (q.quizQuestionNum > index) {
                return {
                    ...q,
                    quizQuestionNum: q.quizQuestionNum - 1,
                    isDirty: true
                };
            }
            return q;
        })
    );
    }

    const handleDeleteQuestion = (index:number) => {
        if(allQuestions[index].questionType === "sequence"){
            SequenceService.deleteSequence(allQuestions[index].sequenceId!, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if(allQuestions[index].questionType === "ranking"){
            RankingService.deleteRanking(allQuestions[index].rankingId!, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if(allQuestions[index].questionType === "matching"){
            MatchingService.deleteMatching(allQuestions[index].matchingId!, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        }

        const updated = allQuestions.filter((_, i) => i !== index);
        setAllQuestions(updated);
        handleQuestionNum(index);
    }

    const handleSaveQuestions = async () => {
        const newQuestions = await Promise.all(
        allQuestions.map(async (q) => {
            if (q.isNew && q.correctAnswer !== "") {
            // Sequence
            if (q.questionType === "sequence") {
                const { isNew, sequenceId, ...rest } = q;
                const created = await SequenceService.createSequence(rest);
                return { ...created, isNew: false, isDirty: false };
            }
            // Matching
            if (q.questionType === "matching") {
                const { isNew, matchingId, ...rest } = q;
                const created = await MatchingService.createMatching(rest);
                return { ...created, isNew: false, isDirty: false };
            }
            // Ranking
            if (q.questionType === "ranking") {
                const { isNew, rankingId, ...rest } = q;
                const created = await RankingService.createRanking(rest);
                return { ...created, isNew: false, isDirty: false };
            }
            }
            return q; // eksisterende spørsmål
        })
        );
        console.log(newQuestions)
        newQuestions.map(q => {
            if(q.isDirty){
                if("rankingId" in q){
                    RankingService.updateRanking(q.rankingId, q);
                }else if ("sequenceId" in q){
                    SequenceService.updateSequence(q.sequenceId, q)
                }
            }
        })
        setAllQuestions(newQuestions);
    }

    useEffect(() => {
        fetchQuiz();
    }, []);

    return(
        <div className="quiz-manage-wrapper">
            <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
            <div className="quiz-manage-header">
                <h3>{quiz?.name}</h3>
                <p>"{quiz?.description}"</p>
                <p className="quiz-manage-num-questions">Number of questions: {quiz?.numOfQuestions}</p>
                <select className="quiz-manage-header-select" onChange={(e) => setSelectedType(e.target.value as QuestionType | "")}>
                    <option value="">Choose type:</option>
                    <option value="fillInTheBlank">Fill in the blank</option>
                    <option value="matching">Matching</option>
                    <option value="sequence">Sequence</option>
                    <option value="ranking">Ranking</option>
                    <option value="multipleChoice">Multiple choice</option>
                    <option value="trueFalse">True or false</option>
                </select>
                <button className="quiz-manage-header-btn-add" onClick={handleAddQuestion}>Add Question</button>
                <hr /><br/>
            </div>
            <div className="quiz-manage-question-container">
                <button className="quiz-manage-btn-save" onClick={handleSaveQuestions}>Save</button>
                <br/><br/>
                {allQuestions.length > 0 ? (
                    allQuestions.map((q:any, index:number) => (
                        <div className="quiz-manage-question-wrapper">
                            <div className="quiz-manage-question-info-wrapper">
                                <h3 className="quiz-manage-question-num">Question number: {q.quizQuestionNum}</h3>
                                <p className="quiz-manage-question-text">{q.questionText || q.question}</p>
                                <button className="quiz-manage-question-delete-btn" onClick={() => handleDeleteQuestion(index)}>Delete</button>
                            </div>
                            {"sequenceId" in q && (
                                <div>
                                    <hr />
                                    <SequenceManageForm sequenceId={q.sequenceId} incomingQuestionText={q.questionText} incomingCorrectAnswer={q.correctAnswer} 
                                    onChange={(updatedQuestion) => {
                                        setAllQuestions(prev => prev.map(pq => "sequenceId" in pq && pq.sequenceId === q.sequenceId ? {...pq, ...updatedQuestion} : pq));
                                    }} />
                                </div>
                            )}{"rankingId" in q &&(
                                <div>
                                    <hr />
                                    <RankingManageForm rankedId={q.rankedId} incomingQuestionText={q.questionText} incomingCorrectAnswer={q.correctAnswer}
                                    onChange={(updatedQuestion) => {
                                        setAllQuestions(prev => prev.map(pq => "rankingId" in pq && pq.rankingId === q.rankingId ? {...pq, ...updatedQuestion} : pq));
                                    }} />
                                </div>
                            )}{"fillInTheBlankId" in q && (
                                <div>
                                    <hr />
                                    <FillInTheBlankManageFrom fillInTheblankId={q.fillInTheBlankId} question={q.question} answer={q.correctAnswer} />
                                </div>
                            )}{"multipleChoiceId" in q && (
                                <div>
                                    <hr />
                                    <MultipleChoiceManageForm incomingMultipleChoice={q} />
                                </div>
                            )}{"trueFalseId" in q && (
                                <div>
                                    <hr />
                                    <TrueFalseManageForm incomingTrueFalse={q} />
                                </div>
                            )}
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
