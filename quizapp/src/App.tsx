import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import './App.css'
import FlashCardQuizPage from './flashCard/FlashCardQuizPage'
import Quizzes from './flashCard/Quizzes'
import MultipleChoiceQuizPage from './multipleChoice/MultipleChoiceQuizPage'
import ManageMultipleChoiceQuizPage from './multipleChoice/manage/ManageMultipleChoiceQuizPage'
import ManageTrueFalseQuizPage from './trueFalse/manage/ManageTrueFalseQuizPage'


const App: React.FC = () => {

  return (
    <Router>
      <Routes>
        <Route path="/" element={<MultipleChoiceQuizPage />} />
        <Route path="/flashCardQuizzes" element={<Quizzes />} />
        <Route path='flashCardQuiz/:id' element={<FlashCardQuizPage />} />
        <Route path="/multipleChoiceQuiz/:quizId" element={<MultipleChoiceQuizPage />} />
        <Route path="/manageMultipleChoice/:id" element={<ManageMultipleChoiceQuizPage />} />
        <Route path="/manageTrueFalseQuiz/:id" element={<ManageTrueFalseQuizPage />} />
      </Routes>
    </Router>
  )
}

export default App
