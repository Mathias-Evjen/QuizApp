import { Option } from "../../types/multipleChoice";

interface MultipleChoiceProps {
    handleAnswer: (multiplechoiceId: number, newAnswer: string) => void;
    multipleChoiceId: number;
    quizQuestionNum: number;
    question: string;
    userAnswer: string | undefined;
    options: Option[]
}


const MultipleChoiceComponent: React.FC<MultipleChoiceProps> = ({ multipleChoiceId, quizQuestionNum, question, userAnswer, options, handleAnswer }) => {
    return (
        <div className="mc-quiz-wrapper">
            <br /><br />
            <div className="mc-card">
                <h3>Question {quizQuestionNum}</h3>
                <p>{question}</p>
                <hr />
                <ul className="multiple-choice-options">
                    {options.map((opt, index) => (
                        <li key={index}>
                            <label className="mc-option">
                                <input type="radio" name={`mc_${multipleChoiceId ?? `temp_${quizQuestionNum}`}`} checked={userAnswer === opt.text} onChange={() => handleAnswer(multipleChoiceId, opt.text)} />
                                {opt.text}
                            </label>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    )
}

export default MultipleChoiceComponent;