
interface FillInTheBlankProps{
    handleAnswer: (userAnswer: string) => void;
    quizQuestionNum: number;
    question: string;
    userAnswer: string;
}

const FillInTheBlankComponent: React.FC<FillInTheBlankProps> = ({ quizQuestionNum, question, userAnswer, handleAnswer }) => {


    return(
        <div className="question-container">
            <h3>Question {quizQuestionNum}</h3>
            <p>{question}</p>

            <input 
                value={userAnswer}
                onChange={() => handleAnswer(userAnswer)} />
        </div>
    )
}

export default FillInTheBlankComponent;