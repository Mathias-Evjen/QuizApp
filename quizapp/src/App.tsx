import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import './App.css'
import Quizzes from './flashCard/Quizzes'
import FlashCardQuizPage from './flashCard/FlashCardQuizPage'

const App: React.FC = () => {

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Quizzes />} />
        <Route path="/flashCardQuizzes" element={<Quizzes />} />
        <Route path='flashCardQuiz/:id' element={<FlashCardQuizPage />} />
      </Routes>
    </Router>
  )
}

export default App
