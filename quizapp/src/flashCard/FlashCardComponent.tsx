
interface FlashCardProps{
    question: string;
    answer: string;
    showAnswer: boolean
    toggleAnswer: () => void;
}

const FlashCardComponent: React.FC<FlashCardProps> = ({ question, answer, showAnswer, toggleAnswer }) => {
    
    return(
        <>
            <div className="flash-card" onClick={toggleAnswer}>
                <div className="flash-card-QA-label">
                    <h5>{showAnswer ? "A" : "Q"}</h5>
                </div>
                <h3>{showAnswer ? answer : question}</h3>
            </div>
        </>
    )
}

export default FlashCardComponent;