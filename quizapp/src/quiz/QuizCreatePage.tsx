import * as QuizService from "./QuizService";
import "./Quiz.css";
import { useNavigate } from 'react-router-dom';
import { Quiz } from "../types/quizCard";
import {useState, useEffect} from "react";

function QuizCreatePage(){
    const navigate = useNavigate(); // Create a navigate function

    const [name, setName] = useState("");
    const [desc, setDesc] = useState("");

    const handleQuizCreated = async () => {
        try {
            const quiz:Quiz = {
                name: name,
                description: desc,
                numOfQuestions: 0
            }
            //Hente name og desc og lage quiz card    
            const data = await QuizService.createQuiz(quiz);
            console.log('Quiz created successfully:', data);
            navigate('/'); // Navigate back after successful creation
        } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        }
    }

    return(
        <div className="quiz-create-wrapper">
            <button className="quiz-create-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
            <h3>Create quiz</h3>
            <hr /> <br/>
            <div className="quiz-create-input-wrapper">
            <div className="quiz-create-input-name">
                <label htmlFor="quiz-create-input-name">Name:</label>
                <input id="quiz-create-input-name" type="text" value={name} onChange={(e) => setName(e.target.value)} />
            </div>

            <div className="quiz-create-input-desc">
                <label htmlFor="quiz-create-input-desc">Description:</label>
                <input id="quiz-create-input-desc" type="text" value={desc} onChange={(e) => setDesc(e.target.value)} />
            </div>
            </div>

            <button className="quiz-create-btn" onClick={() => handleQuizCreated()}>Create</button>
        </div>
    )
}

export default QuizCreatePage