import { useState, useEffect } from "react";
import { Sequence } from "../types/sequence"
import "./Sequence.css";
import * as SequenceService from "../quiz/services/SequenceService";
import * as QuizService from "../quiz/services/QuizService";
import { useNavigate, useLocation } from "react-router-dom";

interface SequenceProps{
  quizQuestionNum: number,
  questionText: string,
  question: string,
  userAnswer: string
}

const SequenceCardQuizPage: React.FC<SequenceProps> = ({quizQuestionNum, questionText, question, userAnswer}) => {
  //const location = useLocation();
  //const navigate = useNavigate();
  //let {quiz, currentQuestionNum} = location.state || {};
  // const [sequenceCards, setSequenceCards] = useState<SequenceCard[]>([]);
  //const [sequenceQuestion, setSequenceQuestion] = useState<string>();
  const [splitQuestion, setSplitQuestion] = useState<string[]>([]);
  const [answer, setAnswer] = useState<string>(userAnswer);
  // const [loadingSequenceCards, setLoadingSequenceCards] = useState<boolean>(false);
  // const [error, setError] = useState<string | null>(null);
  
  useEffect(() => {
    setSplitQuestion(question.split(","));
  }, [question]);


    // const nextQuestion = () => {
    //     console.log("next (sequence)")
    //     currentQuestionNum = currentQuestionNum+1;
    //     const route = QuizService.getQuizRoute(quiz, currentQuestionNum);
    //     navigate(route, { state: {quiz, currentQuestionNum} })
    // }


  return (
    <div>
        {question && (
          <div className="sequence-card-wrapper">
              <div>
                {/* Spørsmålstekst */}
                <h3>{questionText}</h3>
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
        )}
    </div>
  );
}

export default SequenceCardQuizPage;
