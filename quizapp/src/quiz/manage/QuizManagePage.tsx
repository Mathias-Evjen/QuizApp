import "../style/Quiz.css";
import { Quiz } from "../../types/quiz";
import { FillInTheBlank } from "../../types/fillInTheBlank";
import { Matching } from "../../types/matching";
import { Ranking } from "../../types/ranking";
import { TrueFalse } from "../../types/trueFalse";
import { MultipleChoice } from "../../types/multipleChoice";
import { Sequence } from "../../types/sequence";
import { Question } from "../../types/Question";
import { useNavigate, useParams } from 'react-router-dom';
import { useEffect, useState } from "react";
import * as QuizService from "../services/QuizService";
import * as MatchingService from "../services/MatchingService";
import * as SequenceService from "../services/SequenceService";
import * as RankingService from "../services/RankingService";
import * as FillInTheBlankService from "../services/FillInTheBlankService";
import * as TrueFalseService from "../services/TrueFalseService";
import * as MultipleChoiceService from "../services/MultipleChoiceService";
import FillInTheBlankManageForm from "./FillInTheBlankManageForm";
import SequenceManageForm from "./SequenceManageForm";
import RankingManageForm from "./RankingManageForm";
import MultipleChoiceManageForm from "./MultipleChoiceManageForm";
import TrueFalseManageForm from "./TrueFalseManageForm";
import MatchingManageForm from "./MatchingManageForm";

function QuizManagePage() {
    const navigate = useNavigate();
    
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [quiz, setQuiz] = useState<Quiz>();
    const [allQuestions, setAllQuestions] = useState<Question[]>([]);
    const [selectedType, setSelectedType] = useState<string>("");
    const [numOfQuestions, setNumOfQuestions] = useState<number>(quiz?.numOfQuestions || 0);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [notificationType, setNotificationType] = useState<string>("empty");
    const [notificationText, setNotificationText] = useState<string>("");
    const [notificationExit ,setNotificationExit] = useState<boolean>(false);

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
            setNumOfQuestions(data.numOfQuestions);
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
        if (!selectedType) {
            console.log("Must choose a question type!");
            return;
        }
        let newQuestion: Question;
        const questionNum = (allQuestions.length > 0 ?(allQuestions[allQuestions.length-1].quizQuestionNum)+1 : (1));
        if (selectedType === "sequence") {
            newQuestion = {
                questionType: "sequence",
                sequenceId: undefined,
                question: "New question",
                correctAnswer: "",
                quizQuestionNum: questionNum,
                quizId,
                isNew: true,
                isDirty: false
            };
        }
        else if (selectedType === "ranking") {
            newQuestion = {
                questionType: "ranking",
                rankingId: undefined,
                question: "New question",
                correctAnswer: "",
                quizQuestionNum: questionNum,
                quizId,
                isNew: true,
                isDirty: false
            };
        }
        else if (selectedType === "matching") {
            newQuestion = {
                questionType: "matching",
                matchingId: undefined,
                question: "New question",
                correctAnswer: "",
                quizQuestionNum: questionNum,
                quizId,
                isNew: true,
                isDirty: false
            };
        }
        else if (selectedType === "fillInTheBlank") {
            newQuestion = {
                questionType: "fillInTheBlank",
                fillInTheBlankId: undefined,
                question: "New question",
                correctAnswer: "",
                quizQuestionNum: questionNum,
                quizId,
                isNew: true,
                isDirty: false
            };
        }
        else if (selectedType === "trueFalse") {
            newQuestion = {
                questionType: "trueFalse",
                trueFalseId: undefined,
                question: "New question",
                correctAnswer: false,
                quizQuestionNum: questionNum,
                quizId,
                isNew: true,
                isDirty: false
            };
        }
        else if (selectedType === "multipleChoice") {

            newQuestion = {
                questionType: "multipleChoice",
                multipleChoiceId: undefined,
                question: "New question",
                options: [],
                correctAnswer: "",
                quizQuestionNum: questionNum,
                quizId,
                isNew: true,
                isDirty: false
            };
        }

        setAllQuestions(prev => [...prev, newQuestion]);
        setNumOfQuestions(prev => prev+1);
        setSelectedType("");
        setNotificationType("add");
        setNotificationText("Added question!")
        handleNotification();
        return;
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
        if(allQuestions[index].questionType === "sequence" && allQuestions[index].sequenceId && !allQuestions[index].isNew){
            SequenceService.deleteSequence(allQuestions[index].sequenceId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if(allQuestions[index].questionType === "ranking" && allQuestions[index].rankingId && !allQuestions[index].isNew){
            RankingService.deleteRanking(allQuestions[index].rankingId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if(allQuestions[index].questionType === "matching" && allQuestions[index].matchingId && !allQuestions[index].isNew){
            MatchingService.deleteMatching(allQuestions[index].matchingId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if(allQuestions[index].questionType === "fillInTheBlank" && allQuestions[index].fillInTheBlankId && !allQuestions[index].isNew){
            FillInTheBlankService.deleteQuestion(allQuestions[index].fillInTheBlankId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if(allQuestions[index].questionType === "trueFalse" && allQuestions[index].trueFalseId && !allQuestions[index].isNew){
            TrueFalseService.deleteTrueFalse(allQuestions[index].trueFalseId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if(allQuestions[index].questionType === "multipleChoice" && allQuestions[index].multipleChoiceId && !allQuestions[index].isNew){
            MultipleChoiceService.deleteMultipleChoice(allQuestions[index].multipleChoiceId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        }

        const updated = allQuestions.filter((_, i) => i !== index);
        setAllQuestions(updated);
        handleQuestionNum(index);
        setNumOfQuestions(prev => prev-1);
        setNotificationType("delete")
        setNotificationText("Deleted question!")
        handleNotification();
    }

    const handleSaveQuestions = async () => {
        const newQuestions = await Promise.all(
        allQuestions.map(async (q) => {
            if (q.isNew && q.correctAnswer !== "") {
            if (q.questionType === "sequence") {
                const { isNew, sequenceId, ...rest } = q;
                const created = await SequenceService.createSequence(rest);
                return { ...created, isNew: false, isDirty: false, questionType: "sequence" };
            }
            if (q.questionType === "matching") {
                const { isNew, matchingId, ...rest } = q;
                const created = await MatchingService.createMatching(rest);
                return { ...created, isNew: false, isDirty: false, questionType: "matching" };
            }
            if (q.questionType === "ranking") {
                const { isNew, rankingId, ...rest } = q;
                const created = await RankingService.createRanking(rest);
                return { ...created, isNew: false, isDirty: false, questionType: "ranking" };
            }
            if (q.questionType === "fillInTheBlank") {
                const { isNew, fillInTheBlankId, ...rest } = q;
                const created = await FillInTheBlankService.createQuestion(rest);
                return { ...created, isNew: false, isDirty: false, questionType: "fillInTheBlank" };
            }
            if (q.questionType === "trueFalse") {
                const { isNew, trueFalseId, ...rest } = q;
                const created = await TrueFalseService.createTrueFalse(rest);
                return { ...created, isNew: false, isDirty: false, questionType: "trueFalse" };
            }
        }
        if (q.questionType === "multipleChoice" && q.isNew) {
            const { isNew, multipleChoiceId, ...rest } = q;
            const created = await MultipleChoiceService.createMultipleChoice(rest);
            return { ...created, isNew: false, isDirty: false, questionType: "multipleChoice" };
        }
            return q;
        })
        );
        newQuestions.map(q => {
            if(q.isDirty){
                if(q.questionType === "ranking"){
                    RankingService.updateRanking(q.rankingId, q);
                }else if (q.questionType === "sequence"){
                    SequenceService.updateSequence(q.sequenceId, q)
                }else if (q.questionType === "matching"){
                    MatchingService.updateMatching(q.matchingId, q)
                }else if (q.questionType === "fillInTheBlank"){
                    FillInTheBlankService.updateQuestion(q.fillInTheBlankId, q)
                }else if (q.questionType === "trueFalse"){
                    TrueFalseService.updateTrueFalse(q.trueFalseId, q)
                }else if (q.questionType === "multipleChoice"){
                    MultipleChoiceService.updateMultipleChoice(q.multipleChoiceId, q)
                }
            }
        })
        setAllQuestions(newQuestions);
        setNotificationType("save");
        setNotificationText("Saved quiz!")
        handleNotification();
    }

    const handleNotification = () => {
        setNotificationExit(false);
        setTimeout(() => {
            setNotificationExit(true);

            
            setTimeout(() => {
                setNotificationText("");
                setNotificationType("");
            }, 350);
        }, 3000); 
    }

    useEffect(() => {
        fetchQuiz();
    }, []);

    return(
            <>  
                <div className={`quiz-manage-notification-wrapper ${notificationType} ${notificationExit ? "exit" : ""}`}>
                    <label>{notificationText}</label>
                </div>
                {loading ? (
                    <p className="loading">Loading...</p>
                ) : error ? (
                    <p className="fetch-error">{error}</p>
                ) : (
                    <div className="quiz-manage-wrapper">
                        <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
                        <div className="quiz-manage-header">
                            <h3>{quiz?.name}</h3>
                            <p className="quiz-manage-description">"{quiz?.description}"</p>
                            <p className="quiz-manage-num-questions">Number of questions: {numOfQuestions}</p>
                            <select className="quiz-manage-header-select" value={selectedType} onChange={(e) => setSelectedType(e.target.value)}>
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
                                            <button className="quiz-manage-question-delete-btn" onClick={() => handleDeleteQuestion(index)}>Delete</button>
                                        </div>
                                        {q.questionType === "sequence" && (
                                            <div>
                                                <p className="quiz-manage-question-text">Sequence question - Add items in order</p>
                                                <hr />
                                                <SequenceManageForm sequenceId={q.sequenceId} incomingQuestion={q.question} incomingCorrectAnswer={q.correctAnswer} 
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "sequence" && pq.sequenceId === q.sequenceId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}{q.questionType === "ranking" &&(
                                            <div>
                                                <p className="quiz-manage-question-text">Ranking question - Add items in order</p>
                                                <hr />
                                                <RankingManageForm rankedId={q.rankedId} incomingQuestion={q.question} incomingCorrectAnswer={q.correctAnswer}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "ranking" && pq.rankingId === q.rankingId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}{q.questionType === "fillInTheBlank" && (
                                            <div>
                                                <p className="quiz-manage-question-text">Fill in the blank question</p>
                                                <hr />
                                                <FillInTheBlankManageForm fillInTheblankId={q.fillInTheBlankId} question={q.question} answer={q.correctAnswer}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "fillInTheBlank" && pq.fillInTheBlankId === q.fillInTheBlankId ? {...pq, ...updatedQuestion} : pq));}} />
                                            </div>
                                        )}{q.questionType === "multipleChoice" && (
                                            <div>
                                                <p className="quiz-manage-question-text">Multiple choice question</p>
                                                <hr />
                                                <MultipleChoiceManageForm multipleChoiceId={q.multipleChoiceId} incomingQuestion={q.question} incomingOptions={q.options}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "multipleChoice" && pq.multipleChoiceId === q.multipleChoiceId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}{q.questionType === "trueFalse" && (
                                            <div>
                                                <p className="quiz-manage-question-text">True or false question</p>
                                                <hr />
                                                <TrueFalseManageForm trueFalseId={q.trueFalseId} incomingQuestion={q.question} incomingCorrectAnswer={q.correctAnswer}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "trueFalse" && pq.trueFalseId === q.trueFalseId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}{q.questionType === "matching" &&(
                                            <div>
                                                <p className="quiz-manage-question-text">Matching question - Add matching items</p>
                                                <hr />
                                                <MatchingManageForm matchingId={q.matchingId} incomingQuestion={q.question} incomingCorrectAnswer={q.correctAnswer}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "matching" && pq.matchingId === q.matchingId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}
                                    </div>
                                ))
                            ) : (
                                <h3>No questions found!</h3>
                            )}
                        </div>
                    </div>
                )}
            </>
        )
    }

export default QuizManagePage;
