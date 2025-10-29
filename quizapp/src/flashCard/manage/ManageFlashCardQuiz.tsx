import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { FlashCardQuiz } from "../../types/flashCardQuiz";
import { FlashCard } from "../../types/flashCard";
import FlashCardEntry from "./FlashCardEntry";

const API_URL = "http://localhost:5041"

const ManageFlashCardQuiz: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [quiz, setQuiz] = useState<FlashCardQuiz>();
    const [flashCards, setFlashCards] = useState<FlashCard[]>([]);
    const [flashCardIndex, setFlashCardIndex] = useState<number>(0);
    const [currentCard, setCurrentCard] = useState<FlashCard>(flashCards[flashCardIndex]);
    const [loadingQuiz, setLoadingQuiz] = useState<boolean>(false);
    const [loadingFlashCards, setLoadingFlashCards] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const fetchQuiz = async () => {
        setLoadingQuiz(true);
        setError(null);

        try {
            const response = await fetch(`${API_URL}/api/flashcardquizapi/getQuiz/${quizId}`);
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const data = await response.json();
            setQuiz(data);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setError("Failed to fetch quizzes.");
        } finally {
            setLoadingQuiz(false);
        }
    };
    
    const fetchFlashCards = async () => {
        setLoadingFlashCards(true)
        setError(null)

        try {
            const response = await fetch(`${API_URL}/api/flashcardapi/getFlashCards/${quizId}`)
            if (!response.ok) {
                throw new Error("Nework response was not ok");
            }
            const data = await response.json();
            setFlashCards(data);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setError("Failed to fetch flashcards");
        } finally {
            setLoadingFlashCards(false);
        }
    };

    const handleUpdateFlashCard = async (flashCard: FlashCard) => {
        try {
            const response = await fetch(`${API_URL}/api/flashcardapi/update/${flashCard.flashCardId}`, {
                method: "put",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(flashCard)
            });

            if (!response.ok) {
                throw new Error("Network response was not ok");
            }

            const data = await response.json();
            console.log("Flash card updated successfully:", data);
            fetchFlashCards();
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error)
        }
    }

    useEffect(() => {
            console.log("QuizID: ", quizId)
            fetchQuiz();
            fetchFlashCards();
        }, []);

    return(
        <>
            <div>
                <h1>{quiz?.name}</h1>
                <h2>{quiz?.description}</h2>
            </div>
            <div className="flash-card-entry-container">
                {flashCards.map(card =>
                    <FlashCardEntry
                        flashCardId={card.flashCardId}
                        quizQuestionNum={card.quizQuestionNum}
                        existingQuestion={card.question}
                        existingAnswer={card.answer}
                        quizId={quiz?.flashCardQuizId!} 
                        onFlashCardChanged={handleUpdateFlashCard}/>
                )}
            </div>
        </>
    )
}

export default ManageFlashCardQuiz;