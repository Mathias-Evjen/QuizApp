import { Delete } from "@mui/icons-material";

interface FlashCardEntryProps{
    onQuestionChanged: (flashCardId: number, newQuestion: string) => void; 
    onAnswerChanged: (flashCardId: number, newAnswer: string) => void;
    onDeletePressed: (flashCardId: number, quizQuestionNum: number) => void;
    flashCardId: number;
    quizQuestionNum: number;
    question: string;
    answer: string;
    errors?: {question?: string, answer?: string};
}

const FlashCardEntry: React.FC<FlashCardEntryProps> = ({ 
    flashCardId, quizQuestionNum, question, answer, errors, 
    onQuestionChanged, onAnswerChanged, onDeletePressed }) => {

    return(
        <div className="flash-card-entry">
            <div className="flash-card-entry-card">
                <div className="flash-card-entry-question">
                    <label>Question</label>
                    <input 
                        type="text"
                        value={question}
                        onChange={(e) => onQuestionChanged(flashCardId, e.target.value)} 
                        placeholder="Write a question..."/>
                    {errors?.question && <span className="error">{errors.question}</span>}
                </div>
                <div className="flash-card-entry-answer">
                    <label>Answer</label>
                    <input
                        type="text"
                        value={answer}
                        onChange={(e) => onAnswerChanged(flashCardId, e.target.value)} 
                        placeholder="Write an answer"/>
                    {errors?.answer && <span className="error">{errors.answer}</span>}
                </div>
            </div>
            <button className={"flash-card-entry-more-button"} onClick={() => onDeletePressed(flashCardId)}><Delete /></button>
        </div>
    )
}

export default FlashCardEntry;