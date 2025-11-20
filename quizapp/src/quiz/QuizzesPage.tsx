import { useState, useEffect } from "react";
import { Quiz} from "../types/quizCard";
import { useNavigate } from 'react-router-dom';
import { useAuth } from "../auth/AuthContext";
import * as QuizService from "./services/QuizService";
import SearchBar from "../shared/SearchBar";
import "./style/Quiz.css";
import QuizCard from "./QuizCard";


function QuizzesPage() {
  const navigate = useNavigate();

  const [quizzes, setQuizzes] = useState<Quiz[]>([]);
  const [loadingQuizzes, setLoadingQuizzes] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const [query, setQuery] = useState<string>("");
  const filteredQuizzes = quizzes.filter(quiz => quiz.name.toLocaleLowerCase().includes(query.toLocaleLowerCase()) || quiz.description.toLocaleLowerCase().includes(query.toLocaleUpperCase()));

  const { user } = useAuth();

  const fetchQuizzes = async () => {
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
    fetchQuizzes();
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
          <SearchBar query={query} placeholder="Search for a quiz" handleSearch={setQuery}/>
          <hr /><br />
          <div className="quiz-cards-container">
            {quizzes.length === 0 ? (
              <h3>There are no quizzes to show</h3>
            ) : filteredQuizzes.length === 0 ? (
              <h3>There are no quizzes matching "{query}"</h3>
            ) : (
              filteredQuizzes.map((quiz) => (
                <QuizCard
                  key={quiz.quizId}
                  quizId={quiz.quizId!}
                  name={quiz.name}
                  description={quiz.description}
                  numOfQuestions={quiz.numOfQuestions!}
                  openManageQuiz={manageQuiz} 
                  user={user} />
              ))
            )}
          </div>
          {user && (
            <button className="quiz-create" onClick={() => navigate("/quizCreate")}>Create</button>
          )}
        </div>
    </div>
  )
}

export default QuizzesPage