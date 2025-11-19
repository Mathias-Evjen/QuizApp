import { useState, useEffect } from "react";
import { Ranking } from "../types/ranking"
import "./Ranking.css";
import * as RankingService from "../quiz/services/RankingService";
import * as QuizService from "../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";

interface RankingProps{
  quizQuestionNum: number,
  questionText: string,
  question: string,
  userAnswer: string
}

const RankingCardQuizPage: React.FC<RankingProps> = ({quizQuestionNum, questionText, question, userAnswer}) => {
  //const location = useLocation();
  //const navigate = useNavigate();
  //let {quiz, currentQuestionNum} = location.state || {};
  // const [rankingCards, setRankingCards] = useState<RankingCard[]>([]);
  const [splitQuestion, setSplitQuestion] = useState<string[]>([]);
  const [answer, setAnswer] = useState<string>(userAnswer);
  // const [loadingRankingCards, setLoadingRankingCards] = useState<boolean>(false);
  // const [error, setError] = useState<string | null>(null);
  
  useEffect(() => {
    setSplitQuestion(question.split(","));
  }, [question]);


    // const nextQuestion = () => {
    // currentQuestionNum = currentQuestionNum+1;
    // const route = QuizService.getQuizRoute(quiz, currentQuestionNum);
    // navigate(route, { state: {quiz, currentQuestionNum} })
    // }


  return (
    <div>
        {question && (
          <div className="ranking-card-wrapper">
              <div>
                {/* Spørsmålstekst */}
                <h3>{questionText}</h3>
                <hr />
              </div>
          </div>
        )}
    </div>
  );
}

export default RankingCardQuizPage;
