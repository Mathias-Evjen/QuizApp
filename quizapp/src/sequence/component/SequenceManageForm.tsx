import { useState, useEffect } from "react";
import { Sequence } from "../../types/sequence";
import "../Sequence.css"
import * as QuizService from "../../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";
import rightArrow from "../../shared/right-arrow.png";
import bin from "../../shared/bin.png";

interface SequenceManageFormProps {
    sequenceId: number,
    incomingQuestionText: string,
    incomingCorrectAnswer: string
    onChange?: (updatedQuestion: { questionText: string; correctAnswer: string; isDirty: boolean }) => void;
}

const SequenceManageForm: React.FC<SequenceManageFormProps> = ({sequenceId, incomingQuestionText, incomingCorrectAnswer, onChange}) => {
    const [splitQuestion, setSplitQuestion] = useState<string[]>(incomingCorrectAnswer ? incomingCorrectAnswer.split(",") : []);
    const [questionText, setQuestionText] = useState(incomingQuestionText);

    useEffect(() => {
        const combinedAnswer = splitQuestion.join(",");
        onChange?.({ questionText, correctAnswer: combinedAnswer, isDirty: true });
    }, [splitQuestion, questionText]);

    return(
        <div className="sequence-manage-form-wrapper" key={sequenceId}>
            <div className="sequence-manage-form-input-wrapper">
                <input id="sequence-manage-form-questiontext" className="sequence-manage-form-questiontext" value={questionText} onChange={(e) => setQuestionText(e.target.value)} />
                <label htmlFor="sequence-manage-form-questiontext" className="sequence-manage-form-label">Question text: </label>
            </div>
            <br/>
            <div className="sequence-manage-form-question-container">
                {splitQuestion.map((key:string, index:number) => (
                    <div className="sequence-manage-form-question-wrapper">
                        <input className="sequence-manage-form-question-input" value={key} 
                        onChange={(e) => {
                            const newArr = [...splitQuestion];
                            newArr[index] = e.target.value;
                            setSplitQuestion(newArr);
                        }}/>
                        <img src={rightArrow} alt="Right arrow icon" className="sequence-manage-form-right-arrow" />
                        <img src={bin} alt="Bin icon" className="sequence-manage-form-bin" 
                        onClick={() => {
                            const updated = splitQuestion.filter((_, i) => i !== index);
                            setSplitQuestion(updated);
                        }} />
                    </div>
                ))}
                <br/>
            </div>
            <button className="sequence-manage-form-btn-add" onClick={() => setSplitQuestion([...splitQuestion, ""])}>Add box</button>
        </div>
    )
}
export default SequenceManageForm;