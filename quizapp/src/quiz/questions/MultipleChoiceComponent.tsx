import { Option } from "../../types/multipleChoice";

interface MultipleChoiceProps {
    handleAnswer: (multiplechoiceId: number, newAnswer: string[]) => void;
    multipleChoiceId: number;
    quizQuestionNum: number;
    question: string;
    userAnswer: string[] | undefined;
    options: Option[]
}


const MultipleChoiceComponent: React.FC<MultipleChoiceProps> = ({ multipleChoiceId, quizQuestionNum, question, userAnswer, options, handleAnswer }) => {
    const answers = userAnswer ?? [];

    const toggleAnswer = (text: string) => {
        let updated: string[];

        if (answers.includes(text)) {
            updated = answers.filter(t => t !== text);
        } else {
            updated = [...answers, text];
        }

        handleAnswer(multipleChoiceId, updated);
    };

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
                                <input type="checkbox" name={`mc_${multipleChoiceId}`} checked={answers.includes(opt.text)} onChange={() => toggleAnswer(opt.text)} />
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