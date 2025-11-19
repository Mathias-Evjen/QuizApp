import { useState, useEffect } from "react";
import * as QuizService from "./services/QuizService";
import "./style/Quiz.css";
import { Quiz} from "../types/quizCard";
import { useNavigate } from 'react-router-dom';


function QuizzesPage() {
  const navigate = useNavigate();

  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [loadingQuizzes, setLoadingQuizzes] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchQuizCards = async () => {
    setLoadingQuizzes(true);
    setError(null);

    try {
      const data = await QuizService.fetchQuizzes();
      setQuizzes(data);
      console.log(data);
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(`There was a problem fetching data: ${error.message}`);
      } else {
        console.error("Unknown error", error);
      }
      setError("Failed to fetch quizzes");
    } finally {
      setLoadingQuizzes(false);
    }
  };

  useEffect(() => {
    console.log("Fetching quizzes");
    fetchQuizCards();
  }, []);

  if (loadingQuizzes) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  const openQuiz = (quizId: number) => {
    navigate(`/quiz/${quizId}`);
  };

  const manageQuiz = (quizId: number) => {
    navigate(`/quiz/manage/${quizId}`);
  };

  return (
    <div>
        <div className="quizzes-wrapper">
          <h1>Quizzes</h1>
          <hr /><br />
          <div className="quiz-cards-container">
            {quizzes.length > 0 ? (
              quizzes.map((quiz) => (
                <div className="quiz-card-box" key={quiz.quizId}>
                  <p className="quiz-card-name">{quiz.name}</p>
                  <p className="quiz-card-desc">"{quiz.description}"</p>
                  <p className="quiz-card-num-questions">Questions: {quiz.numOfQuestions}</p>
                  <div className="quiz-card-buttons">
                    <button className="quiz-card-btn-open" onClick={() => openQuiz(quiz.quizId!)}>Open</button>
                    <button className="quiz-card-btn-manage" onClick={() => manageQuiz(quiz.quizId!)}>Manage</button>
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