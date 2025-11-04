import { useState, useEffect } from "react";
import { MatchingCard } from "../types/matchingCard";
import "./Matching.css";

const API_URL = "http://localhost:5041";

function MatchingCardQuizPage() {
  const [matchingCards, setMatchingCards] = useState<MatchingCard[]>([]);
  const [loadingMatchingCards, setLoadingMatchingCards] =
    useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchMatchingCards = async () => {
    setLoadingMatchingCards(true);
    setError(null);

    try {
      const response = await fetch(
        `${API_URL}/api/matchingapi/getQuestions/${2}`,
      );
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      const data = await response.json();
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
      <h1>Matching Question</h1>
      {matchingCards.length > 0 ? (
        <div className="matching-card-wrapper">
          {matchingCards.map((card) => (
            <div key={card.matchingCardId}>
              {/* Spørsmålstekst */}
              <h3>{card.questionText}</h3>
              <hr />

              <div className="matching-table-wrapper">
                <table className="matching-table">
                  <td className="matching-table-keys">
                    {card.keys.map((key, i) => (
                      <tr className="matching-table-keys-tr" key={i}>
                        {key}
                      </tr>
                    ))}
                  </td>
                  <td className="matching-table-values">
                    {card.values.map((value, i) => (
                      <tr className="matching-table-values-tr" key={i}>
                        {value}
                      </tr>
                    ))}
                  </td>
                </table>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <h3>No matching cards found.</h3>
      )}
      <button>Next question</button>
    </div>
  );
}

export default MatchingCardQuizPage;

