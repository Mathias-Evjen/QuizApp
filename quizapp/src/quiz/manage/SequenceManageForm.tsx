import { useState, useEffect } from "react";
import "../style/Sequence.css"
import rightArrow from "../../assets/right-arrow.png";
import bin from "../../assets/bin.png";

interface SequenceManageFormProps {
    sequenceId?: number;
    incomingQuestion: string;
    incomingCorrectAnswer: string;
    errors?: {question?: string; length?: string; answer?: string; blankPos?: number[]};
    onChange?: (updatedQuestion: { question: string; correctAnswer: string; isDirty: boolean }) => void;
}

const SequenceManageForm: React.FC<SequenceManageFormProps> = ({sequenceId, incomingQuestion, incomingCorrectAnswer, errors, onChange}) => {
    const [splitQuestion, setSplitQuestion] = useState<string[]>(incomingCorrectAnswer ? incomingCorrectAnswer.split(",") : []);
    const [question, setQuestion] = useState(incomingQuestion);

    useEffect(() => {
        const combinedAnswer = splitQuestion.join(",");
        onChange?.({ question, correctAnswer: combinedAnswer, isDirty: true });
    }, [splitQuestion, question]);

    return(
        <div className="sequence-manage-form-wrapper" key={sequenceId}>
            <div className="sequence-manage-form-input-wrapper">
                <input id="sequence-manage-form-questiontext" className="sequence-manage-form-questiontext" value={question} onChange={(e) => setQuestion(e.target.value)} />
                <label htmlFor="sequence-manage-form-questiontext" className="sequence-manage-form-label">Question text: </label>
                {errors?.question && <span className="error">{errors.question}</span>}
            </div>
            <br/>
            <div className="sequence-manage-form-question-container">
                {splitQuestion.map((key:string, index:number) => (
                    <div className="sequence-manage-form-question-wrapper" key={index}>
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
                        {(errors?.answer && errors.blankPos?.includes(index)) && <span className="error">{errors.answer}</span>}
                    </div>
                ))}
                {errors?.length && <span className="error">{errors.length}</span>}
                <br/>
            </div>
            <button className="sequence-manage-form-btn-add" onClick={() => setSplitQuestion([...splitQuestion, ""])}>Add box</button>
        </div>
    )
}
export default SequenceManageForm;