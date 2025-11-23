import { useState, useEffect } from "react";
import { MultipleChoice, Option } from "../../types/multipleChoice";
import "../style/MultipleChoice.css";

interface MultipleChoiceManageFormProps {
    multipleChoiceId: number,
    incomingQuestion: string,
    incomingOptions: Option[],
    onChange?: (updatedQuestion: { question: string; options: Option[]; isDirty: boolean }) => void;
}

function MultipleChoiceManageForm({ multipleChoiceId, incomingQuestion, incomingOptions, onChange }: MultipleChoiceManageFormProps) {

    const safeOptions = Array.isArray(incomingOptions) ? incomingOptions : [];

    const [questionText, setQuestionText] = useState(incomingQuestion);
    const [options, setOptions] = useState([...safeOptions]);

    const handleOptionChange = (index: number, value: string) => {
        const updated = [...options];
        updated[index].text = value;
        setOptions(updated);
    };

    const handleCorrectToggle = (index: number) => {
        const updated = [...options];
        updated[index].isCorrect = !updated[index].isCorrect;
        setOptions(updated);
    };

    const removeOption = (index: number) => {
        setOptions(options.filter((_, i) => i !== index));
    };

    const addOption = () => {
        setOptions([...options, { text: "", isCorrect: false }]);
    };

    useEffect(() => {
        onChange?.({ question: questionText, options, isDirty: true });
    }, [questionText, options]);

    return (
        <div className="multiplechoice-manage-form-wrapper">
            <div className="multiplechoice-manage-form-input-wrapper">
                <input className="multiplechoice-manage-form-questiontext" value={questionText} onChange={(e) => setQuestionText(e.target.value)} />
                <label className="multiplechoice-manage-form-label">Question text:</label>
            </div>

            <div className="multiplechoice-manage-form-options-container">
                {options.map((op, index) => (
                    <div className="multiplechoice-manage-form-option-wrapper" key={index}>
                        <input className="multiplechoice-manage-form-option-input" value={op.text} onChange={(e) => handleOptionChange(index, e.target.value)} />
                        <input type="checkbox" className="multiplechoice-manage-form-option-correct-checkbox" checked={op.isCorrect} onChange={() => handleCorrectToggle(index)} />
                        <button className="multiplechoice-manage-form-delete-btn" onClick={() => removeOption(index)}>
                            <img src="/src/shared/bin.png" alt="delete"/>
                        </button>
                    </div>
                ))}
            </div>
            <button className="multiplechoice-manage-form-btn-add" onClick={addOption}>
                Add option
            </button>
        </div>
    );
}

export default MultipleChoiceManageForm;
