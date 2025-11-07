import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { MultipleChoice } from "../types/multipleChoice";
import "./MultipleChoice.css";
import { fetchMultipleChoiceQuestions } from "../services/MultipleChoiceService";

function MultipleChoiceQuizPage() {
  const { id } = useParams<{ id: string }>();
  const quizId = Number(id);

  const [multipleChoice, setMultipleChoice] = useState<MultipleChoice[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const loadQuestions = async () => {
    setLoading(true);
    setError(null);

    try {
      const data = await fetchMultipleChoiceQuestions(3); // TODO Husk å endre til quizId når det skal brukes ordentlig
      setMultipleChoice(data);
    } catch (err) {
      setError("Failed to fetch multiple choice questions");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    console.log("Fetching multiple choice questions...");
    loadQuestions();
  }, [quizId]);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="multiple-choice-container">
      <h1>Multiple Choice Quiz</h1>

      {multipleChoice.length > 0 ? (
        multipleChoice.map((question) => (
          <div key={question.multipleChoiceId} className="multiple-choice-card">
            <h3>{question.question}</h3>

            <ul className="multiple-choice-options">
              {question.options.map((opt, index) => (
                <li key={index}>
                  <label>
                    <input type="radio" name={`q_${question.multipleChoiceId}`} />
                    {opt.text}
                  </label>
                </li>
              ))}
            </ul>
          </div>
        ))
      ) : (
        <h3>No questions found.</h3>
      )}
    </div>
  );
}

export default MultipleChoiceQuizPage;
