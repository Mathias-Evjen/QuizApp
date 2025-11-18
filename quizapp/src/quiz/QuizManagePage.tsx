import "./Quiz.css";
import {useState, useEffect} from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import { Quiz } from "../types/quizCard";
import * as QuizService from "./QuizService";
import SequenceManageForm from "../sequence/component/SequenceManageForm";
import RankingManageForm from "../ranking/component/RankingManageForm";
import * as MatchingService from "../matching/MatchingService";
import * as SequenceService from "../sequence/SequenceService";
import * as RankingService from "../ranking/RankingService";

function QuizManagePage(){
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
                    quizQuestionNum: q.quizQuestionNum - 1
                };
            }
            return q;
        })
    );
    }

    const handleDeleteQuestion = (index:number) => {
        //Fjerner bare fra allQuestions ikke databasen, det skjer under handleSaveQuestions()
        const updated = allQuestions.filter((_, i) => i !== index);
        setAllQuestions(updated);
        handleQuestionNum(index);
    }

    const handleSaveQuestions = () => {
        //Gå gjennom alle questions og se om noen er fjernet.
        //Oppdatere database hvis noen questions er fjernet og om noen er oppdatert
        const newQuestions = allQuestions.map(q => {
        if (q.isNew && q.correctAnswer != "") {
            if ("matchingId" in q) {
            const { matchingId, isNew,  ...rest } = q;
            MatchingService.createMatching(rest);
            return rest;
            }
            if ("sequenceId" in q) {
            const { sequenceId, isNew,  ...rest } = q;
            SequenceService.createSequence(rest);
            return rest;
            }
            if ("rankingId" in q) {
            const { rankingId, isNew,  ...rest } = q;
            RankingService.createRanking(rest);
            return rest;
            }
        }
        return q;
        });
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
                                <button className="quiz-manage-question-edit-btn">Edit</button>
                                <button className="quiz-manage-question-delete-btn" onClick={() => handleDeleteQuestion(index)}>Delete</button>
                            </div>
                            {"sequenceId" in q && (
                                <div>
                                    <hr />
                                    <SequenceManageForm incomingSequenceCard={q} />
                                </div>
                            )}{"rankingId" in q &&(
                                <div>
                                    <hr />
                                    <RankingManageForm incomingRankingCard={q} />
                                </div>
                            )}
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