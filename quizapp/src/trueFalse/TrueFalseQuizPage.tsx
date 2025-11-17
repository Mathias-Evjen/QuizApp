import { useState, useEffect } from "react";
import { TrueFalse } from "../types/trueFalse";
import "./TrueFalse.css";
import * as QuizService from "../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";

function TrueFalseQuizPage() {
  const location = useLocation();
  const navigate = useNavigate();

  let { quiz, currentQuestionNum } = location.state || {};

  const [trueFalseCard, setTrueFalseCard] = useState<TrueFalse>();
  const [selectedAnswer, setSelectedAnswer] = useState<boolean | null>(null);

  useEffect(() => {
    if (quiz && currentQuestionNum) {
      const tfObject = quiz.allQuestions[currentQuestionNum - 1];

      const tfCardObject: TrueFalse = {
        trueFalseId: tfObject.id,
        question: tfObject.question,
        correctAnswer: tfObject.correctAnswer,
        quizId: tfObject.quizId,
        quizQuestionNum: tfObject.quizQuestionNum,
      };

      setTrueFalseCard(tfCardObject);
      setSelectedAnswer(null);
    }
  }, [quiz, currentQuestionNum]);

  const nextQuestion = () => {
    console.log("next (true/false)");
    currentQuestionNum = currentQuestionNum + 1;
    const route = QuizService.getQuizRoute(quiz, currentQuestionNum);
    navigate(route, { state: { quiz, currentQuestionNum } });
  };

  return (
    <div className="tf-quiz-wrapper">
      <br /><br />
      {trueFalseCard && (
        <div className="tf-card">
          <div key={trueFalseCard.trueFalseId}>
            <h3>{trueFalseCard.question}</h3>
            <hr />
            <div className="tf-options-wrapper">
              <label className="tf-option">
                <input type="radio" name="tf" checked={selectedAnswer === true} onChange={() => setSelectedAnswer(true)}/>
                True
              </label>
              <label className="tf-option">
                <input type="radio" name="tf" checked={selectedAnswer === false} onChange={() => setSelectedAnswer(false)}/>
                False
              </label>
            </div>
          </div>
          <button className="tf-next-btn" onClick={nextQuestion}>
            Next question
          </button>
        </div>
      )}
    </div>
  );
}

export default TrueFalseQuizPage;
