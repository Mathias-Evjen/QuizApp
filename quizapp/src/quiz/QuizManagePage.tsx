import "./Quiz.css";
import {useState, useEffect} from "react";
import { useNavigate, useLocation, useParams, data } from 'react-router-dom';
import { Quiz } from "../types/quizCard";
import * as QuizService from "./QuizService";
import { Question } from "../types/Question";
import { FillInTheBlank } from "../types/fillInTheBlank";
import { Matching } from "../types/matching";
import { Ranking } from "../types/ranking";
import { Sequence } from "../types/sequence";
import SequenceManageForm from "../sequence/component/SequenceManageForm";
import RankingManageForm from "../ranking/component/RankingManageForm";
import * as MatchingService from "../matching/MatchingService";
import * as SequenceService from "../sequence/SequenceService";
import * as RankingService from "../ranking/RankingService";
import * as FillInTheBlankService from "./services/FillInTheBlankService"
import FillInTheBlankEdit from "./questions/FillInTheBlankEdit";


function QuizManagePage(){
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);
    
    const navigate = useNavigate();

    // const location = useLocation();
    // const incomingQuiz:any = location.state;
    const [quiz, setQuiz] = useState<Quiz>();
    const [allQuestions, setAllQuestions] = useState<Question[]>([]);
    const [selectedType, setSelectedType] = useState<string>("");
    const [numOfQuestions, setNumOfQuestions] = useState<number>(quiz?.numOfQuestions || 0);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);


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
                data.sequenceQuestions);
            setNumOfQuestions(data.numOfQuestions);
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

    //TODO: HUSK Å LEGG TIL MULTIPLECHOICE OG TRUEFALSE
    const handleSetAllQuestions = (
        fib: FillInTheBlank[], matching: Matching[], ranking: Ranking[], 
        seq: Sequence[]
    ) => {
        const combined: Question[] = [
            ...fib.map(q => ({ ...q, questionType: "fillInTheBlank" as const })),
            ...matching.map(q => ({ ...q, questionType: "matching" as const })),
            ...ranking.map(q => ({ ...q, questionType: "ranking" as const })),
            ...seq.map(q => ({ ...q, questionType: "sequence" as const }))
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
                questionText: "New question",
                question: "",
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
                questionText: "New question",
                question: "",
                correctAnswer: "",
                quizQuestionNum: questionNum,
                quizId,
                isNew: true,
                isDirty: false
            };
        }

        setAllQuestions(prev => [...prev, newQuestion]);
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
        if("sequenceId" in allQuestions[index] && allQuestions[index].sequenceId && !allQuestions[index].isNew){
            SequenceService.deleteSequence(allQuestions[index].sequenceId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if("rankingId" in allQuestions[index] && allQuestions[index].rankingId && !allQuestions[index].isNew){
            RankingService.deleteRanking(allQuestions[index].rankingId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if("matchingId" in allQuestions[index] && allQuestions[index].matchingId && !allQuestions[index].isNew){
            MatchingService.deleteMatching(allQuestions[index].matchingId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if("fillInTheBlankId" in allQuestions[index] && allQuestions[index].fillInTheBlankId && !allQuestions[index].isNew){
            FillInTheBlankService.deleteQuestion(allQuestions[index].fillInTheBlankId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        }

        const updated = allQuestions.filter((_, i) => i !== index);
        setAllQuestions(updated);
        handleQuestionNum(index);
        setNumOfQuestions(prev => prev-1);
    }

    const handleSaveQuestions = async () => {
        const newQuestions = await Promise.all(
        allQuestions.map(async (q) => {
            if (q.isNew && q.correctAnswer !== "") {
            // Sequence
            if ("sequenceId" in q) {
                const { isNew, sequenceId, ...rest } = q;
                const created = await SequenceService.createSequence(rest);
                return { ...created, isNew: false, isDirty: false };
            }
            // Matching
            if ("matchingId" in q) {
                const { isNew, matchingId, ...rest } = q;
                const created = await MatchingService.createMatching(rest);
                return { ...created, isNew: false, isDirty: false };
            }
            // Ranking
            if ("rankingId" in q) {
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
        fetchQuiz();
    }

    useEffect(() => {
        fetchQuiz();
    }, [quizId])

    return(
        <>
            {loading ? (
                <p className="loading">Loading...</p>
            ) : error ? (
                <p className="fetch-error">{error}</p>
            ) : (
                <div className="quiz-manage-wrapper">
                    <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
                    <div className="quiz-manage-header">
                        <h3>{quiz?.name}</h3>
                        <p>"{quiz?.description}"</p>
                        <p className="quiz-manage-num-questions">Number of questions: {numOfQuestions}</p>
                        <select className="quiz-manage-header-select" onChange={(e) => setSelectedType(e.target.value)}>
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
                                            <FillInTheBlankEdit fillInTheblankId={q.fillInTheBlankId} question={q.question} answer={q.correctAnswer}
                                            onChange={(updatedQuestion) => {
                                                setAllQuestions(prev => prev.map(pq => "fillInTheBlankId" in pq && pq.fillInTheBlankId === q.fillInTheBlankId ? {...pq, ...updatedQuestion} : pq));}} />
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
export default QuizManagePage