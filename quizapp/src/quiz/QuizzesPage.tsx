import { useState, useEffect } from "react";
import * as QuizService from "./QuizService";
import "./Quiz.css";
import { Quiz} from "../types/quizCard";
import { useNavigate } from 'react-router-dom';


function QuizzesPage() {
  const navigate = useNavigate();

  const [quizCards, setQuizCards] = useState<Quiz[]>([]);
  const [loadingQuizCards, setLoadingQuizCards] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchQuizCards = async () => {
    setLoadingQuizCards(true);
    setError(null);

    try {
      const data = await QuizService.fetchQuizzes();
      setQuizCards(data);
      console.log(data);
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(`There was a problem fetching data: ${error.message}`);
      } else {
        console.error("Unknown error", error);
      }
      setError("Failed to fetch quiz cards");
    } finally {
      setLoadingQuizCards(false);
    }
  };

  useEffect(() => {
    console.log("Fetching quiz cards");
    fetchQuizCards();
  }, []);

  if (loadingQuizCards) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div>
        <div className="quizzes-wrapper">
          <h1>Quizzes</h1>
          <hr /><br />
          <div className="quiz-cards-container">
            {quizCards.length > 0 ? (
              quizCards.map((card) => (
                <div className="quiz-card-box" key={card.quizId}>
                  <p className="quiz-card-name">{card.name}</p>
                  <p className="quiz-card-desc">"{card.description}"</p>
                  <p className="quiz-card-num-questions">Questions: {card.numOfQuestions}</p>
                  <div className="quiz-card-buttons">
                    <button className="quiz-card-btn-open">Open</button>
                    <button className="quiz-card-btn-manage" onClick={() => navigate("/quizManage", {state: card})}>Manage</button>
                  </div>
                </div>
              ))
            ) : (
              <h3>No quizzes found.</h3>
            )}
          </div>
          <button className="quiz-create" onClick={() => navigate("/quizCreate")}>Create</button>
        </div>
    </div>
  )
}

export default QuizzesPage