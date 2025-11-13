import { useState, useEffect } from "react";
import { MatchingCard } from "../types/matchingCard";
import "./Matching.css";
import * as MatchingService from "./MatchingService";


function MatchingCardQuizPage() {
  const [matchingCards, setMatchingCards] = useState<MatchingCard[]>([]);
  const [loadingMatchingCards, setLoadingMatchingCards] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchMatchingCards = async () => {
    setLoadingMatchingCards(true);
    setError(null);

    try {
      const data = await MatchingService.fetchMatchings(2);
      setMatchingCards(data);
      console.log(data);
    } catch (error: unknown) {
      if (error instanceof Error) {
        console.error(`There was a problem fetching data: ${error.message}`);
      } else {
        console.error("Unknown error", error);
      }
      setError("Failed to fetch matching cards");
    } finally {
      setLoadingMatchingCards(false);
    }
  };

  useEffect(() => {
    console.log("Fetching matching cards");
    fetchMatchingCards();
  }, []);

  if (loadingMatchingCards) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div>
      <br/><br/>
      {matchingCards.length > 0 ? (
        <div className="matching-card-wrapper">
          {matchingCards.map((card) => (
            <div key={card.matchingCardId}>
              {/* Spørsmålstekst */}
              <h3>{card.questionText}</h3>
              <hr />

              <div className="matching-table-wrapper">
                <table className="matching-table">
                  <tbody>
                    {card.keys.map((key, i) => (
                      <tr className="matching-table-tr" key={i}>
                        <td className="matching-table-keys-td">{key}</td>
                        <td className="matching-table-values-td">{card.values[i]}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          ))}
          <button className="matching-card-next-btn">Next question</button>
        </div>
      ) : (
        <h3>No matching cards found.</h3>
      )}
    </div>
  );
}

export default MatchingCardQuizPage;
