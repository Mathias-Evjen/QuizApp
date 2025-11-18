import { useState, useEffect } from "react";
import { RankingCard } from "../../types/rankingCard";
import "../Ranking.css"
import * as QuizService from "../../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";
import rightArrow from "../../shared/right-arrow.png";
import bin from "../../shared/bin.png";

interface RankingManageFormProps {
    incomingRankingCard : RankingCard;
}

function RankingManageForm({ incomingRankingCard } : RankingManageFormProps) {
    console.log(incomingRankingCard)
    const [splitQuestion, setSplitQuestion] = useState<string[]>(incomingRankingCard.correctAnswer.split(","));
    const [rankingCard, setRankingCard] = useState<RankingCard>(incomingRankingCard);
    const [questionText, setQuestionText] = useState(incomingRankingCard.questionText);

    return(
        <div className="ranking-manage-form-wrapper">
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