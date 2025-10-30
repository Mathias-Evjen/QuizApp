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
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error)
        }
    }

    const handleQuestionChanged = (flashCardId: number, newQuestion: string) => {
        setFlashCards(prevCards =>
            prevCards.map(card =>
                card.flashCardId === flashCardId || card.tempId === flashCardId
                ? {...card, question: newQuestion, isDirty: true}
                : card
            )
        );
        
    }

    const handleAnswerChanged = (flashCardId: number, newAnswer: string) => {
        setFlashCards(prevCards =>
            prevCards.map(card =>
                card.flashCardId === flashCardId || card.tempId === flashCardId
                ? {...card, answer: newAnswer, isDirty: true}
                : card
            )
        );
        
    }

    const handleCreate = async (flashCard: FlashCard) => {
        const newCard: FlashCard = {question: flashCard.question, answer: flashCard.answer, quizId: flashCard.quizId, quizQuestionNum: flashCard.quizQuestionNum}
        try {
            const response = await fetch(`${API_URL}/api/flashcardapi/create`, {
                method: "post",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(newCard)
            });

            if (!response.ok) {
                throw new Error("Network response was not ok")
            }

            const data = await response.json();
            console.log("Flash card created successfully:", data);
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error)
        }
    }

    const handleAddFlashCard = () => {
        const newCard: FlashCard = {question: "", answer: "", quizId: quizId, quizQuestionNum: flashCards.length + 1, isNew: true, tempId: Date.now() + Math.random()}
        setFlashCards(prevCards =>
            [...prevCards, newCard]
        )
    }

    const handleSaveFlashCard = async () => {
        const dirtyCards = flashCards.filter(card => card.isDirty)
        const newCards = flashCards.filter(card => card.isNew)
        
        await Promise.all([
            ...dirtyCards.map(card => handleUpdateFlashCard(card)),
            ...newCards.map(card => handleCreate(card))
        ]);

        await fetchFlashCards();
    }

    useEffect(() => {
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
                        key={card.flashCardId ?? card.tempId}
                        flashCardId={card.flashCardId! ?? card.tempId}
                        quizQuestionNum={card.quizQuestionNum}
                        question={card.question}
                        answer={card.answer}
                        quizId={quiz?.flashCardQuizId!}
                        onQuestionChanged={handleQuestionChanged}
                        onAnswerChanged={handleAnswerChanged}/>
                )}
            </div>

            <div>
                <button className="popup-button" onClick={handleAddFlashCard}>Add</button>
            </div>
            <div>
                <button className="popup-button" onClick={handleSaveFlashCard}>Save</button>
            </div>
        </>
    )
}

export default ManageFlashCardQuiz;