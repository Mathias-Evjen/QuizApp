import { useState, useEffect } from "react";
import { Matching } from "../../types/matching";
import * as MatchingService from "../services/MatchingService";
import "../style/Matching.css";
import rightArrow from "../../shared/right-arrow.png";
import bin from "../../shared/bin.png";

interface MatchingManageFormProps {
    matchingId: number,
    incomingQuestionText: string,
    incomingCorrectAnswer: string
    onChange?: (updatedQuestion: { questionText: string; correctAnswer: string; isDirty: boolean }) => void;
}

const MatchingManageForm: React.FC<MatchingManageFormProps> = ({matchingId, incomingQuestionText, incomingCorrectAnswer, onChange}) => {
    const [splitQuestion, setSplitQuestion] = useState<{ keys: string[]; values: string[] }>({keys: [],values: [],});
    const [questionText, setQuestionText] = useState(incomingQuestionText);

    useEffect(() => {
        const combinedAnswer = splitQuestion.keys.map((k, i) => `${k},${splitQuestion.values[i]}`).join(",");
        onChange?.({ questionText, correctAnswer: combinedAnswer, isDirty: true });
    }, [splitQuestion, questionText]);

    useEffect(() => {
        setSplitQuestion(MatchingService.splitQuestion(incomingCorrectAnswer));
    }, []);

    return (
    <div className="matching-manage-form-wrapper" key={matchingId}>
        <div className="matching-manage-form-input-wrapper">
        <input
            id="matching-manage-form-questiontext"
            className="matching-manage-form-questiontext"
            value={questionText}
            onChange={(e) => setQuestionText(e.target.value)}
        />
        <label htmlFor="matching-manage-form-questiontext" className="matching-manage-form-label">
            Question text:
        </label>
        </div>
        <br />
        <div className="matching-manage-form-question-container">
        {splitQuestion?.keys.map((key, index) => (
            <div className="matching-manage-form-question-wrapper" key={index}>
            {/* Key input */}
            <input
                className="matching-manage-form-question-input"
                value={key}
                onChange={(e) => {
                const newKeys = [...splitQuestion.keys];
                newKeys[index] = e.target.value;
                setSplitQuestion({ ...splitQuestion, keys: newKeys });
                }}
            />

            <img
                src={rightArrow}
                alt="Right arrow icon"
                className="matching-manage-form-right-arrow"
            />

            {/* Value input */}
            <input
                className="matching-manage-form-question-input"
                value={splitQuestion.values[index]}
                onChange={(e) => {
                const newValues = [...splitQuestion.values];
                newValues[index] = e.target.value;
                setSplitQuestion({ ...splitQuestion, values: newValues });
                }}
            />

            {/* Delete pair */}
            <img
                src={bin}
                alt="Bin icon"
                className="matching-manage-form-bin"
                onClick={() => {
                const newKeys = splitQuestion.keys.filter((_, i) => i !== index);
                const newValues = splitQuestion.values.filter((_, i) => i !== index);
                setSplitQuestion({ keys: newKeys, values: newValues });
                }}
            />
            </div>
        ))}
        <br />
        </div>

        {/* Add new key/value pair */}
        <button
        className="matching-manage-form-btn-add"
        onClick={() => {
            setSplitQuestion({
            keys: [...splitQuestion.keys, ""],
            values: [...splitQuestion.values, ""],
            });
        }}
        >
        Add box
        </button>
    </div>
    );

}
export default MatchingManageForm;