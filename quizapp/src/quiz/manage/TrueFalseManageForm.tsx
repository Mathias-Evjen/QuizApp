import { useState, useEffect } from "react";
import "../style/TrueFalse.css";

interface TrueFalseManageFormProps {
    trueFalseId?: number;
    incomingQuestion: string;
    incomingCorrectAnswer: boolean;
    errors?: {question?: string};
    onChange?: (updatedQuestion: { question: string; correctAnswer: boolean; isDirty: boolean }) => void;
}

function TrueFalseManageForm({ trueFalseId, incomingQuestion, incomingCorrectAnswer, errors, onChange }: TrueFalseManageFormProps) {
    const [questionText, setQuestionText] = useState(incomingQuestion);
    const [correctAnswer, setCorrectAnswer] = useState<boolean>(incomingCorrectAnswer);

    useEffect(() => {
        onChange?.({ question: questionText, correctAnswer, isDirty: true });
    }, [questionText, correctAnswer]);


    return (
        <div className="tf-manage-form-wrapper">
            <div className="tf-manage-form-input-wrapper">
                <input id="tf-manage-form-questiontext" className="tf-manage-form-questiontext" value={questionText} onChange={(e) => setQuestionText(e.target.value)} />
                <label htmlFor="tf-manage-form-questiontext" className="tf-manage-form-label">
                    Question text:
                </label>
                {errors?.question && <span className="error">{errors.question}</span>}
            </div>
            <div className="tf-manage-form-answer-container">
                <button className={correctAnswer === true ? "tf-answer-btn tf-active" : "tf-answer-btn"} onClick={() => setCorrectAnswer(true)}>
                    True
                </button>
                <button className={correctAnswer === false ? "tf-answer-btn tf-active" : "tf-answer-btn"} onClick={() => setCorrectAnswer(false)}>
                    False
                </button>
            </div>
        </div>
    );
}

export default TrueFalseManageForm;
