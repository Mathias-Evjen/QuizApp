import { useState, useEffect } from "react";
import { SequenceCard } from "../types/sequence"
import "./Sequence.css";
import * as SequenceService from "./SequenceService";
import * as QuizService from "../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";


function SequenceCardQuizPage() {
  const location = useLocation();
  const navigate = useNavigate();
  let {quiz, currentQuestionNum} = location.state || {};
  // const [sequenceCards, setSequenceCards] = useState<SequenceCard[]>([]);
  const [sequenceCard, setSequenceCard] = useState<SequenceCard>();
  const [splitQuestion, setSplitQuestion] = useState<string[]>([]);
  // const [loadingSequenceCards, setLoadingSequenceCards] = useState<boolean>(false);
  // const [error, setError] = useState<string | null>(null);
  
    useEffect(() => {
    if (quiz && currentQuestionNum) {
        console.log(quiz);
        const sequenceObject = quiz.allQuestions[currentQuestionNum - 1];

        const sequenceCardObject = {
        sequenceCardId: sequenceObject.id,
        question: sequenceObject.question,
        questionText: sequenceObject.questionText,
        answer: sequenceObject.answer,
        correctAnswer: sequenceObject.correctAnswer,
        quizId: sequenceObject.quizId,
        quizQuestionNum: sequenceObject.quizQuestionNum
        };
        setSequenceCard(sequenceCardObject);
        setSplitQuestion(sequenceCardObject.question.split(","));
    }
    }, [quiz, currentQuestionNum]);


    const nextQuestion = () => {
        console.log("next (sequence)")
        currentQuestionNum = currentQuestionNum+1;
        const route = QuizService.getQuizRoute(quiz, currentQuestionNum);
        navigate(route, { state: {quiz, currentQuestionNum} })
    }


  return (
    <div>
      <br/><br/>
        {sequenceCard && (
          <div className="sequence-card-wrapper">
              <div key={sequenceCard.sequenceCardId}>
                {/* Spørsmålstekst */}
                <h3>{sequenceCard.questionText}</h3>
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
            <button className="sequence-card-next-btn" onClick={nextQuestion}>Next question</button>
          </div>
        )}
    </div>
  );
}

export default SequenceCardQuizPage;
