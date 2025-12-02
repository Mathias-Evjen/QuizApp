import "../style/FillInTheBlank.css";

interface FillInTheBlankProps{
    handleAnswer: (fibId: number, newAnswer: string) => void;
    fillInTheBlankId: number;
    quizQuestionNum: number;
    question: string;
    userAnswer: string;
}

const FillInTheBlankComponent: React.FC<FillInTheBlankProps> = ({ fillInTheBlankId, quizQuestionNum, question, userAnswer, handleAnswer }) => {

    return(
        <div className="fill-in-the-blank-container">
            <h3>Question {quizQuestionNum}</h3>
            <p>{question}</p>
            <hr />
            <input 
                value={userAnswer}
                onChange={(e) => handleAnswer(fillInTheBlankId, e.target.value)}
                placeholder="Fill in your answer..." />
        </div>
    )
}

export default FillInTheBlankComponent;