import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import './App.css'
import FlashCardQuizPage from './flashCard/flashCards/FlashCardQuizPage'
import Quizzes from './flashCard/quizzes/Quizzes'
import ManageFlashCardQuiz from './flashCard/manage/ManageFlashCardQuiz'
import NavMenu from './shared/NavMenu'
import "./App.css";
import MatchingCardQuizPage from "./matching/MatchingCardQuizPage";
import SequenceCardQuizPage from "./sequence/SequenceCardQuizPage";
import RankingCardQuizPage from "./ranking/RankingCardQuizPage";
import QuizzesPage from "./quiz/QuizzesPage";
import QuizCreatePage from "./quiz/QuizCreatePage";
import QuizManagePage from "./quiz/QuizManagePage";
import QuizPage from './quiz/QuizPage'


const App: React.FC = () => {
  return (
    <>
      <Router>
        <NavMenu />
        <div className='site-container'>
          <Routes>
            <Route path="/" element={<QuizzesPage />} />
            <Route path="/quiz" element={<QuizzesPage />} />
            <Route path="/quiz/:id" element={<QuizPage />} />
            <Route path="/quizCreate" element={<QuizCreatePage />} />
            <Route path="/quizManage/:id" element={<QuizManagePage />} />
            <Route path="/flashCards" element={<Quizzes />} />
            <Route path='/flashCards/:id' element={<FlashCardQuizPage />} />
            <Route path='/flashCards/manage/:id' element={<ManageFlashCardQuiz />} />
            {/* <Route path="/matchingQuiz" element={<MatchingCardQuizPage />} />
            <Route path="/sequenceQuiz" element={<SequenceCardQuizPage />} />
            <Route path="/rankingQuiz" element={<RankingCardQuizPage />} /> */}
          </Routes>
        </div>
      </Router>
    </>
  )
}

export default App;
