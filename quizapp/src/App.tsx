import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import './App.css'
import FlashCardQuizPage from './flashCard/flashCards/FlashCardQuizPage'
import Quizzes from './flashCard/quizzes/Quizzes'
import ManageFlashCardQuiz from './flashCard/manage/ManageFlashCardQuiz'
import NavMenu from './shared/NavMenu'
import TestSide from './questions/TestSide'

const App: React.FC = () => {

  return (
    <>

      <Router>
        <NavMenu />
        <div className='site-container'>
          <Routes>
            <Route path="/" element={<Quizzes />} />
            <Route path="/flashCards" element={<Quizzes />} />
            <Route path='/flashCards/:id' element={<FlashCardQuizPage />} />
            <Route path='/flashCards/manage/:id' element={<ManageFlashCardQuiz />} />
            <Route path="/question" element={<TestSide />} />
          </Routes>
        </div>
      </Router>
    </>
  )
}

export default App
