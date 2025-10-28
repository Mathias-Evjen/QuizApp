import { useState } from "react";
import { FlashCardQuiz } from "../types/flashCardQuiz";

const API_URL = "http://localhost:5041"

interface CreateFormProps {
    onQuizChanged: (newQuiz: FlashCardQuiz) => void;
    flashCardQuizId?: number;
    handleCancel: (value: boolean) => void;
}

const CreateForm: React.FC<CreateFormProps> = ({ onQuizChanged, flashCardQuizId, handleCancel }) => {
    const [name, setName] = useState<string>("");
    const [description, setDescription] = useState<string>("");

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        const quiz: FlashCardQuiz = { flashCardQuizId, name, description };
        onQuizChanged(quiz);
    }

    return(
        <div className="create-flash-card-quiz-popup-content">
            <form>
                <div className="input-name">
                    <label>Name</label>
                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Enter a name..."/>
                </div>
                <div className="input-description">
                    <label>Description</label>
                    <textarea
                        className="description"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        placeholder="Enter description..."/>
                </div>
                <button type="button" onClick={() => handleCancel(false)}>Cancel</button>
                <button type="submit" onClick={handleSubmit}>Create</button>
            </form>
        </div>
    )
}

export default CreateForm;