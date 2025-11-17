import { Delete } from "@mui/icons-material";
import { useState } from "react";

interface FillInTheBlankProps{
    onQuestionChanged: (fillInTheBlankId: number, newQuestion: string) => void;
    onAnswerChanged: (fillInTheBlankId: number, newAnswer: string) => void;
    onDeletePressed: (fillInTheBlankId: number, quizQuestionNum: number) => void;
    fillInTheblankId: number;
    quizQuestionNum: number;
    question: string;
    answer: string;
    errors?: {question?: string, answer?: string}
}

const FillInTheBlankEdit: React.FC<FillInTheBlankProps> = ({ 
    fillInTheblankId, quizQuestionNum, question, answer,  errors,
    onQuestionChanged, onAnswerChanged, onDeletePressed }) => {

    return(
        <div className="fill-in-the-blank-edit-container">
            <div className="fill-in-the-blank-edit-content">
                <h2>Question {quizQuestionNum}</h2>
                
                <div className="fill-in-the-blank-edit-inputs">
                    <div className="fill-in-the-blank-edit-input-block">
                        <label>Question</label>
                        <input
                            type="text"
                            value={question}
                            onChange={(e) => onQuestionChanged(fillInTheblankId, e.target.value)}
                            placeholder="Write a question..." />
                    </div>
                    
                    <div className="fill-in-the-blank-edit-input-block">
                        <label>Answer</label>
                        <input
                            type="text"
                            value={answer}
                            onChange={(e) => onAnswerChanged(fillInTheblankId, e.target.value)}
                            placeholder="Write the answer..." />
                    </div>
                    
                </div>
            </div>

            <button className={"flash-card-entry-delete-button"} onClick={() => onDeletePressed(fillInTheblankId, quizQuestionNum)}><Delete /></button>
        </div>
    )
}

export default FillInTheBlankEdit;