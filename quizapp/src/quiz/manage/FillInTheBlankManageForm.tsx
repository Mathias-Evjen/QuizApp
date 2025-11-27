import { useState, useEffect } from "react";

interface FillInTheBlankProps{
    fillInTheblankId?: number;
    question: string;
    answer: string;
    errors?: {question?: string, answer?: string};
    onChange?: (updatedQuestion: { question: string; correctAnswer: string; isDirty: boolean }) => void;
}

const FillInTheBlankManageFrom: React.FC<FillInTheBlankProps> = ({ 
    fillInTheblankId, question, answer,  errors, onChange
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
                            onChange={(e) => setQuestionEdit(e.target.value)}
                            placeholder="Write a question..." />
                        {errors?.question && <span className="error">{errors.question}</span>}
                    </div>
                    
                    <div className="fill-in-the-blank-edit-input-block">
                        <label>Answer</label>
                        <input
                            type="text"
                            value={answer}
                            onChange={(e) => setCorrectAnswerEdit(e.target.value)}
                            placeholder="Write the answer..." />
                        {errors?.answer && <span className="error">{errors.answer}</span>}
                    </div>
                    
                </div>
            </div>
        </div>
    )
}

export default FillInTheBlankManageFrom;