import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";
import FlashCardQuizPage from "./flashCard/FlashCardQuizPage";
import Quizzes from "./flashCard/Quizzes";
import MatchingCardQuizPage from "./matching/MatchingCardQuizPage";

const App: React.FC = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<MatchingCardQuizPage />} />
        <Route path="/flashCardQuizzes" element={<Quizzes />} />
        <Route path="flashCardQuiz/:id" element={<FlashCardQuizPage />} />
      </Routes>
    </Router>
  );
};

export default App;
