import { useState } from "react";
import { FlashCardQuiz } from "../types/flashCardQuiz";

const API_URL = "http://localhost:5041"

interface FlashCardQuizFormProps {
    onQuizChanged: (newQuiz: FlashCardQuiz) => void;
    flashCardQuizId?: number;
}

const QuizForm: React.FC<FlashCardQuizFormProps> = ({ onQuizChanged, flashCardQuizId}) => {
    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        const quiz: FlashCardQuiz = { flashCardQuizId, name, description };
        onQuizChanged(quiz);
    }

    return(
        <>
        <form>
            <label>Name</label>
            <input
                type="text"
                value={name}
                onChange={(e) => setName(e.target.value)}/>

            <label>Description</label>
            <input
                type="text"
                value={description}
                onChange={(e) => setDescription(e.target.value)}/>
            
            <button type="submit" onClick={handleSubmit}>Create</button>
        </form>
        </>
    )
}

export default QuizForm;