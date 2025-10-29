import { useState } from "react";
import { FlashCard } from "../../types/flashCard";
import { Save, Close } from "@mui/icons-material";

interface FlashCardEntryProps{
    onFlashCardChanged: (updatedCard: FlashCard) => void;
    flashCardId: number;
    quizQuestionNum: number;
    existingQuestion: string;
    existingAnswer: string;
    quizId: number;
}

const FlashCardEntry: React.FC<FlashCardEntryProps> = ({ flashCardId, quizQuestionNum, existingQuestion, existingAnswer, quizId, onFlashCardChanged }) => {
    const [question, setQuestion] = useState<string>(existingQuestion);
    const [answer, setAnswer] = useState<string>(existingAnswer);

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        const flashCard: FlashCard = { flashCardId, question, answer, quizQuestionNum, quizId};
        onFlashCardChanged(flashCard);
    }

    return(
        <div className="flash-card-entry">
            <p>{quizQuestionNum}</p>
            <div className="flash-card-entry-card">
                <div className="flash-card-entry-question">
                    <label>Question</label>
                    <input 
                        type="text"
                        value={question}
                        onChange={(e) => setQuestion(e.target.value)} />
                </div>
                <div className="flash-card-entry-answer">
                    <label>Answer</label>
                    <input
                        type="text"
                        value={answer}
                        onChange={(e) => setAnswer(e.target.value)} />
                </div>
            </div>
            <button className={"flash-card-entry-more-button"}>Save</button>
            <button className={"flash-card-entry-more-button"}><Close /></button>
        </div>
    )
}

export default FlashCardEntry;