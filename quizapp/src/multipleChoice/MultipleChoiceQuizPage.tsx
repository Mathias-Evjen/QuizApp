import { useState, useEffect } from "react";
import { MultipleChoice } from "../types/multipleChoice";
import "../quiz/style/MultipleChoice.css";
import * as QuizService from "../quiz/services/QuizService";
import { useNavigate, useLocation } from "react-router-dom";

function MultipleChoiceQuizPage() {
  const location = useLocation();
  const navigate = useNavigate();

  let { quiz, currentQuestionNum } = location.state || {};

  const [questionObj, setQuestionObj] = useState<MultipleChoice>();
  const [selectedOption, setSelectedOption] = useState<number | null>(null);

  // useEffect(() => {
  //   if (quiz && currentQuestionNum) {
  //     const mcObject = quiz.allQuestions[currentQuestionNum - 1];

  //     const mcCard: MultipleChoice = {
  //       multipleChoiceId: mcObject.id,
  //       question: mcObject.question,
  //       options: mcObject.options,
  //       quizId: mcObject.quizId,
  //       quizQuestionNum: mcObject.quizQuestionNum
  //     };

  //     setQuestionObj(mcCard);
  //     setSelectedOption(null);
  //   }
  // }, [quiz, currentQuestionNum]);


  const nextQuestion = () => {
    console.log("next (multiple choice)");
    currentQuestionNum = currentQuestionNum + 1;

    const route = QuizService.getQuizRoute(quiz, currentQuestionNum);
    navigate(route, { state: { quiz, currentQuestionNum } });
  };


  return (
    <div className="mc-quiz-wrapper">
      <br /><br />
      {questionObj && (
        <div className="mc-card">
          <h3>{questionObj.question}</h3>
          <hr />
          <ul className="multiple-choice-options">
            {questionObj.options.map((opt, index) => (
              <li key={index}>
                <label className="mc-option">
                  <input type="radio" name={`mc_${questionObj.multipleChoiceId}`} checked={selectedOption === index} onChange={() => setSelectedOption(index)} />
                  {opt.text}
                </label>
              </li>
            ))}
          </ul>
          <button className="mc-next-btn" onClick={nextQuestion}>
            Next question
          </button>
        </div>
      )}
    </div>
  );
}

export default MultipleChoiceQuizPage;
