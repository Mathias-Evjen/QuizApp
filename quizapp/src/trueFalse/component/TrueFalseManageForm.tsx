import { useState } from "react";
import { TrueFalse } from "../../types/trueFalse";
import "../trueFalse.css";

interface TrueFalseManageFormProps {
    incomingTrueFalse: TrueFalse;
}

function TrueFalseManageForm({ incomingTrueFalse }: TrueFalseManageFormProps) {
    const [questionText, setQuestionText] = useState(incomingTrueFalse.question);
    const [correctAnswer, setCorrectAnswer] = useState<boolean>(incomingTrueFalse.correctAnswer);

    return (
        <div className="tf-manage-form-wrapper">
            <div className="tf-manage-form-input-wrapper">
                <input id="tf-manage-form-questiontext" className="tf-manage-form-questiontext" value={questionText} onChange={(e) => setQuestionText(e.target.value)} />
                <label htmlFor="tf-manage-form-questiontext" className="tf-manage-form-label">
                    Question text:
                </label>
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
