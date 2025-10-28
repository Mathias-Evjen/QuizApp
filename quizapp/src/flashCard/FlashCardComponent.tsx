
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
                <div className={`flash-card-inner ${showAnswer ? "flipped" : ""}`}>
                    <div className="flash-card-front">
                        <div className="flash-card-QA-label">
                            <h5>Q</h5>
                        </div>
                        <h3>{question}</h3>
                    </div>
                    <div className="flash-card-back">
                        <div className="flash-card-QA-label">
                            <h5>A</h5>
                        </div>
                        <h3>{answer}</h3>
                    </div>
                </div>
            </div>
        </>
    )
}

export default FlashCardComponent;