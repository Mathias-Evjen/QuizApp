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

const MatchingComponent: React.FC<MatchingProps> = ({
  matchingId,
  quizQuestionNum,
  question,
  questionText,
  userAnswer,
  handleAnswer
}) => {
  const [splitQuestion, setSplitQuestion] = useState<{ keys: string[]; values: string[] } | null>(MatchingService.splitQuestion(question));
  const [selectedKeyIndex, setSelectedKeyIndex] = useState<number | null>(null);

//   useEffect(() => {
//     setSplitQuestion(MatchingService.splitQuestion(question));
//   }, [question]);

  const handleKeyClick = (keyIndex: number) => {
    setSelectedKeyIndex(keyIndex); 
  };

  const handleValueClick = (valueIndex: number) => {
    if (selectedKeyIndex === null || !splitQuestion) return;

    const newValues = [...splitQuestion.values];
    [newValues[selectedKeyIndex], newValues[valueIndex]] = [newValues[valueIndex], newValues[selectedKeyIndex]];

    setSplitQuestion({ ...splitQuestion, values: newValues });
    setSelectedKeyIndex(null); 

    handleAnswer(matchingId, MatchingService.assemble({ keys: splitQuestion.keys, values: newValues }));
  };

  if (!splitQuestion) return null;

  return (
    <div className="matching-card-wrapper">
      <h3>Question {quizQuestionNum}</h3>
      <p>{questionText}</p>
      <hr />
        <div className="matching-grid">
        {splitQuestion.keys.map((key, i) => (
            <div className="matching-grid-row" key={i}>
            <div
                className={`matching-grid-key ${selectedKeyIndex === i ? "selected" : ""}`}
                onClick={() => handleKeyClick(i)}
            >
                {key}
            </div>

            <div
                className={`matching-grid-value ${selectedKeyIndex !== null ? "keySelected" : ""}`}
                onClick={() => handleValueClick(i)}
            >
                {splitQuestion.values[i]}
            </div>
            </div>
        ))}
        </div>
    </div>
  );
};

export default MatchingComponent;
