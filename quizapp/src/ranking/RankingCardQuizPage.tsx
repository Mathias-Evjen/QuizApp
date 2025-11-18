import { useState, useEffect } from "react";
import { RankingCard } from "../types/ranking"
import "./Ranking.css";
import * as RankingService from "./RankingService";
import * as QuizService from "../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";


function RankingCardQuizPage() {
  const location = useLocation();
  const navigate = useNavigate();
  let {quiz, currentQuestionNum} = location.state || {};
  // const [rankingCards, setRankingCards] = useState<RankingCard[]>([]);
  const [rankingCard, setRankingCard] = useState<RankingCard>();
  // const [loadingRankingCards, setLoadingRankingCards] = useState<boolean>(false);
  // const [error, setError] = useState<string | null>(null);
  
    useEffect(() => {
    if (quiz && currentQuestionNum) {
        console.log(quiz);
        const rankingObject = quiz.allQuestions[currentQuestionNum - 1];

        const rankingCardObject = {
        rankingCardId: rankingObject.id,
        question: rankingObject.question,
        questionText: rankingObject.questionText,
        answer: rankingObject.answer,
        quizId: rankingObject.quizId,
        quizQuestionNum: rankingObject.quizQuestionNum
        };

        setRankingCard(rankingCardObject);
    }
    }, [quiz, currentQuestionNum]);


    const nextQuestion = () => {
    currentQuestionNum = currentQuestionNum+1;
    const route = QuizService.getQuizRoute(quiz, currentQuestionNum);
    navigate(route, { state: {quiz, currentQuestionNum} })
    }


  return (
    <div>
      <br/><br/>
        {rankingCard && (
          <div className="ranking-card-wrapper">
              <div key={rankingCard.rankingCardId}>
                {/* Spørsmålstekst */}
                <h3>{rankingCard.questionText}</h3>
                <hr />
              </div>
            <button className="ranking-card-next-btn" onClick={nextQuestion}>Next question</button>
          </div>
        )}
    </div>
  );
}

export default RankingCardQuizPage;
