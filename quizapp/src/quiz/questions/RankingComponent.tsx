import { useState, useEffect } from "react";

interface RankingProps {
  handleAnswer: (rankingId: number, newAnswer: string) => void;
  rankingId: number;
  quizQuestionNum: number;
  questionItems: string[];
  question: string;
  userAnswer: string | undefined;
}

const RankingComponent: React.FC<RankingProps> = ({
  rankingId,
  quizQuestionNum,
  questionItems,
  question,
  userAnswer,
  handleAnswer
}) => {

  const [splitQuestion, setSplitQuestion] = useState<string[]>(questionItems);

  // Tomme svarbokser, én for hver item
  const [answers, setAnswers] = useState<(string | null)[]>(Array(splitQuestion.length).fill(null));

  // Når man begynner å dra
  const handleDragStart = (e: React.DragEvent, value: string) => {
    e.dataTransfer.setData("text/plain", value);
  };

  // Når man drar over en answer-boks
  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault(); // nødvendig for å aktivere drop
  };

  // Når man slipper i en answer-boks
  const handleDrop = (e: React.DragEvent, index: number) => {
    e.preventDefault();
    if(!answers[index]){
      const value = e.dataTransfer.getData("text/plain");
  
      const newAnswers = [...answers];
      newAnswers[index] = value;
      setAnswers(newAnswers);
  
      const newSplitQuestion = splitQuestion.filter(item => item !== value);
      setSplitQuestion(newSplitQuestion);
  
    }
  };

  const handleRemoveAnswer = (itemValue: string, index: number) => {
    if (!itemValue) return;

    // Legg tilbake i item-container
    setSplitQuestion([...splitQuestion, itemValue]);

    // Fjern fra answer-boksen
    const newAnswers = [...answers];
    newAnswers[index] = "";
    setAnswers(newAnswers);
  }

  useEffect(() => {
    handleAnswer(rankingId, answers.join(","));
  }, [answers])

  return (
    <div className="ranking-card-wrapper">
      <h3>Question {quizQuestionNum}</h3>
      <p>{question}</p>
      <hr />

      <div className="ranking-question-answer-wrapper">

        {/* ANSWER CONTAINER */}
        <div className="ranking-answer-container">
          {answers.map((ans, i) => (
            <div
              key={i}
              className="ranking-item-wrapper answer-box"
              onDragOver={handleDragOver}
              onDrop={(e) => handleDrop(e, i)}
              onClick={() => ans && handleRemoveAnswer(ans, i)}
            >
              <label className="ranking-item-label">
                {ans || ""}
              </label>
              <label className="ranking-answer-counter-label">{i+1}.</label>
            </div>
          ))}
        </div>

        {/* ITEM CONTAINER */}
        <div className="ranking-item-container">
          {splitQuestion.map((key: string, i) => (
            <div
              key={i}
              className="ranking-item-wrapper draggable-item"
              draggable
              onDragStart={(e) => handleDragStart(e, key)}
            >
              <label className="ranking-item-label">{key}</label>
            </div>
          ))}
        </div>

      </div>
    </div>
  );
};

export default RankingComponent;
