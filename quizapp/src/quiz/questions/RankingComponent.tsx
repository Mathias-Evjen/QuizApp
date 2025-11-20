import { useEffect, useState } from "react";

interface RankingProps {
    handleAnswer: (rankingId: number, newAnswer: string) => void;
    rankingId: number;
    quizQuestionNum: number;
    question: string;
    questionText: string;
    userAnswer: string | undefined;
}

const RankingComponent: React.FC<RankingProps> = ({ rankingId, quizQuestionNum, question, questionText, userAnswer, handleAnswer }) => {
    const [splitQuestion, setSplitQuestion] = useState<string[]>([]);
    
    useEffect(() => {
        setSplitQuestion(question.split(","));
      }, [question]);

    return(
        <div>
            <div className="ranking-card-wrapper">
                <div>
                    {/* Spørsmålstekst */}
                    <h3>Question {quizQuestionNum}</h3>
                    <p>{questionText}</p>
                    <hr />
                </div>
            </div>
        </div>
    )
}

export default RankingComponent;