import { useState } from "react";
import { useForm } from "react-hook-form";

interface QuizUpdateProps {
    onCancelClick: (value: boolean) => void;
    onSave: (name: string, description: string) => void;
    name: string;
    description: string;
}

type QuizFormData = {
    name: string;
    description: string;
}

const QuizUpdateForm: React.FC<QuizUpdateProps> = ({ name, description, onCancelClick, onSave }) => {
    const { register, handleSubmit, formState: { errors, isDirty } } = useForm<QuizFormData>({ defaultValues: {name, description} });
    
    const onSubmit = (data: QuizFormData) => {
        onSave(data.name, data.description);
    }

    return(
        <div className="flash-card-quiz-popup-content">
            <form onSubmit={handleSubmit(onSubmit)}>
                <h1>Edit quiz</h1>
                <div className="input-name">
                    <label>Name</label>
                    <input
                        {...register("name", { required: "Name is required" })}/>
                    {errors.name && <span className={`error ${errors.name ? "visible" : ""}`}>{errors.name.message}</span>}
                </div>
                <div className="input-description">
                    <label>Description</label>
                    <textarea
                        {...register("description", { required: "Description is required" })}/>
                    {errors.description && <span className={`error ${errors.description ? "visible" : ""}`}>{errors.description.message}</span>}
                </div>

                <div className="flash-card-quiz-popup-buttons">   
                    <button type="button" className="button" onClick={() => onCancelClick(false)}>Cancel</button>
                    <button type="submit" className={`button save-button ${isDirty? "active" : ""}`}>Save</button>
                </div>
            </form>
        </div>
    )
}

export default QuizUpdateForm;