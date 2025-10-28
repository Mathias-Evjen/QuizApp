import { use, useEffect, useState } from "react";
import { FlashCardQuiz } from "../types/flashCardQuiz";
import QuizCard from "./QuizCard";
import CreateQuizComponent from "./QuizForm";
import QuizForm from "./QuizForm";

const API_URL = "http://localhost:5041"

const Quizzes: React.FC = () => {
    const [quizzes, setQuizzes] = useState<FlashCardQuiz[]>([]);
    const [showCreate, setShowCreate] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const fetchQuizzes = async () => {
        setLoading(true);
        setError(null);

        try {
            const response = await fetch(`${API_URL}/api/flashcardquizapi/getQuizzes`);
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            const data = await response.json();
            setQuizzes(data);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setError("Failed to fetch quizzes.");
        } finally {
            setLoading(false);
        }
    };

    const handleCreate = async (quiz: FlashCardQuiz) => {
        try {
            const response = await fetch(`${API_URL}/api/flashcardquizapi/create`, {
                method: "post",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(quiz),
            });

            if (!response.ok) {
                throw new Error("Network response was not ok");
            }

            const data = await response.json();
            console.log("Flash card quiz created successfully:", data);
            handleShowCreate(false)
            fetchQuizzes();
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error)
        }
    }

    useEffect(() => {
        fetchQuizzes();
    }, []);

    const handleShowCreate = (value: boolean) => {
        setShowCreate(value);
    }

    return(
        <>
            <h1>Flash card quizzes</h1>
            <div className="flash-card-quiz-container">
                {quizzes.map(quiz => (
                    <QuizCard
                        key={quiz.flashCardQuizId}
                        id={quiz.flashCardQuizId}
                        name={quiz.name}
                        description={quiz.description}
                        numOfQuestions={quiz.numOfQuestions}
                        />
                ))}
            </div>
            <button className="create-flash-card-quiz-button" onClick={() => handleShowCreate(true)}>Create new quiz</button>
            <div>
                {showCreate ? <QuizForm onQuizChanged={handleCreate}/> : ""}
            </div>
        </>
    )
}

export default Quizzes;