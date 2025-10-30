import { Close } from "@mui/icons-material";

interface FlashCardEntryProps{
    onQuestionChanged: (flashCardId: number, newQuestion: string) => void; 
    onAnswerChanged: (flashCardId: number, newAnswer: string) => void;
    onDeletePressed: (flashCardId: number) => void;
    flashCardId: number;
    quizQuestionNum: number;
    question: string;
    answer: string;
    quizId: number;
}

const FlashCardEntry: React.FC<FlashCardEntryProps> = ({ 
    flashCardId, quizQuestionNum, question, answer, quizId, 
    onQuestionChanged, onAnswerChanged, onDeletePressed }) => {

    return(
        <div className="flash-card-entry">
            <p>{quizQuestionNum}</p>
            <div className="flash-card-entry-card">
                <div className="flash-card-entry-question">
                    <label>Question</label>
                    <input 
                        type="text"
                        value={question}
                        onChange={(e) => onQuestionChanged(flashCardId, e.target.value)} 
                        placeholder="Write a question..."/>
                </div>
                <div className="flash-card-entry-answer">
                    <label>Answer</label>
                    <input
                        type="text"
                        value={answer}
                        onChange={(e) => onAnswerChanged(flashCardId, e.target.value)} 
                        placeholder="Write an answer"/>
                </div>
            </div>
            <button className={"flash-card-entry-more-button"} onClick={() => onDeletePressed(flashCardId)}><Close /></button>
        </div>
    )
}

export default FlashCardEntry;