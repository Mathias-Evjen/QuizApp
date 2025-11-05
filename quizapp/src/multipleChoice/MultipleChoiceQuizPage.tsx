import { useState, useEffect } from "react";
import { MultipleChoice } from "../types/multipleChoice";
import "./MultipleChoice.css";

const API_URL = "http://localhost:5041"

function MultipleChoiceQuizPage() {
    const [multipleChoice, setMultipleChoice] = useState<MultipleChoice[]>([]);
    const [loadingMultipleChoice, setLoadingMultipleChoice] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const quizId = 2;     // Må kanskje endre verdi

    const fetchMultipleChoice = async () => {
        setLoadingMultipleChoice(true);
        setError(null);
        
        try {
            const response = await fetch(`${API_URL}/api/multiplechoice/getAllMultipleChoice/${quizId}`);
            if (!response.ok) {
                throw new Error("Failed to fetch multiple choice questions");
            }

            const data = await response.json();
            setMultipleChoice(data);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setError("Failed to fetch multiple choice");
        } finally {
            setLoadingMultipleChoice(false);
        }
    };

    useEffect(() => {
        console.log("Fetching multiple choice");
        fetchMultipleChoice();
    }, []); 
    
    if (loadingMultipleChoice) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div className="multiple-choice-container">
            <h1>Multiple Choice Quiz</h1>

            {multipleChoice.length > 0 ? (
                multipleChoice.map((card) => (
                    <div key={card.multipleChoiceId} className="multiple-choice-card">
                        <h3>{card.question}</h3>

                        <ul className="multiple-choice-options">
                            {card.options.map((opt, i) => (
                                <li key={i}>
                                    <label>
                                        <input type="radio" name={`card_${card.multipleChoiceId}`} />
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

            <button className="next-question-btn">Next Question</button>
        </div>
    );
}

export default MultipleChoiceQuizPage;