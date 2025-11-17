import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import "./App.css";
import { AuthProvider } from "./auth/AuthContext";
import ProtectedRoute from "./auth/ProtectedRoute";
import FlashCardQuizPage from "./flashCard/FlashCardQuizPage";
import Quizzes from "./flashCard/Quizzes";
import MatchingCardQuizPage from "./matching/MatchingCardQuizPage";
import SequenceCardQuizPage from "./sequence/SequenceCardQuizPage";
import RankingCardQuizPage from "./ranking/RankingCardQuizPage";
import QuizzesPage from "./quiz/QuizzesPage";
import QuizCreatePage from "./quiz/QuizCreatePage";
import QuizManagePage from "./quiz/QuizManagePage";
import MultipleChoiceQuizPage from './multipleChoice/MultipleChoiceQuizPage'
import ManageMultipleChoiceQuizPage from './multipleChoice/ManageMultipleChoiceQuizPage'
import ManageTrueFalseQuizPage from './trueFalse/ManageTrueFalseQuizPage'
import TrueFalseQuizPage from "./trueFalse/TrueFalseQuizPage";
import LoginPage from "./auth/LoginPage";
import RegisterPage from "./auth/RegisterPage";
import NavMenu from "./shared/NavMenu"; 


const App: React.FC = () => {
  return (
    <Router>
      <NavMenu />
      <br /> <br />
      <Routes /*element={<ProtectedRoute />} Sjekker Auth. Må putte dette rundt riktige ting*/>
        <Route path="/" element={<QuizzesPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/quizCreate" element={<QuizCreatePage />} />
        <Route path="/quizManage" element={<QuizManagePage />} />
        <Route path="/flashCardQuizzes" element={<Quizzes />} />
        <Route path="flashCardQuiz/:id" element={<FlashCardQuizPage />} />
        <Route path="/matchingQuiz" element={<MatchingCardQuizPage />} />
        <Route path="/quizMultipleChoice" element={<MultipleChoiceQuizPage />} />
        <Route path="/manageMultipleChoice/:id" element={<ManageMultipleChoiceQuizPage />} />
        <Route path="/manageTrueFalseQuiz/:id" element={<ManageTrueFalseQuizPage />} />
        <Route path="/sequenceQuiz" element={<SequenceCardQuizPage />} />
        <Route path="/rankingQuiz" element={<RankingCardQuizPage />} />
        <Route path="/quizTrueFalse" element={<TrueFalseQuizPage />} />
      </Routes>
    </Router>
  );
};

export default App;
