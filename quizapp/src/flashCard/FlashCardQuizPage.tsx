import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { FlashCard } from "../types/flashCard";
import { FlashCardQuiz } from "../types/flashCardQuiz";
import FlashCardComponent from "./FlashCardComponent";

const API_URL = "http://localhost:5041"

const FlashCardQuizPage: React.FC = () => {
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

    useEffect(() => {
        console.log("QuizID: ", quizId)
        fetchQuiz();
        fetchFlashCards();
    }, []);


    const toggleShowAnswer = (flashCardId: number) => {
        setFlashCards(prevCards => 
            prevCards.map(card =>
                card.flashCardId === flashCardId
                ? { ...card, showAnswer: !card.showAnswer}
                : card
            )
        )
    }

    const handlePrevCard = () => {
        if (flashCardIndex > 0)
            setFlashCardIndex(flashCardIndex - 1)
    }

    const handleNextCard = () => {
        if (flashCardIndex + 1 != flashCards.length)
        setFlashCardIndex(flashCardIndex + 1)
    }

    return(
        <>
            <div className="flash-card-container">
                <h1>{quiz?.name}</h1>
                {loadingFlashCards ? (
                    <p>Loading flashcards...</p>
                ) : flashCards.length > 0 ? (
                    <div>
                        <FlashCardComponent 
                        key={flashCards[flashCardIndex].flashCardId} 
                        question={flashCards[flashCardIndex].question}
                        answer={flashCards[flashCardIndex].answer}
                        showAnswer={flashCards[flashCardIndex].showAnswer}
                        toggleAnswer={() => toggleShowAnswer(flashCards[flashCardIndex].flashCardId)}
                        />

                        <div>
                            <button onClick={handlePrevCard}>Prev</button>
                            <div>
                                <p>{flashCards[flashCardIndex].quizQuestionNum}/{flashCards.length}</p>
                            </div>
                            <button onClick={handleNextCard}>Next</button>
                        </div>
                    </div>
                ) : (
                    <p>No flashcards to display</p>
                )}
                
            </div>
        </>
    )
}

export default FlashCardQuizPage;