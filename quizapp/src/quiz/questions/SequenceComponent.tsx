import { useEffect, useState } from "react";
import rightArrow from "../../assets/right-arrow.png";

interface SequenceProps {
  handleAnswer: (sequenceId: number, newAnswer: string) => void;
  sequenceId: number;
  quizQuestionNum: number;
  questionItems: string[];
  question: string;
  userAnswer: string | undefined;
}

const SequenceComponent: React.FC<SequenceProps> = ({
  sequenceId,
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
    handleAnswer(sequenceId, answers.join(","));
  }, [answers])

  return (
    <div className="sequence-card-wrapper">
      <h3>Question {quizQuestionNum}</h3>
      <p>{question}</p>
      <hr />

      <div className="sequence-question-answer-wrapper">

        {/* ANSWER CONTAINER */}
        <div className="sequence-answer-container">
          {answers.map((ans, i) => (
            <div
              key={i}
              className="sequence-item-wrapper answer-box"
              onDragOver={handleDragOver}
              onDrop={(e) => handleDrop(e, i)}
              onClick={() => ans && handleRemoveAnswer(ans, i)}
            >
              <label className="sequence-item-label">
                {ans || ""}
              </label>
              <img src={rightArrow} alt="Right arrow icon" className="sequence-answer-right-arrow" />
            </div>
          ))}
        </div>

        {/* ITEM CONTAINER */}
        <div className="sequence-item-container">
          {splitQuestion.map((key: string, i) => (
            <div
              key={i}
              className="sequence-item-wrapper draggable-item"
              draggable
              onDragStart={(e) => handleDragStart(e, key)}
            >
              <label className="sequence-item-label">{key}</label>
            </div>
          ))}
        </div>

      </div>
    </div>
  );
};

export default SequenceComponent;
