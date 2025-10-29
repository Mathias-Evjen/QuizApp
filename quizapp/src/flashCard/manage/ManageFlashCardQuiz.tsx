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

    const handleQuestionChanged = (flashCardId: number, newQuestion: string) => {
        setFlashCards(prevCards =>
            prevCards.map(card =>
                card.flashCardId === flashCardId
                ? {...card, question: newQuestion, isDirty: true}
                : card
            )
        );
        
    }

    const handleAnswerChanged = (flashCardId: number, newAnswer: string) => {
        setFlashCards(prevCards =>
            prevCards.map(card =>
                card.flashCardId === flashCardId
                ? {...card, answer: newAnswer, isDirty: true}
                : card
            )
        );
        
    }

    const handleSaveFlashCard = async () => {
        const dirtyCards = flashCards.filter(card => card.isDirty)
        dirtyCards.forEach(handleUpdateFlashCard)
    }

    useEffect(() => {
            console.log("QuizID: ", quizId)
            fetchQuiz();
            fetchFlashCards();
        }, []);

    useEffect(() => {
        if (flashCards.length > 0) {
            setFlashCards(prevCards => 
                prevCards.map(card => 
                ({...card, isDirty: false})
                ))
        }
    }, [flashCards.length])

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
                        question={card.question}
                        answer={card.answer}
                        quizId={quiz?.flashCardQuizId!}
                        onQuestionChanged={handleQuestionChanged}
                        onAnswerChanged={handleAnswerChanged}/>
                )}
            </div>

            <div>
                <button className="popup-button" onClick={handleSaveFlashCard}>Save</button>
            </div>
        </>
    )
}

export default ManageFlashCardQuiz;