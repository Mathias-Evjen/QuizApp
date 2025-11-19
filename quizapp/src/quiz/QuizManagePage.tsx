import "./Quiz.css";
import { useState, useEffect, useOptimistic } from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import * as QuizService from "./QuizService";
import SequenceManageForm from "../sequence/component/SequenceManageForm";
import RankingManageForm from "../ranking/component/RankingManageForm";
import * as MatchingService from "../matching/MatchingService";
import * as SequenceService from "../sequence/SequenceService";
import * as RankingService from "../ranking/RankingService";
import FillInTheBlankEdit from "./questions/FillInTheBlankEdit";
import MultipleChoiceManageForm from "../multipleChoice/component/MultipleChoiceManageForm";
import TrueFalseManageForm from "../trueFalse/component/TrueFalseManageForm";

function QuizManagePage() {
    const navigate = useNavigate();
    const location = useLocation();
    const incomingQuiz:any = location.state;
    const [quiz, setQuiz] = useState<any>(incomingQuiz);
    console.log(quiz);
    const [allQuestions, setAllQuestions] = useState([...(incomingQuiz.allQuestions || [])].sort(
        (a, b) => a.quizQuestionNum - b.quizQuestionNum
    ));
    const [selectedType, setSelectedType] = useState("");
    console.log(allQuestions);


    const handleAddQuestion = () => {
        if(selectedType != ""){
            const questionNum = (allQuestions[allQuestions.length-1].quizQuestionNum)+1;
            const newQuestion = {
                [selectedType]: null,
                questionText: "New question",
                question: "",
                correctAnswer: "",
                quizQuestionNum: questionNum,
                quizId: quiz.quizId,
                isNew: true
            }
            setAllQuestions([...allQuestions, newQuestion]);
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
        if("sequenceId" in allQuestions[index]){
            SequenceService.deleteSequence(allQuestions[index].sequenceId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if("rankingId" in allQuestions[index]){
            RankingService.deleteRanking(allQuestions[index].rankingId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
        } else if("matchingId" in allQuestions[index]){
            MatchingService.deleteMatching(allQuestions[index].matchingId, allQuestions[index].quizQuestionNum, allQuestions[index].quizId)
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

    }

    return(
        <div className="quiz-manage-wrapper">
            <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
            <div className="quiz-manage-header">
                <h3>{quiz.name}</h3>
                <p>"{quiz.description}"</p>
                <p className="quiz-manage-num-questions">Number of questions: {quiz.numOfQuestions}</p>
                <select className="quiz-manage-header-select" onChange={(e) => setSelectedType(e.target.value)}>
                    <option value="">Choose type:</option>
                    <option value="fillInTheBlankId">Fill in the blank</option>
                    <option value="matchingId">Matching</option>
                    <option value="sequenceId">Sequence</option>
                    <option value="rankingId">Ranking</option>
                    <option value="multipleChoiceId">Multiple choice</option>
                    <option value="trueFalseId">True or false</option>
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
                                    <FillInTheBlankEdit fillInTheblankId={q.fillInTheBlankId} question={q.question} answer={q.correctAnswer} />
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
