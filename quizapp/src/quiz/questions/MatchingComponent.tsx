import { useEffect, useState } from "react";
import * as MatchingService from "../services/MatchingService";
import "../style/Matching.css";

interface MatchingProps {
    handleAnswer: (matchingId: number, newAnswer: string) => void;
    matchingId: number;
    quizQuestionNum: number;
    question: string;
    questionText: string;
    userAnswer: string | undefined;
}

const MatchingComponent: React.FC<MatchingProps> = ({ matchingId, quizQuestionNum, question, questionText, userAnswer, handleAnswer }) => {
    const [splitQuestion, setSplitQuestion] = useState<{ keys: string[]; values: string[] } | null>(null);

    useEffect(() => {
        setSplitQuestion(MatchingService.splitQuestion(question));
    }, [question]);

    return(
        <div>
            <div className="matching-card-wrapper">
                <div>
                    {/* Spørsmålstekst */}
                    <h3>{questionText}</h3>
                    <hr />

                    <div className="matching-table-wrapper">
                    <table className="matching-table">
                        <tbody>
                        {splitQuestion && splitQuestion.keys.map((key:string, i:number) => (
                            <tr className="matching-table-tr" key={i}>
                            <td className="matching-table-keys-td">{key}</td>
                            <td className="matching-table-values-td">{splitQuestion.values[i]}</td>
                            </tr>
                        ))}
                        </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default MatchingComponent;