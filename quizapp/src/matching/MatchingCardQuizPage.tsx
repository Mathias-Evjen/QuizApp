import { useState, useEffect } from "react";
import { MatchingCard } from "../types/matchingCard";
import "./Matching.css";
import * as MatchingService from "./MatchingService";
import * as QuizService from "../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";

//TODO: Finne ut en måte å oppdatere quiz objektet med svar

function MatchingCardQuizPage() {
  const location = useLocation();
  const navigate = useNavigate();
  let {quiz, currentQuestionNum} = location.state || {};
  // const [matchingCards, setMatchingCards] = useState<MatchingCard[]>([]);
  const [matchingCard, setMatchingCard] = useState<MatchingCard>();
  // const [loadingMatchingCards, setLoadingMatchingCards] = useState<boolean>(false);
  // const [error, setError] = useState<string | null>(null);
  
  useEffect(() => {
    if (quiz && currentQuestionNum) {
      console.log(quiz);
      const matchingObject = quiz.allQuestions[currentQuestionNum - 1];
      const { keys, values } = MatchingService.splitQuestion(matchingObject.question);

      const matchingCardObject = {
        matchingCardId: matchingObject.id,
        question: matchingObject.question,
        questionText: matchingObject.questionText,
        answer: matchingObject.answer,
        quizId: matchingObject.quizId,
        quizQuestionNum: matchingObject.quizQuestionNum,
        keys: keys,
        values: values,
      };

      setMatchingCard(matchingCardObject);
    }
  }, [quiz, currentQuestionNum]);

  const nextQuestion = () => {
    currentQuestionNum = currentQuestionNum+1;
    const route = QuizService.getQuizRoute(quiz, currentQuestionNum);
    console.log(route)
    navigate(route, { state: {quiz, currentQuestionNum} })
  }

  // const fetchMatchingCards = async () => {
  //   setLoadingMatchingCards(true);
  //   setError(null);

  //   try {
  //     const data = await MatchingService.fetchMatchings(2);
  //     setMatchingCards(data);
  //     console.log(data);
  //   } catch (error: unknown) {
  //     if (error instanceof Error) {
  //       console.error(`There was a problem fetching data: ${error.message}`);
  //     } else {
  //       console.error("Unknown error", error);
  //     }
  //     setError("Failed to fetch matching cards");
  //   } finally {
  //     setLoadingMatchingCards(false);
  //   }
  // };

  // useEffect(() => {
  //   console.log("Fetching matching cards");
  //   fetchMatchingCards();
  // }, []);

  // if (loadingMatchingCards) {
  //   return <div>Loading...</div>;
  // }

  // if (error) {
  //   return <div>Error: {error}</div>;
  // }

  return (
    <div>
      <br/><br/>
        {matchingCard && (
          <div className="matching-card-wrapper">
              <div key={matchingCard.matchingCardId}>
                {/* Spørsmålstekst */}
                <h3>{matchingCard.questionText}</h3>
                <hr />

                <div className="matching-table-wrapper">
                  <table className="matching-table">
                    <tbody>
                      {matchingCard.keys.map((key:string, i:number) => (
                        <tr className="matching-table-tr" key={i}>
                          <td className="matching-table-keys-td">{key}</td>
                          <td className="matching-table-values-td">{matchingCard.values[i]}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            <button className="matching-card-next-btn" onClick={nextQuestion}>Next question</button>
          </div>
        )}
    </div>
  );
}

export default MatchingCardQuizPage;
