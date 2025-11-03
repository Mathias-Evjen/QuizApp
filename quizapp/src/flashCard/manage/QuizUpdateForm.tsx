import { useState } from "react";

interface QuizUpdateProps {
    onCancelClick: (value: boolean) => void;
    onSaveClick: () => void;
    onNameChanged: (newName: string) => void;
    onDescriptionChanged: (newDescription: string) => void;
    isDirty?: boolean;
    name: string;
    description: string;
}

const QuizUpdateForm: React.FC<QuizUpdateProps> = ({ name, description, isDirty, onCancelClick, onSaveClick, onNameChanged, onDescriptionChanged }) => {

    return(
        <div className="flash-card-quiz-update">
            <h1>Edit quiz</h1>
            <div className="flash-card-quiz-update-inputs">
                <div className="input-group">
                    <label>Name</label>
                <input
                    value={name}
                    onChange={(e) => onNameChanged(e.target.value)} />
                </div>
                <div className="input-group">
                    <label>Description</label>
                    <textarea
                        value={description}
                        onChange={(e) => onDescriptionChanged(e.target.value)} />
                </div>
            </div>
            <div className="flash-card-quiz-update-buttons">   
                <button className="button" onClick={() => onCancelClick(false)}>Cancel</button>
                <button className={`button save-button ${isDirty? "active" : ""}`} onClick={onSaveClick}>Save</button>
            </div>
        </div>
    )
}

export default QuizUpdateForm;