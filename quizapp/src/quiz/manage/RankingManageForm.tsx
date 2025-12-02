import { useState, useEffect } from "react";
import rightArrow from "../../assets/right-arrow.png";
import bin from "../../assets/bin.png";
import "../style/Ranking.css"

interface RankingManageFormProps {
    rankedId?: number;
    incomingQuestion: string;
    incomingCorrectAnswer: string;
    errors?: {question?: string; length?: string; answer?: string; blankPos?: number[]};
    onChange?: (updatedQuestion: { question: string; correctAnswer: string; isDirty: boolean }) => void;
}

const RankingManageForm: React.FC<RankingManageFormProps> = ({rankedId, incomingQuestion, incomingCorrectAnswer, errors, onChange}) => {
    const [splitQuestion, setSplitQuestion] = useState<string[]>(incomingCorrectAnswer.split(","));
    const [question, setQuestion] = useState(incomingQuestion);

    useEffect(() => {
        const combinedAnswer = splitQuestion.join(",");
        onChange?.({ question, correctAnswer: combinedAnswer, isDirty: true });
    }, [splitQuestion, question]);

    return(
        <div className="ranking-manage-form-wrapper" key={rankedId}>
            <div className="ranking-manage-form-input-wrapper">
                <input id="ranking-manage-form-questiontext" className="ranking-manage-form-questiontext" value={question} onChange={(e) => setQuestion(e.target.value)} />
                <label htmlFor="ranking-manage-form-questiontext" className="ranking-manage-form-label">Question text: </label>
                {errors?.question && <span className="error">{errors.question}</span>}
            </div>
            <br/>
            <div className="ranking-manage-form-question-container">
                {splitQuestion.map((key: string, index: number) => (
                    <div className="ranking-manage-form-question-wrapper" key={index}>
                        <input className="ranking-manage-form-question-input" value={key} 
                        onChange={(e) => {
                            const newArr = [...splitQuestion];
                            newArr[index] = e.target.value;
                            setSplitQuestion(newArr);
                        }}/>
                        <img src={rightArrow} alt="Right arrow icon" className="ranking-manage-form-right-arrow" />
                        <img src={bin} alt="Bin icon" className="ranking-manage-form-bin" 
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
            <button className="ranking-manage-form-btn-add" onClick={() => setSplitQuestion([...splitQuestion, ""])}>Add box</button>
        </div>
    )
}
export default RankingManageForm;