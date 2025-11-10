import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { FlashCard } from "../../types/flashCard";
import { FlashCardQuiz } from "../../types/flashCardQuiz";
import { KeyboardArrowLeft, KeyboardArrowRight, Shuffle, ShuffleOn } from "@mui/icons-material";
import FlashCardComponent from "./FlashCardComponent";
import InfoCard from "./InfoCard";
import { fetchQuizById } from "../FlashCardQuizService";
import * as FlashCardService from "../FlashCardService";

const FlashCardQuizPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [quiz, setQuiz] = useState<FlashCardQuiz>();
    const [loadingQuiz, setLoadingQuiz] = useState<boolean>(false);

    const [flashCards, setFlashCards] = useState<FlashCard[]>([]);
    const [loadingFlashCards, setLoadingFlashCards] = useState<boolean>(false);

    const [error, setError] = useState<string | null>(null);

    const [shuffle, setShuffle] = useState<boolean>(false);
    const [flashCardIndex, setFlashCardIndex] = useState<number>(0);


    // -----------------------
    //     Fetch operations 
    // -----------------------

    const fetchQuiz = async () => {
        setLoadingQuiz(true);
        setError(null);

        try {
            const data = await fetchQuizById(quizId);
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


    // ------------------------
    //     Flash card logic
    // ------------------------

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
        if (flashCardIndex > 0) {
            if (flashCards[flashCardIndex].showAnswer)
                toggleShowAnswer(flashCards[flashCardIndex].flashCardId!);
            setFlashCardIndex(flashCardIndex - 1)
        }
    }

    const handleNextCard = () => {
        if (flashCardIndex + 1 != flashCards.length) {
            if (flashCards[flashCardIndex].showAnswer)
                toggleShowAnswer(flashCards[flashCardIndex].flashCardId!);
            setFlashCardIndex(flashCardIndex + 1)
        }
    }

    const handleShuffle = () => {
        if (flashCards[flashCardIndex].showAnswer)
                toggleShowAnswer(flashCards[flashCardIndex].flashCardId!);

        if (!shuffle) {
            setShuffle(true);
            setFlashCards(prevCards => {
                const randomized = [...prevCards];
                for (let i = randomized.length - 1; i > 0; i--) {
                    const j = Math.floor(Math.random() * (i + 1));
                    [randomized[i], randomized[j]] = [randomized[j], randomized[i]];
                }
                return randomized;
            })
        } else {
            setShuffle(false);
            setFlashCards(prevCards =>
                [...prevCards].sort((a, b) => a.quizQuestionNum - b.quizQuestionNum)
            );
        };

        setFlashCardIndex(0);
    }


    useEffect(() => {
        console.log("Fetching data...")
        fetchQuiz();
        fetchFlashCards();
    }, []);

    useEffect(() => {
        const handleKeyDown = (e: KeyboardEvent) => {
            if (e.key === "ArrowRight") handleNextCard();
            if (e.key === "ArrowLeft") handlePrevCard();
            if (e.key === " ") toggleShowAnswer(flashCards[flashCardIndex].flashCardId!);
        };

        window.addEventListener("keydown", handleKeyDown);
        return () => window.removeEventListener("keydown", handleKeyDown);
    }, [handleNextCard, handlePrevCard, toggleShowAnswer]);

    return(
        <>
            {loadingFlashCards || loadingQuiz ? (
                <p className="loading">Loading...</p>
            ) : error ? (
                <p className="fetch-error">{error}</p>
            ) : (
                <>
                <div className="flash-card-container">
                    {flashCards.length > 0 ? (
                        <>
                            <h1>{quiz?.name}</h1>
                            <FlashCardComponent 
                                key={flashCards[flashCardIndex].flashCardId} 
                                question={flashCards[flashCardIndex].question}
                                answer={flashCards[flashCardIndex].answer}
                                showAnswer={flashCards[flashCardIndex].showAnswer!}
                                color={flashCards[flashCardIndex].color!}
                                toggleAnswer={() => toggleShowAnswer(flashCards[flashCardIndex].flashCardId!)} />

                            {quiz?.description ? (
                                <div className="flash-card-sidebar">
                                <div className="flash-card-page-description">
                                    <h2>Description:</h2>
                                    <p>{quiz?.description}</p>
                                </div>
                                
                            </div>
                            ) : ("")}
                            

                            <div className="flash-card-menu">
                                <button className="button nav-button" onClick={handlePrevCard}><KeyboardArrowLeft /></button>
                                <div className="flash-card-menu-middle">
                                    <div className={`shuffle-button ${shuffle ? "active" : ""}`} onClick={handleShuffle}>
                                        {shuffle ? <ShuffleOn /> : <Shuffle />}
                                    </div>
                                    <p>{flashCardIndex + 1}/{flashCards.length}</p>
                                </div>
                                <button className="button nav-button" onClick={handleNextCard}><KeyboardArrowRight /></button>
                            </div>
                        </>
                    ) : (
                        <p>No flashcards to display</p>
                    )}
                </div>
                <InfoCard />
                </>
            )}
        </>
    )
}

export default FlashCardQuizPage;