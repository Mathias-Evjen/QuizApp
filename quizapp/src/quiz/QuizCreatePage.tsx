import * as QuizService from "./QuizService";
import "./Quiz.css";
import { useNavigate } from 'react-router-dom';
import { Quiz } from "../types/quizCard";

function QuizCreatePage(){
    const navigate = useNavigate(); // Create a navigate function

    const handleQuizCreated = async (quiz: Quiz) => {
        try {
            //Hente name og desc og lage quiz card    
            const data = await QuizService.createQuiz(quiz);
            console.log('Quiz created successfully:', data);
            navigate('/quizzes'); // Navigate back after successful creation
        } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        }
    }

    return(
        <div className="quiz-create-wrapper">
            <h3>Create quiz</h3>
            <div className="quiz-create-input-wrapper">
                <label htmlFor="quiz-create-input-name">Name: </label>
                <input id="quiz-create-input-name" className="quiz-create-input-name" />
                <label htmlFor="quiz-create-input-desc">Description: </label>
                <input id="quiz-create-input-desc" className="quiz-create-input-desc" />
            </div>
            <button className="quiz-create-btn">Create</button>
        </div>
    )
}

export default QuizCreatePage