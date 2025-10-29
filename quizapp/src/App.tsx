import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import './App.css'
import FlashCardQuizPage from './flashCard/flashCards/FlashCardQuizPage'
import Quizzes from './flashCard/quizzes/Quizzes'
import ManageFlashCardQuiz from './flashCard/manage/ManageFlashCardQuiz'

const App: React.FC = () => {

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Quizzes />} />
        <Route path="/flashCardQuizzes" element={<Quizzes />} />
        <Route path='/flashCardQuiz/:id' element={<FlashCardQuizPage />} />
        <Route path='/manageFlashCardQuiz/:id' element={<ManageFlashCardQuiz />} />
      </Routes>
    </Router>
  )
}

export default App
