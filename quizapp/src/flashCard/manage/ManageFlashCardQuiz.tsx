import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { FlashCardQuiz } from "../../types/flashCardQuiz";
import { FlashCard } from "../../types/flashCard";
import * as FlashCardQuizService from "../FlashCardQuizService";
import * as FlashCardService from "../FlashCardService";
import FlashCardQuizForm from "../FlashCardQuizForm";
import FlashCardEntry from "./FlashCardEntry";
import SearchBar from "../../shared/SearchBar";
import "../style/FlashCard.css";

const ManageFlashCardQuiz: React.FC = () => {
    const navigate = useNavigate();

    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [quiz, setQuiz] = useState<FlashCardQuiz>();
    const [loadingQuiz, setLoadingQuiz] = useState<boolean>(false);
    
    const [flashCards, setFlashCards] = useState<FlashCard[]>([]);
    const [loadingFlashCards, setLoadingFlashCards] = useState<boolean>(false);
    
    const [error, setError] = useState<string | null>(null);
    
    const [query, setQuery] = useState<string>("");
    const filteredCards = flashCards.filter(card =>
        card.question.toLocaleLowerCase().includes(query.toLocaleLowerCase()) || card.answer.toLocaleLowerCase().includes(query.toLocaleLowerCase())
    );

    const [validationErrors, setValidationErrors] = useState<{[key: number]: { question?: string; answer?: string }}>({});

    const [showUpdateQuiz, setShowUpdateQuiz] = useState<boolean>(false);


    // ------------------------------
    //     CRUD Operations - Quiz
    // ------------------------------

    const fetchQuiz = async () => {
        setLoadingQuiz(true);
        setError(null);

        try {
            const data = await FlashCardQuizService.fetchQuizById(quizId);
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

    const handleUpdateQuiz = async (quiz: FlashCardQuiz) => {
        try {
            const data = await FlashCardQuizService.updateQuiz(quizId, quiz);
            console.log("Flash card quiz updated successfully:", data);
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error);
        }
    };


    // -------------------------------------
    //     CRUD Operations - Flash cards
    // -------------------------------------
    
    const fetchFlashCards = async () => {
        setLoadingFlashCards(true)
        setError(null)

        try {
            const data = await FlashCardService.fetchFlashCards(quizId);
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

    const handleCreateFlashCard = async (flashCard: FlashCard) => {
        const newCard: FlashCard = {question: flashCard.question, answer: flashCard.answer, quizId: flashCard.quizId, quizQuestionNum: flashCard.quizQuestionNum}
        try {
            const data = await FlashCardService.createFlashCard(newCard);
            console.log("Flash card created successfully:", data);
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error);
        }
    };

    const handleUpdateFlashCard = async (flashCard: FlashCard) => {
        try {
            const data = await FlashCardService.updateFlashCard(flashCard);
            console.log("Flash card updated successfully:", data);
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error)
        }
    };

    const handleDeleteFlashCard = async (flashCardId: number, qNum: number) => {
        try {
            const isTempCard = flashCards.some(card => card.tempId === flashCardId);

            if (!isTempCard) {
                await FlashCardService.deleteFlashCard(flashCardId, qNum, quizId);
            }
            setFlashCards(prevCards => prevCards.filter(card => card.flashCardId !== flashCardId && card.tempId !== flashCardId));
            console.log("Flash card deleted: ", flashCardId);
        } catch (error) {
            console.error("Error deleting flash card: ", error);
            setError("Failed to delete flash card");
        }
    };

    
    // ------------------------------
    //     Validations and checks
    // ------------------------------

    const handleQuestionChanged = (flashCardId: number, newQuestion: string) => {
        setFlashCards(prevCards =>
            prevCards.map(card =>
                card.flashCardId === flashCardId || card.tempId === flashCardId
                ? {...card, question: newQuestion, isDirty: true}
                : card
            )
        );
        
    };

    const handleAnswerChanged = (flashCardId: number, newAnswer: string) => {
        setFlashCards(prevCards =>
            prevCards.map(card =>
                card.flashCardId === flashCardId || card.tempId === flashCardId
                ? {...card, answer: newAnswer, isDirty: true}
                : card
            )
        );
        
    };

    const validateFlashCard = (card: FlashCard) => {
        const errors: { question?: string; answer?: string } = {};
        if (!card.question || card.question.trim() === "") errors.question = "Question is required";
        if (!card.answer || card.answer.trim() === "") errors.answer = "Answer is required";
        return errors;
    };

    const flashCardsToSave = () => {
        if (flashCards.some(card => card.isDirty || card.isNew))
            return true;
        return false;
    };
    

    // ----------------------
    //     Frontend logic
    // ----------------------

    const handleAddFlashCard = () => {
        const newCard: FlashCard = {question: "", answer: "", quizId: quizId, quizQuestionNum: flashCards.length + 1, isNew: true, tempId: Date.now() + Math.random()}
        setFlashCards(prevCards =>
            [...prevCards, newCard]
        )
    };

    const handleSaveFlashCard = async () => {
        const allErrors: typeof validationErrors = {};
        flashCards.forEach(card => {
            const errs = validateFlashCard(card);
            if (Object.keys(errs).length > 0) allErrors[card.flashCardId ?? card.tempId!] = errs;
        });

        if (Object.keys(allErrors).length > 0) {
            setValidationErrors(allErrors);
            return;
        }

        const dirtyCards = flashCards.filter(card => card.isDirty)
        const newCards = flashCards.filter(card => card.isNew)
        
        await Promise.all([
            ...dirtyCards.map(card => handleUpdateFlashCard(card)),
            ...newCards.map(card => handleCreateFlashCard(card))
        ]);

        setFlashCards(prevCards =>
            prevCards.map(card =>
                ({...card, isNew: false, isDirty: false})
            )
        )
        
        setValidationErrors({});
    };
    
    const saveQuiz = (newName: string, newDescription: string) => {
        const updatedQuiz = { ...quiz!, name: newName, description: newDescription === "" ? undefined : newDescription}
        setQuiz(updatedQuiz)
        handleUpdateQuiz(updatedQuiz);
        handleShowUpdateQuiz(false);
    };

    const handleShowUpdateQuiz = (value: boolean) => {
        setShowUpdateQuiz(value);
    };
    
    useEffect(() => {
            fetchQuiz();
            fetchFlashCards();
        }, []);

    return(
        <>
            {loadingQuiz || loadingFlashCards ? (
                <p className="loading">Loading...</p>
            ) : error ? (
                <p className="fetch-error">{error}</p>
            ) : (
                <div className="manage-quiz-container">
                    <div className="flash-card-quiz-details-container">
                        <div className="flash-card-quiz-details">
                            <h1>{quiz?.name}</h1>
                            <h2>{quiz?.description}</h2>
                        </div>
                        <button className="button" onClick={() => handleShowUpdateQuiz(true)}>Edit</button> 
                    </div>
                    <div className="page-top-container">
                        <SearchBar query={query} placeholder="Search for a flash card" handleSearch={setQuery}/>
                        <div className="manage-buttons-div">
                            <button className="button add-button" onClick={handleAddFlashCard}>Add</button>
                            <button className={`button primary-button ${flashCardsToSave() ? "active" : ""}`} onClick={handleSaveFlashCard}>Save</button>
                        </div>
                    </div>
                    
                    <div className="flash-card-entry-container">
                        {flashCards.length === 0 ? (
                            <p className="flash-card-entry-container-emtpy">Add the first flash card!</p>
                        ) : filteredCards.length === 0 ? (
                            <p className="flash-card-entry-container-emtpy">No cards matching search</p>
                        ) : (
                            filteredCards.map(card =>
                                <FlashCardEntry
                                    key={card.flashCardId ?? card.tempId}
                                    flashCardId={card.flashCardId! ?? card.tempId}
                                    quizQuestionNum={card.quizQuestionNum}
                                    question={card.question}
                                    answer={card.answer}
                                    onQuestionChanged={handleQuestionChanged}
                                    onAnswerChanged={handleAnswerChanged}
                                    onDeletePressed={handleDeleteFlashCard}
                                    errors={validationErrors[card.flashCardId ?? card.tempId!]}/>
                            )
                        )}
                    </div>

                    <div className={`${showUpdateQuiz ? "flash-card-quiz-popup" : ""}`}>
                        {showUpdateQuiz ?
                            (
                                <FlashCardQuizForm
                                    onSubmit={saveQuiz}
                                    onCancel={handleShowUpdateQuiz}
                                    name={quiz!.name}
                                    description={quiz!.description} 
                                    isUpdate={true} />
                            ) : (
                                ""
                            )}
                    </div>
                </div>
            )}
            <div className="play-quiz-container">
                <button className="button" onClick={() => navigate(`/flashCards/${id}`)}>Play quiz</button>
            </div>
        </>
        
    )
}

export default ManageFlashCardQuiz;