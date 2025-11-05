import { useState } from "react";
import { FlashCardQuiz } from "../../types/flashCardQuiz";
import { useForm } from "react-hook-form";

interface CreateFormProps {
    onQuizChanged: (newQuiz: FlashCardQuiz) => void;
    flashCardQuizId?: number;
    handleCancel: (value: boolean) => void;
}

type QuizFormData = {
    name: string;
    description: string;
}

const CreateForm: React.FC<CreateFormProps> = ({ onQuizChanged, flashCardQuizId, handleCancel }) => {
    const { register, handleSubmit, formState: { errors } } = useForm<QuizFormData>()

    const onSubmit = async (data: QuizFormData) => {
        const quiz: FlashCardQuiz = { flashCardQuizId, ...data};
        onQuizChanged(quiz);
    }

    return(
        <div className="flash-card-quiz-popup-content" onClick={(e) => e.stopPropagation()}>
            <form onSubmit={handleSubmit(onSubmit)}>
                <h1>Create quiz</h1>
                <div className="input-name">
                    <label>Name</label>
                    <input
                        type="text"
                        placeholder="Enter a name..."
                    {...register("name", {required: "Name is required"})} />
                    {errors.name && <span className={`error ${errors.name ? "visible" : ""}`}>{errors.name.message}</span>}
                </div>
                <div className="input-description">
                    <label>Description</label>
                    <textarea
                        className="description"
                        placeholder="Enter description..."
                        {...register("description", {required: "Description is required"})}/>
                    {errors.description && <span className={`error ${errors.description ? "visible" : ""}`}>{errors.description.message}</span>}
                </div>
                <div className="flash-card-quiz-popup-buttons">
                    <button type="button" className="button" onClick={() => handleCancel(false)}>Cancel</button>
                    <button type="submit" className="button">Create</button>
                </div>
            </form>
        </div>
    )
}

export default CreateForm;