import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { FlashCardQuiz } from "../../types/flashCardQuiz";
import { FlashCard } from "../../types/flashCard";
import FlashCardEntry from "./FlashCardEntry";
import QuizUpdateForm from "./QuizUpdateForm";

const API_URL = "http://localhost:5041"

const ManageFlashCardQuiz: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [quiz, setQuiz] = useState<FlashCardQuiz>();
    const [flashCards, setFlashCards] = useState<FlashCard[]>([]);
    
    const [loadingQuiz, setLoadingQuiz] = useState<boolean>(false);
    const [loadingFlashCards, setLoadingFlashCards] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [flashCardErrors, setFlashCardErrors] = useState<{[key: number]: { question?: string; answer?: string }}>({});

    const [showUpdateQuiz, setShowUpdateQuiz] = useState<boolean>(false);

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

    const handleUpdateQuiz = async (quiz: FlashCardQuiz) => {
        try {
            const response = await fetch(`${API_URL}/api/flashcardquizapi/update/${quizId}`, {
                method: "put",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(quiz)
            });

            if (!response.ok) {
                throw new Error("Network response was not ok");
            }

            const data = await response.json();
            console.log("Flash card quiz updated successfully:", data);
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error);
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

    const validateFlashCard = (card: FlashCard) => {
        const errors: { question?: string; answer?: string } = {};
        if (!card.question || card.question.trim() === "") errors.question = "Question is required";
        if (!card.answer || card.answer.trim() === "") errors.answer = "Answer is required";
        return errors;
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
        const allErrors: typeof flashCardErrors = {};
        flashCards.forEach(card => {
            const errs = validateFlashCard(card);
            if (Object.keys(errs).length > 0) allErrors[card.flashCardId ?? card.tempId!] = errs;
        });

        if (Object.keys(allErrors).length > 0) {
            setFlashCardErrors(allErrors);
            return;
        }

        const dirtyCards = flashCards.filter(card => card.isDirty)
        const newCards = flashCards.filter(card => card.isNew)
        
        await Promise.all([
            ...dirtyCards.map(card => handleUpdateFlashCard(card)),
            ...newCards.map(card => handleCreate(card))
        ]);
        
        setFlashCardErrors({})
    }

    const handleDelete = async (flashCardId: number) => {
        try {
            const isTempCard = flashCards.some(card => card.tempId === flashCardId);

            if (!isTempCard) {
                const response = await fetch(`${API_URL}/api/flashcardapi/delete/${flashCardId}`, {
                    method: "DELETE"
                });
            }
            setFlashCards(prevCards => prevCards.filter(card => card.flashCardId !== flashCardId && card.tempId !== flashCardId));
            console.log("Flash card deleted: ", flashCardId);
        } catch (error) {
            console.error("Error deleting flash card: ", error);
            setError("Failed to delete flash card");
        }
    }

    const flashCardsToSave = () => {
        if (flashCards.some(card => card.isDirty || card.isNew))
            return true;
        return false;
    }

    const handleShowUpdateQuiz = (value: boolean) => {
        setShowUpdateQuiz(value);
    }

    const saveQuiz = (newName: string, newDescription: string) => {
        const updatedQuiz = { ...quiz!, name: newName, description: newDescription}
        setQuiz(updatedQuiz)
        handleUpdateQuiz(updatedQuiz);
        handleShowUpdateQuiz(false);
    }

    useEffect(() => {
            fetchQuiz();
            fetchFlashCards();
        }, []);

    return(
        <div className="manage-quiz-container">
            <div className="flash-card-quiz-details-container">
                <div className="flash-card-quiz-details">
                    <h1>{quiz?.name}</h1>
                    <h2>{quiz?.description}</h2>
                </div>
                <button className="button" onClick={() => handleShowUpdateQuiz(true)}>Edit</button> 
            </div>
            
            <div className="flash-card-entry-container">
                {flashCards.map(card =>
                    <FlashCardEntry
                        key={card.flashCardId ?? card.tempId}
                        flashCardId={card.flashCardId! ?? card.tempId}
                        quizQuestionNum={card.quizQuestionNum}
                        question={card.question}
                        answer={card.answer}
                        onQuestionChanged={handleQuestionChanged}
                        onAnswerChanged={handleAnswerChanged}
                        onDeletePressed={handleDelete}
                        errors={flashCardErrors[card.flashCardId ?? card.tempId!]}/>
                )}
            </div>

            <div className="manage-buttons-div">
                <button className="button add-button" onClick={handleAddFlashCard}>Add</button>
                <button className={`button save-button ${flashCardsToSave() ? "active" : ""}`} onClick={handleSaveFlashCard}>Save</button>
            </div>

            <div className={`${showUpdateQuiz ? "flash-card-quiz-popup" : ""}`}>
                {showUpdateQuiz ?
                    (
                        <QuizUpdateForm 
                            name={quiz!.name} 
                            description={quiz!.description}
                            onCancelClick={handleShowUpdateQuiz}
                            onSave={saveQuiz}/>
                    ) : (
                        ""
                    )}
            </div>
        </div>
    )
}

export default ManageFlashCardQuiz;