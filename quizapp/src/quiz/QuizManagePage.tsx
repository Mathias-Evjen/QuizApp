import "./Quiz.css";
import {useState, useEffect} from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import { Quiz } from "../types/quizCard";
import * as QuizService from "./QuizService";

function QuizManagePage(){
    const navigate = useNavigate();
    const location = useLocation();
    const quiz:any = location.state;

    const [name, setName] = useState("");
    const [desc, setDesc] = useState("");
    console.log(quiz);
    const allQuestions = [...(quiz.allQuestions || [])].sort(
        (a, b) => a.quizQuestionNum - b.quizQuestionNum
    );

    return(
        <div className="quiz-manage-wrapper">
            <h3>{quiz.name}</h3><br/>
            <p>"{quiz.description}"</p>
            <p>Number of questions: {quiz.numOfQuestions}</p><hr /><br/>
            <div className="quiz-manage-question-container">
                {allQuestions.length > 0 ? (
                    allQuestions.map((q:any) => (
                        <div className="quiz-manage-question-wrapper">
                            <h3>Question Num: {q.quizQuestionNum}</h3>
                            <p>{q.questionText || q.question}</p>
                        </div>
                    ))
                ) : (
                    <h3>No questions found!</h3>
                )}
            </div>
            <button onClick={() => navigate(-1)}>{"<"}</button>
        </div>
    )
}
export default QuizManagePage