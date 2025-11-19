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

/* FLASHCARD SYSTEM */
import FlashCardQuizPage from "./flashCard/flashCards/FlashCardQuizPage";
import FlashCardQuizzesPage from "./flashCard/quizzes/Quizzes";
import ManageFlashCardQuiz from "./flashCard/manage/ManageFlashCardQuiz";

/* MATCHING */
import MatchingCardQuizPage from "./matching/MatchingCardQuizPage";

/* SEQUENCE + RANKING */
import SequenceCardQuizPage from "./sequence/SequenceCardQuizPage";
import RankingCardQuizPage from "./ranking/RankingCardQuizPage";

/* MULTIPLE CHOICE */
import MultipleChoiceQuizPage from "./multipleChoice/MultipleChoiceQuizPage";
import ManageMultipleChoiceQuizPage from "./multipleChoice/ManageMultipleChoiceQuizPage";

/* TRUE/FALSE */
import ManageTrueFalseQuizPage from "./trueFalse/ManageTrueFalseQuizPage";
import TrueFalseQuizPage from "./trueFalse/TrueFalseQuizPage";


const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <NavMenu />

        <div className="site-container">
          <Routes>

            {/* PUBLIC */}
            <Route path="/" element={<QuizzesPage />} />

            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            {/* QUIZ TAKE */}
            <Route path="/quiz/:id" element={<QuizPage />} />

            {/* FLASHCARDS */}
            <Route path="/flashCards" element={<FlashCardQuizzesPage />} />
            <Route path="/flashCards/:id" element={<FlashCardQuizPage />} />

            {/* QUESTION TYPES PUBLIC VIEW */}
            <Route path="/matchingQuiz" element={<MatchingCardQuizPage />} />
            {/* <Route path="/sequenceQuiz" element={<SequenceCardQuizPage />} /> */}
            {/* <Route path="/rankingQuiz" element={<RankingCardQuizPage />} /> */}
            <Route path="/quizMultipleChoice" element={<MultipleChoiceQuizPage />} />
            <Route path="/quizTrueFalse" element={<TrueFalseQuizPage />} />


            {/* PROTECTED ROUTES */}
            <Route element={<ProtectedRoute />}>
              {/* QUIZ ADMIN */}
              <Route path="/quiz/manage/:id" element={<QuizManagePage />} />
              <Route path="/quizCreate" element={<QuizCreatePage />} />

              {/* FLASHCARD ADMIN */}
              <Route path="/flashCards/manage/:id" element={<ManageFlashCardQuiz />} />

              {/* MULTIPLE CHOICE ADMIN */}
              <Route path="/manageMultipleChoice/:id" element={<ManageMultipleChoiceQuizPage />} />

              {/* TRUE/FALSE ADMIN */}
              <Route path="/manageTrueFalseQuiz/:id" element={<ManageTrueFalseQuizPage />} />
            </Route>

            <Route path="*" element={<Navigate to="/" replace />} />

          </Routes>
        </div>
      </Router>
    </AuthProvider>
  );
};

export default App;
