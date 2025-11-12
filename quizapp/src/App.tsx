import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";
import FlashCardQuizPage from "./flashCard/FlashCardQuizPage";
import Quizzes from "./flashCard/Quizzes";
import MatchingCardQuizPage from "./matching/MatchingCardQuizPage";
import QuizzesPage from "./quiz/QuizzesPage";
import QuizCreatePage from "./quiz/QuizCreatePage";
import QuizManagePage from "./quiz/QuizManagePage";
import NavMenu from "./shared/NavMenu";


const App: React.FC = () => {
  return (
    <Router>
      <NavMenu />
      <br /> <br />
      <Routes>
        <Route path="/" element={<QuizzesPage />} />
        <Route path="/quizCreate" element={<QuizCreatePage />} />
        <Route path="/quizManage" element={<QuizManagePage />} />
        <Route path="/flashCardQuizzes" element={<Quizzes />} />
        <Route path="flashCardQuiz/:id" element={<FlashCardQuizPage />} />
        <Route path="/matchingQuiz" element={<MatchingCardQuizPage />} />
      </Routes>
    </Router>
  );
};

export default App;
