import "./Quiz.css";
import {useState, useEffect} from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import { Quiz } from "../types/quizCard";
import * as QuizService from "./QuizService";

function QuizManagePage(){
    const navigate = useNavigate();

    const location = useLocation();
    const quiz:any = location.state;
    console.log(quiz);
    const allQuestions = [...(quiz.allQuestions || [])].sort(
        (a, b) => a.quizQuestionNum - b.quizQuestionNum
    );
    console.log(allQuestions);


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
                    allQuestions.map((q:any) => (
                        <div className="quiz-manage-question-wrapper">
                            <h3 className="quiz-manage-question-num">Question number: {q.quizQuestionNum}</h3>
                            <p className="quiz-manage-question-text">{q.questionText || q.question}</p>
                            <button className="quiz-manage-question-edit-btn">Edit</button>
                            <button className="quiz-manage-question-delete-btn">Delete</button>
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