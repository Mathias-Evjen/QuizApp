import { useState, useEffect } from "react";
import { Ranking } from "../../types/ranking";
import "../Ranking.css"
import * as QuizService from "../../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";
import rightArrow from "../../shared/right-arrow.png";
import bin from "../../shared/bin.png";

interface RankingManageFormProps {
    rankedId: number
    incomingQuestionText: string,
    incomingCorrectAnswer: string,
    onChange?: (updatedQuestion: { questionText: string; correctAnswer: string; isDirty: boolean }) => void;
}

const RankingManageForm: React.FC<RankingManageFormProps> = ({rankedId, incomingQuestionText, incomingCorrectAnswer, onChange}) => {
    const [splitQuestion, setSplitQuestion] = useState<string[]>(incomingCorrectAnswer.split(","));
    const [questionText, setQuestionText] = useState(incomingQuestionText);

    useEffect(() => {
        const combinedAnswer = splitQuestion.join(",");
        onChange?.({ questionText, correctAnswer: combinedAnswer, isDirty: true });
    }, [splitQuestion, questionText]);

    return(
        <div className="ranking-manage-form-wrapper" key={rankedId}>
            <div className="ranking-manage-form-input-wrapper">
                <input id="ranking-manage-form-questiontext" className="ranking-manage-form-questiontext" value={questionText} onChange={(e) => setQuestionText(e.target.value)} />
                <label htmlFor="ranking-manage-form-questiontext" className="ranking-manage-form-label">Question text: </label>
            </div>
            <br/>
            <div className="ranking-manage-form-question-container">
                {splitQuestion.map((key:string, index:number) => (
                    <div className="ranking-manage-form-question-wrapper">
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
                    </div>
                ))}
                <br/>
            </div>
            <button className="ranking-manage-form-btn-add" onClick={() => setSplitQuestion([...splitQuestion, ""])}>Add box</button>
        </div>
    )
}
export default RankingManageForm;