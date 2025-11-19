import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import './App.css'
import FlashCardQuizPage from './flashCard/flashCards/FlashCardQuizPage'
import Quizzes from './flashCard/quizzes/Quizzes'
import ManageFlashCardQuiz from './flashCard/manage/ManageFlashCardQuiz'
import NavMenu from './shared/NavMenu'
import "./App.css";
import QuizzesPage from "./quiz/QuizzesPage";
import QuizCreatePage from "./quiz/QuizCreatePage";
import LoginPage from "./auth/LoginPage";
import RegisterPage from "./auth/RegisterPage";
import QuizPage from './quiz/QuizPage'
import QuizManagePage from './quiz/manage/QuizManagePage'


const App: React.FC = () => {
  return (
    <>
      <Router>
        <NavMenu />
        <div className='site-container'>
          <Routes /*element={<ProtectedRoute />} Sjekker Auth. Må putte dette rundt riktige ting*/>
            <Route path="/" element={<QuizzesPage />} />
            <Route path="/quiz" element={<QuizzesPage />} />
            <Route path="/quiz/:id" element={<QuizPage />} />
            <Route path="/quiz/manage/:id" element={<QuizManagePage />} />
            <Route path="/quiz/create" element={<QuizCreatePage />} />
            <Route path="/flashCards" element={<Quizzes />} />
            <Route path='/flashCards/:id' element={<FlashCardQuizPage />} />
            <Route path='/flashCards/manage/:id' element={<ManageFlashCardQuiz />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            {/* <Route path="/matchingQuiz" element={<MatchingCardQuizPage />} />
            <Route path="/quizMultipleChoice" element={<MultipleChoiceQuizPage />} />
            <Route path="/manageMultipleChoice/:id" element={<ManageMultipleChoiceQuizPage />} />
            <Route path="/manageTrueFalseQuiz/:id" element={<ManageTrueFalseQuizPage />} />
            <Route path="/sequenceQuiz" element={<SequenceCardQuizPage />} />
            <Route path="/rankingQuiz" element={<RankingCardQuizPage />} /> */}
            {/* <Route path="/quizTrueFalse" element={<TrueFalseQuizPage />} /> */}
          </Routes>
        </div>
      </Router>
    </>
  )
}

export default App;
