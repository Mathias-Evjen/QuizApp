import * as QuizService from "./services/QuizService";
import "./style/Quiz.css";
import { useNavigate } from 'react-router-dom';
import { Quiz } from "../types/quiz/quiz";
import { useForm } from "react-hook-form";

type QuizFormData = {
    name: string;
    description: string;
}

function QuizCreatePage(){
    const navigate = useNavigate(); // Create a navigate function

    const { register, handleSubmit, formState: { errors } } = useForm<QuizFormData>({
        defaultValues: {
            name: "",
            description: ""
        }
    });

    const handleQuizCreated = async (data: QuizFormData) => {
        const newName = data.name;
        const newDescription = data.description;
        try {
            const quiz:Quiz = {
                name: newName,
                description: newDescription,
                numOfQuestions: 0
            }
            const data = await QuizService.createQuiz(quiz);
            console.log('Quiz created successfully:', data);
            navigate('/quiz'); // Navigate back after successful creation
        } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        }
    }

    return(
        <form onSubmit={handleSubmit(handleQuizCreated)}>
            <div className="quiz-create-wrapper">
                <button className="quiz-back-btn" onClick={() => navigate(-1)}>{"<"}</button>
                <h3>Create quiz</h3>
                <hr /> <br/>
                <div className="quiz-create-input-wrapper">
                <div className="quiz-create-input-name">
                    <label htmlFor="quiz-create-input-name">Name:</label>
                    <input 
                        id="quiz-create-input-name" 
                        type="text" 
                        placeholder="Enter a name..."
                        {...register("name", {
                            required: {value: true, message: "Name is required"},
                            maxLength: {value: 60, message: "Name too long"}
                        })}/>
                    {errors.name && <span className={`error ${errors.name ? "visible" : ""}`}>{errors.name.message}</span>}
                </div>

                <div className="quiz-create-input-desc">
                    <label htmlFor="quiz-create-input-desc">Description:</label>
                    <textarea 
                        id="quiz-create-input-desc" 
                        placeholder="Enter description..."
                        {...register("description", {
                            required: {value: true, message: "Description is required"},
                            maxLength: {value: 400, message: "Description too long"}
                        })} />
                    {errors.description && <span className={`error ${errors.description ? "visible" : ""}`}>{errors.description.message}</span>}
                </div>
                </div>

                <button type="submit" className="quiz-create-btn">Create</button>
            </div>
        </form>
    )
}

export default QuizCreatePage