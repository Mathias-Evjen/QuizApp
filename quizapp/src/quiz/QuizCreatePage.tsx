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
            <button className="quiz-create-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
            <h3>Create quiz</h3>
            <hr /> <br/>
            <div className="quiz-create-input-wrapper">
            <div className="quiz-create-input-name">
                <label htmlFor="name">Name:</label>
                <input id="name" type="text" />
            </div>

            <div className="quiz-create-input-desc">
                <label htmlFor="desc">Description:</label>
                <input id="desc" type="text" />
            </div>
            </div>

            <button className="quiz-create-btn">Create</button>
        </div>
    )
}

export default QuizCreatePage