import "./Quiz.css";
import {useState, useEffect} from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import { Quiz } from "../types/quizCard";
import * as QuizService from "./QuizService";
import SequenceManageForm from "../sequence/component/SequenceManageForm";
import RankingManageForm from "../ranking/component/RankingManageForm";

function QuizManagePage(){
    const navigate = useNavigate();

    const location = useLocation();
    const incomingQuiz:any = location.state;
    const [quiz, setQuiz] = useState<any>(incomingQuiz);
    console.log(quiz);
    const [allQuestions, setAllQuestions] = useState([...(incomingQuiz.allQuestions || [])].sort(
        (a, b) => a.quizQuestionNum - b.quizQuestionNum
    ));
    console.log(allQuestions);


    const refreshQuizObjekt = async () => {
        try{
            const data = await QuizService.fetchQuiz(quiz.quizId);
            setQuiz(data);
            setAllQuestions([...(data.allQuestions || [])].sort((a, b) => a.quizQuestionNum - b.quizQuestionNum))
        } catch (error:unknown){
            console.log("Error fetching data: ", error)
        }
    }

    return(
        <div className="quiz-manage-wrapper">
            <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
            <div className="quiz-manage-header">
                <h3>{quiz.name}</h3>
                <p>"{quiz.description}"</p>
                <p className="quiz-manage-num-questions">Number of questions: {quiz.numOfQuestions}</p><hr /><br/>
            </div>
            <div className="quiz-manage-question-container">
                {allQuestions.length > 0 ? (
                    allQuestions.map((q:any, index:number) => (
                        <div className="quiz-manage-question-wrapper">
                            <div className="quiz-manage-question-info-wrapper">
                                <h3 className="quiz-manage-question-num">Question number: {q.quizQuestionNum}</h3>
                                <p className="quiz-manage-question-text">{q.questionText || q.question}</p>
                                <button className="quiz-manage-question-edit-btn">Edit</button>
                                <button className="quiz-manage-question-delete-btn">Delete</button>
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