import "../style/Quiz.css";
import { Quiz } from "../../types/quiz/quiz";
import { Question } from "../../types/quiz/question";
import { FillInTheBlank } from "../../types/quiz/fillInTheBlank";
import { Matching } from "../../types/quiz/matching";
import { Ranking } from "../../types/quiz/ranking";
import { MultipleChoice } from "../../types/quiz/multipleChoice";
import { TrueFalse } from "../../types/quiz/trueFalse";
import { Sequence } from "../../types/quiz/sequence";
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
    const [numOfQuestions, setNumOfQuestions] = useState<number>(quiz?.numOfQuestions || 0);
    const [quizName, setQuizName] = useState<string>("");
    const [quizDesc, setQuizDesc] = useState<string>("");
    
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    
    const [selectedType, setSelectedType] = useState<string>("");
    const [notificationType, setNotificationType] = useState<string>("empty");
    const [notificationText, setNotificationText] = useState<string>("");
    const [notificationExit ,setNotificationExit] = useState<boolean>(false);

    const [quizValidatonErrors, setQuizValidationErrors] = useState<{[key: number]: { name?: string, description?: string}}>({});
    const [trueFalseValidationErrors, setTrueFalseValidationErrors] = useState<{[key: number]: { question?: string}}>({});
    const [fibValidationErrors, setFibValidationErrors] = useState<{[key: number]: { question?: string; answer?: string}}>({});
    const [rankingValidationErrors, setRankingValidationErrors] = useState<{[key: number]: { question?: string; length?: string; answer?: string; blankPos?: number[]}}>({});
    const [matchingValidationErrors, setMatchingValidationErrors] = useState<{[key: number]: { question?: string; length?: string; answer?: string; blankPos?: number[]}}>({});
    const [sequenceValidationErrors, setSequenceValidationErrors] = useState<{[key: number]: { question?: string; length?: string; answer?: string; blankPos?: number[]}}>({});
    const [multipleChoiceValidationErrors, setMultipleChoiceValidationErrors] = useState<{[key: number]: { question?: string; length?: string; answer?: string; blankPos?: number[]; hasCorrect?: string}}>({});

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
            setQuizName(data.name);
            setQuizDesc(data.description);
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
        if (!handleValidation()) {
            setNotificationType("delete");
            setNotificationText("Could not save!");
            handleNotification();
            return;
        }

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
        newQuestions.map((q: Question) => {
            console.log(q)
            if(q.isDirty){
                if(q.questionType === "ranking" && q.rankingId){
                    RankingService.updateRanking(q.rankingId, q);
                }else if (q.questionType === "sequence" && q.sequenceId){
                    SequenceService.updateSequence(q.sequenceId, q)
                }else if (q.questionType === "matching" && q.matchingId){
                    MatchingService.updateMatching(q.matchingId, q)
                }else if (q.questionType === "fillInTheBlank" && q.fillInTheBlankId){
                    FillInTheBlankService.updateQuestion(q.fillInTheBlankId, q)
                }else if (q.questionType === "trueFalse" && q.trueFalseId){
                    TrueFalseService.updateTrueFalse(q.trueFalseId, q)
                }else if (q.questionType === "multipleChoice" && q.multipleChoiceId){
                    MultipleChoiceService.updateMultipleChoice(q.multipleChoiceId, q)
                }
            }
        })
        console.log(quiz)
        if(quiz && quiz.quizId && quiz?.isDirty){
            QuizService.updateQuiz(quiz.quizId, quiz)
        }
        setAllQuestions(newQuestions);
        setNotificationType("save");
        setNotificationText("Saved quiz!")
        handleNotification();
    }

    const handleValidation = () => {
        const allQuizErrors: typeof quizValidatonErrors = {};
        const allFibErrors: typeof fibValidationErrors = {};
        const allRankingErrors: typeof rankingValidationErrors = {};
        const allMatchingErrors: typeof matchingValidationErrors = {};
        const allSequenceErrors: typeof sequenceValidationErrors = {};
        const allTrueFalseErrors: typeof trueFalseValidationErrors = {};
        const allMultipleChoiceErrors: typeof multipleChoiceValidationErrors = {};

        if(quiz){
            const q = quiz as Quiz;
            const quizErrs = validateQuiz(q);
            if (Object.keys(quizErrs).length > 0 && typeof q.quizId === "number") allQuizErrors[q.quizId] = quizErrs;
        }

        allQuestions.forEach(q => {
            if (q.questionType === "fillInTheBlank") {
                const fibErrs = validateFib(q);
                if (Object.keys(fibErrs).length > 0) allFibErrors[q.fillInTheBlankId ?? q.tempId!] = fibErrs;
            }
            if (q.questionType === "matching") {
                const matchingErrs = validateMatching(q);
                if (Object.keys(matchingErrs).length > 0) allMatchingErrors[q.matchingId ?? q.tempId!] = matchingErrs;
            }
            if (q.questionType === "sequence") {
                const sequenceErrs = validateSequenceAndRanking(q);
                if (Object.keys(sequenceErrs).length > 0) allSequenceErrors[q.sequenceId ?? q.tempId!] = sequenceErrs;
            }
            if (q.questionType === "ranking") {
                const rankingErrs = validateSequenceAndRanking(q);
                if (Object.keys(rankingErrs).length > 0) allRankingErrors[q.rankingId ?? q.tempId!] = rankingErrs;
            }
            if (q.questionType === "multipleChoice") {
                const multipleChoiceErrs = validateMultipleChoice(q);
                if (Object.keys(multipleChoiceErrs).length > 0) allMultipleChoiceErrors[q.multipleChoiceId ?? q.tempId!] = multipleChoiceErrs;
            }
            if (q.questionType === "trueFalse") {
                const trueFalseErrs = validateTrueFalse(q);
                if (Object.keys(trueFalseErrs).length > 0) allTrueFalseErrors[q.trueFalseId ?? q.tempId!] = trueFalseErrs;
            }
        });

        if (Object.keys(allQuizErrors).length > 0){
            setQuizValidationErrors(allQuizErrors);
            return false;
        }

        if (Object.keys(allFibErrors).length > 0) {
            setFibValidationErrors(allFibErrors);
            return false;
        }

        if (Object.keys(allMatchingErrors).length > 0) {
            setMatchingValidationErrors(allMatchingErrors);
            return false;
        }

        if (Object.keys(allSequenceErrors).length > 0) {
            setSequenceValidationErrors(allSequenceErrors);
            return false;
        }

        if (Object.keys(allRankingErrors).length > 0) {
            setRankingValidationErrors(allRankingErrors);
            return false;
        }

        if (Object.keys(allMultipleChoiceErrors).length > 0) {
            setMultipleChoiceValidationErrors(allMultipleChoiceErrors);
            return false;
        }

        if (Object.keys(allTrueFalseErrors).length > 0) {
            setTrueFalseValidationErrors(allTrueFalseErrors);
            return false;
        }
        
        return true;
    }

    const validateQuiz = (q: Quiz) => {
        const errors: { name?: string; description?: string } = {};
        if (!q.name || q.name.trim() === "") errors.name = "Name is required";
        if(!q.description || q.description.trim() === "") errors.description = "Description is required";
        return errors;
    }

    const validateFib = (q: FillInTheBlank) => {
        const errors: { question?: string; answer?: string } = {};
        if (!q.question || q.question.trim() === "") errors.question = "Question is required";
        if (!q.correctAnswer || q.correctAnswer.trim() === "") errors.answer = "Answer is required";
        return errors;
    }

    const validateMatching = (q: Matching) => {
        const errors: { question?: string; length?: string; answer?: string; blankPos?: number[] } = {};
        if (!q.question || q.question.trim() === "") errors.question = "Question is required";

        const answerList = q.correctAnswer.split(",");
        var pairCounter = 0;
        answerList.forEach((a, i) => {
            if (i % 2 === 0 && (a === "" || answerList[i+1] === "")) {
                if (!errors.blankPos) errors.blankPos = [];

                errors.blankPos.push(pairCounter);
                errors.answer = "Cannot be blank";
            }
            if (i % 2 !== 0) pairCounter++;
        });
        if (answerList.length < 4) errors.length = "Must have 2 or more options";

        return errors;
    }

    const validateSequenceAndRanking = (q: Sequence | Ranking) => {
        const errors: { question?: string; length?: string; answer?: string; blankPos?: number[] } = {};
        if (!q.question || q.question.trim() === "") errors.question = "Question is required";

        const answerList = q.correctAnswer.split(",");
        answerList.forEach((a, i) => {
            if (a === "") {
                if (!errors.blankPos) errors.blankPos = [];

                errors.blankPos.push(i);
                errors.answer = "Cannot be blank";
            }
        });

        if (answerList.length < 3) errors.length = "Must have 3 or more options";

        return errors;
    }

    const validateMultipleChoice = (q: MultipleChoice) => {
        const errors: { question?: string; length?: string; answer?: string; blankPos?: number[]; hasCorrect?: string } = {};
        if (!q.question || q.question.trim() === "") errors.question = "Question is required";

        var hasCorrect = false;

        q.options.forEach((op, i) => {
            if (op.text === "") {
                if (!errors.blankPos) errors.blankPos = [];

                errors.blankPos.push(i);
                errors.answer = "Cannot be blank";
            }
            if (op.isCorrect) hasCorrect = true;
        });

        if (q.options.length < 2) errors.length = "Must have 2 or more options";

        if (!hasCorrect) errors.hasCorrect = "Must set a correct answer";
        return errors;
    }

    const validateTrueFalse = (q: TrueFalse) => {
        const errors: { question?: string } = {};
        if (!q.question || q.question.trim() === "") errors.question = "Question is required";

        return errors;
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
                <div>
                    <div className="quiz-manage-sticky-bar">
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
                        <button className="quiz-manage-btn-save" onClick={handleSaveQuestions}>Save</button>

                    </div>
                
                    <div className="quiz-manage-wrapper">
                        <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
                        <div className="quiz-manage-header">
                            <div className="quiz-manage-header-form-field">
                                <input className="quiz-manage-name" value={quizName} onChange={(e) => {setQuizName(e.target.value); setQuiz(prev => ({...prev!, isDirty:true, name:e.target.value}))}} />
                                {quiz?.quizId && quizValidatonErrors[quiz.quizId] && <span className="quiz-manage-header-error">{quizValidatonErrors[quiz.quizId].name}</span>}
                            </div>
                            <div className="quiz-manage-header-form-field">
                                <textarea className="quiz-manage-description" value={quizDesc} onChange={(e) => {setQuizDesc(e.target.value); setQuiz(prev => ({...prev!, isDirty: true, description: e.target.value}))}}></textarea>
                                {quiz?.quizId && quizValidatonErrors[quiz.quizId] && <span className="quiz-manage-header-error">{quizValidatonErrors[quiz.quizId].description}</span>}
                            </div>
                            <p className="quiz-manage-num-questions">Number of questions: {numOfQuestions}</p>
                            <hr /><br/>
                        </div>
                        <div className="quiz-manage-question-container">
                            <br/><br/>
                            {allQuestions.length > 0 ? (
                                allQuestions.map((q:Question, index:number) => (
                                    <div className="quiz-manage-question-wrapper" key={q.quizQuestionNum}>
                                        <div className="quiz-manage-question-info-wrapper">
                                            <h3 className="quiz-manage-question-num">Question number: {q.quizQuestionNum}</h3>
                                            <button className="quiz-manage-question-delete-btn" onClick={() => handleDeleteQuestion(index)}>Delete</button>
                                        </div>
                                        {q.questionType === "sequence" && (
                                            <div>
                                                <p className="quiz-manage-question-text">Sequence question - Add items in order</p>
                                                <hr />
                                                <SequenceManageForm sequenceId={q.sequenceId} incomingQuestion={q.question} incomingCorrectAnswer={q.correctAnswer} errors={sequenceValidationErrors[q.sequenceId || q.tempId!]}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "sequence" && pq.sequenceId === q.sequenceId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}{q.questionType === "ranking" &&(
                                            <div>
                                                <p className="quiz-manage-question-text">Ranking question - Add items in order</p>
                                                <hr />
                                                <RankingManageForm rankedId={q.rankingId} incomingQuestion={q.question} incomingCorrectAnswer={q.correctAnswer} errors={rankingValidationErrors[q.rankingId ?? q.tempId!]}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "ranking" && pq.rankingId === q.rankingId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}{q.questionType === "fillInTheBlank" && (
                                            <div>
                                                <p className="quiz-manage-question-text">Fill in the blank question</p>
                                                <hr />
                                                <FillInTheBlankManageForm fillInTheblankId={q.fillInTheBlankId} question={q.question} answer={q.correctAnswer} errors={fibValidationErrors[q.fillInTheBlankId! ?? q.tempId]}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "fillInTheBlank" && pq.fillInTheBlankId === q.fillInTheBlankId ? {...pq, ...updatedQuestion} : pq));}} />
                                            </div>
                                        )}{q.questionType === "multipleChoice" && (
                                            <div>
                                                <p className="quiz-manage-question-text">Multiple choice question</p>
                                                <hr />
                                                <MultipleChoiceManageForm multipleChoiceId={q.multipleChoiceId} incomingQuestion={q.question} incomingOptions={q.options} errors={multipleChoiceValidationErrors[q.multipleChoiceId! ?? q.tempId]}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "multipleChoice" && pq.multipleChoiceId === q.multipleChoiceId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}{q.questionType === "trueFalse" && (
                                            <div>
                                                <p className="quiz-manage-question-text">True or false question</p>
                                                <hr />
                                                <TrueFalseManageForm trueFalseId={q.trueFalseId} incomingQuestion={q.question} incomingCorrectAnswer={q.correctAnswer} errors={trueFalseValidationErrors[q.trueFalseId! ?? q.tempId]}
                                                onChange={(updatedQuestion) => {
                                                    setAllQuestions(prev => prev.map(pq => pq.questionType === "trueFalse" && pq.trueFalseId === q.trueFalseId ? {...pq, ...updatedQuestion} : pq));
                                                }} />
                                            </div>
                                        )}{q.questionType === "matching" &&(
                                            <div>
                                                <p className="quiz-manage-question-text">Matching question - Add matching items</p>
                                                <hr />
                                                <MatchingManageForm matchingId={q.matchingId} incomingQuestion={q.question} incomingCorrectAnswer={q.correctAnswer} errors={matchingValidationErrors[q.matchingId! ?? q.tempId]}
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
                </div>
            )}
        </>
    )
}

export default QuizManagePage;
