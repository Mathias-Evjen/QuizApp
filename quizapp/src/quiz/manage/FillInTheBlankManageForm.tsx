import { Delete } from "@mui/icons-material";
import { useState, useEffect } from "react";

interface FillInTheBlankProps{
    // onQuestionChanged: (fillInTheBlankId: number, newQuestion: string) => void;
    // onAnswerChanged: (fillInTheBlankId: number, newAnswer: string) => void;
    // onDeletePressed: (fillInTheBlankId: number, quizQuestionNum: number) => void;
    fillInTheblankId?: number;
    question: string;
    answer: string;
    errors?: {question?: string, answer?: string};
    onChange?: (updatedQuestion: { question: string; correctAnswer: string; isDirty: boolean }) => void;
}

const FillInTheBlankManageFrom: React.FC<FillInTheBlankProps> = ({ 
    fillInTheblankId, question, answer,  errors, onChange
    // onQuestionChanged, onAnswerChanged, onDeletePressed 
    }) => {

    const [questionEdit, setQuestionEdit] = useState<string>(question);
    const [correctAnswerEdit, setCorrectAnswerEdit] = useState<string>(answer);

    useEffect(() => {
        onChange?.({ question: questionEdit, correctAnswer: correctAnswerEdit, isDirty: true });
    }, [questionEdit, correctAnswerEdit]);

    return(
        <div className="fill-in-the-blank-edit-container">
            <div className="fill-in-the-blank-edit-content">    
                
                <div className="fill-in-the-blank-edit-inputs">
                    <div className="fill-in-the-blank-edit-input-block">
                        <label>Question</label>
                        <input
                            type="text"
                            value={question}
                            // onChange={(e) => onQuestionChanged(fillInTheblankId, e.target.value)}
                            onChange={(e) => setQuestionEdit(e.target.value)}
                            placeholder="Write a question..." />
                    </div>
                    
                    <div className="fill-in-the-blank-edit-input-block">
                        <label>Answer</label>
                        <input
                            type="text"
                            value={answer}
                            // onChange={(e) => onAnswerChanged(fillInTheblankId, e.target.value)}
                            onChange={(e) => setCorrectAnswerEdit(e.target.value)}
                            placeholder="Write the answer..." />
                    </div>
                    
                </div>
            </div>

            {/* <button className={"flash-card-entry-delete-button"} onClick={() => onDeletePressed(fillInTheblankId, quizQuestionNum)}><Delete /></button> */}
        </div>
    )
}

export default FillInTheBlankManageFrom;