import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";

import { AuthProvider } from "./auth/AuthContext";
import ProtectedRoute from "./auth/ProtectedRoute";

import NavMenu from "./shared/NavMenu";
import LoginPage from "./auth/LoginPage";
import RegisterPage from "./auth/RegisterPage";

/* QUIZ SYSTEM */
import QuizzesPage from "./quiz/QuizzesPage";
import QuizPage from "./quiz/QuizPage";
import QuizCreatePage from "./quiz/QuizCreatePage";
import QuizManagePage from "./quiz/manage/QuizManagePage";
import QuizResultPage from "./quiz/QuizResultPage";

/* FLASHCARD SYSTEM */
import FlashCardQuizPage from "./flashCard/flashCards/FlashCardQuizPage";
import FlashCardQuizzesPage from "./flashCard/quizzes/Quizzes";
import ManageFlashCardQuiz from "./flashCard/manage/ManageFlashCardQuiz";

/* HOME PAGE */
import HomePage from "./home/HomePage";


const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <NavMenu />

        <div className="site-container">
          <Routes>

            {/* PUBLIC */}
            <Route path="/" element={<HomePage />} />

            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            {/* QUIZ */}
            <Route path="/quiz" element={<QuizzesPage />} />
            <Route path="/quiz/:id" element={<QuizPage />} />
            <Route path="/quiz/:id/result" element={<QuizResultPage />} />

            {/* FLASHCARDS */}
            <Route path="/flashCards" element={<FlashCardQuizzesPage />} />
            <Route path="/flashCards/:id" element={<FlashCardQuizPage />} />


            {/* PROTECTED ROUTES */}
            <Route element={<ProtectedRoute />}>
              {/* QUIZ ADMIN */}
              <Route path="/quiz/manage/:id" element={<QuizManagePage />} />
              <Route path="/quizCreate" element={<QuizCreatePage />} />

              {/* FLASHCARD ADMIN */}
              <Route path="/flashCards/manage/:id" element={<ManageFlashCardQuiz />} />
            </Route>

            <Route path="*" element={<Navigate to="/" replace />} />

          </Routes>
        </div>
      </Router>
    </AuthProvider>
  );
};

export default App;
