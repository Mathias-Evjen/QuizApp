import { useState, useEffect } from "react";
import { Matching } from "../types/matching";
import "./Matching.css";
import * as MatchingService from "../quiz/services/MatchingService";
import * as QuizService from "../quiz/QuizService";
import { useNavigate, useLocation } from "react-router-dom";

//TODO: Finne ut en måte å oppdatere quiz objektet med svar

// interface MatchingProps{
//   quizQuestionNum: number,
//   questionText: string,
//   question: string,
//   userAnswer: string
// }

const MatchingCardQuizPage: React.FC = ({}) => {
  // const location = useLocation();
  // const navigate = useNavigate();
  // let {quiz, currentQuestionNum} = location.state || {};
  // const [matchingCards, setMatchingCards] = useState<MatchingCard[]>([]);
  // const [splitQuestion, setSplitQuestion] = useState<{ keys: string[]; values: string[] } | null>(null);
  // const [answer, setAnswer] = useState<string>(userAnswer);
  // const [loadingMatchingCards, setLoadingMatchingCards] = useState<boolean>(false);
  // const [error, setError] = useState<string | null>(null);
  
  // useEffect(() => {
  //   setSplitQuestion(MatchingService.splitQuestion(question));
  // }, [question]);

  // // const nextQuestion = () => {
  //   currentQuestionNum = currentQuestionNum+1;
  //   const route = QuizService.getQuizRoute(quiz, currentQuestionNum);
  //   console.log(route)
  //   navigate(route, { state: {quiz, currentQuestionNum} })
  // }

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
        {/* {question && (
          <div className="matching-card-wrapper">
              <div>
                {/* Spørsmålstekst */}
                {/* <h3>{questionText}</h3>
                <hr />

                <div className="matching-table-wrapper">
                  <table className="matching-table">
                    <tbody>
                      {splitQuestion && splitQuestion.keys.map((key:string, i:number) => (
                        <tr className="matching-table-tr" key={i}>
                          <td className="matching-table-keys-td">{key}</td>
                          <td className="matching-table-values-td">{splitQuestion.values[i]}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
          </div>
        )} */} */
    </div>
  );
}

export default MatchingCardQuizPage;
