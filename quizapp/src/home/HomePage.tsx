import { useEffect, useState } from "react";
import { Quiz } from "../types/quiz/quiz";
import { FlashCardQuiz } from "../types/flashcard/flashCardQuiz";
import { useNavigate } from "react-router-dom";
import * as QuizService from "../quiz/services/QuizService";
import * as FlashCardQuizService from "../flashCard/FlashCardQuizService";
import "./style/HomePage.css";
import FlashCardQuizCard from "../flashCard/quizzes/FlashCardQuizCard";
import QuizCard from "../quiz/QuizCard";


const HomePage: React.FC = () => {
    const navigate = useNavigate();

    const [quizzes, setQuizzes] = useState<Quiz[]>([]);
    const [loadingQuizzes, setLoadingQuizzes] = useState<boolean>(false);
    const [quizError, setQuizError] = useState<string | null>(null);

    const [flashCardQuizzes, setFlashCardQuizzes] = useState<FlashCardQuiz[]>([]);
    const [loadingFlashCardQuizzes, setLoadingFlashCardQuizzes] = useState<boolean>(false);
    const [flashCardError, setFlashCardError] = useState<string | null>(null);

    const [displayQuizzes, setDisplayQuizzes] = useState<Quiz[]>([]);
    const [displayFlashCards, setDisplayFlashCards] = useState<FlashCardQuiz[]>([]);

    const fetchQuizzes = async () => {
        setLoadingQuizzes(true);
        setQuizError(null);
    
        try {
            const data = await QuizService.fetchQuizzes();
            setQuizzes(data);
            console.log(data);
            setQuizzesToDisplay(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setQuizError("Failed to fetch quizzes");
        } finally {
            
        }
    };

    const fetchFlashCardQuizzes = async () => {
        setLoadingFlashCardQuizzes(true);
        setFlashCardError(null);

        try {
            const data = await FlashCardQuizService.fetchQuizzes();
            setFlashCardQuizzes(data);
            console.log(data);
            setFlashCardsToDisplay(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setFlashCardError("Failed to fetch quizzes.");
        } finally {
            setLoadingFlashCardQuizzes(false);
        }
    };

    const setQuizzesToDisplay = (quizzes: Quiz[]) => {
        setDisplayQuizzes([]);

        var randomIndex = pickRandom(quizzes.length);
        setDisplayQuizzes(prev => [...prev, quizzes[randomIndex]]);

        var newRandomIndex = pickRandom(quizzes.length);
        while (newRandomIndex === randomIndex) {
            newRandomIndex = pickRandom(quizzes.length);
        }
        setDisplayQuizzes(prev => [...prev, quizzes[newRandomIndex]]);
    };

    const setFlashCardsToDisplay = (flashCards: FlashCardQuiz[]) => {
        setDisplayFlashCards([]);

        var randomIndex = pickRandom(flashCards.length);
        setDisplayFlashCards(prev => [...prev, flashCards[randomIndex]]);
        
        var newRandomIndex = pickRandom(flashCards.length);
        while (newRandomIndex === randomIndex) {
            newRandomIndex = pickRandom(flashCards.length);
        }
        setDisplayFlashCards(prev => [...prev, flashCards[newRandomIndex]]);
    };

    const pickRandom = (max: number) => {
        return Math.floor(Math.random() * max);
    };

    useEffect(() => {
        fetchQuizzes();
        fetchFlashCardQuizzes();
    }, []);

    return(
        <div className="home-page-wrapper">
            <div className="home-page-container">
                <h1>QuizApp</h1>
                
                <div className="home-page-quizzes-wrapper">
                    <div className="home-page-quizzes-container">
                        <h2>Try a quiz</h2>

                        {displayQuizzes.length === 0 ? (
                            <h3>Loading...</h3>
                        ) : (
                            displayQuizzes.map(quiz =>
                                <QuizCard
                                    key={quiz.quizId} 
                                    quizId={quiz.quizId!}
                                    name={quiz.name}
                                    description={quiz.description!}
                                    numOfQuestions={quiz.numOfQuestions!} />
                            )
                        )}
                    </div>
                    
                    <div className="home-page-quizzes-container">
                        <h2>Try a flash card quiz</h2>
                        
                        {displayFlashCards.length === 0 ? (
                            <h3>Loading...</h3>
                        ) : (
                            displayFlashCards.map(quiz => 
                                <FlashCardQuizCard
                                    key={quiz.flashCardQuizId}
                                    id={quiz.flashCardQuizId}
                                    name={quiz.name}
                                    description={quiz.description}
                                    numOfQuestions={quiz.numOfQuestions}
                                    />
                            )
                        )}
                    </div>
                </div>
            </div>
        </div>
    )
}

export default HomePage;