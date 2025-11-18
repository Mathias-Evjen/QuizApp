import { useState } from "react";

interface FillInTheBlankProps{
    // handleAnswer: (userAnswer: string) => void;
    quizQuestionNum: number;
    question: string;
    userAnswer: string;
}

const FillInTheBlankComponent: React.FC<FillInTheBlankProps> = ({ quizQuestionNum, question, userAnswer }) => {
    const [testAnswer, setTestAnswer] = useState<string>(userAnswer);

    return(
        <div className="fill-in-the-blank-container">
            <h3>Question {quizQuestionNum}</h3>
            <p>{question}</p>

            <input 
                value={testAnswer}
                onChange={(e) => setTestAnswer(e.target.value)}
                placeholder="Fill in your answer..." />
        </div>
    )
}

export default FillInTheBlankComponent;