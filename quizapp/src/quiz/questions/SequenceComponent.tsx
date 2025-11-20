import { useEffect, useState } from "react";

interface SequenceProps {
    handleAnswer: (sequenceId: number, newAnswer: string) => void;
    sequenceId: number;
    quizQuestionNum: number;
    question: string;
    questionText: string;
    userAnswer: string | undefined;
}

const SequenceComponent: React.FC<SequenceProps> = ({ sequenceId, quizQuestionNum, question, questionText, userAnswer, handleAnswer }) => {
    const [splitQuestion, setSplitQuestion] = useState<string[]>([]);


    useEffect(() => {
        setSplitQuestion(question.split(","));
    }, [question]);

    return(
        <div>
            <div className="sequence-card-wrapper">
                <div>
                    {/* Spørsmålstekst */}
                    <h3>Question {quizQuestionNum}</h3>
                    <p>{questionText}</p>
                    <hr />
                    <div className="sequence-question-answer-wrapper">
                    <div className="sequence-answer-container">
                        {splitQuestion.map(() => (
                        <div className="sequence-item-wrapper">
                            <label className="sequence-item-label"></label>
                        </div>
                        ))}
                    </div>
                    <div className="sequence-item-container">
                        {splitQuestion.map((key:string) => (
                        <div className="sequence-item-wrapper">
                            <label className="sequence-item-label">{key}</label>
                        </div>
                        ))}
                    </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default SequenceComponent;