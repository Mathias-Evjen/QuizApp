import { useEffect, useState } from "react";
import { FlashCardQuiz } from "../../types/flashCardQuiz";
import QuizCard from "./QuizCard";
import CreateForm from "./CreateForm";
import { Add, MoreVert, Settings, Delete, Close } from "@mui/icons-material";
import { useNavigate } from "react-router-dom";

const API_URL = "http://localhost:5041"

const Quizzes: React.FC = () => {
    const navigate = useNavigate();

    const [quizzes, setQuizzes] = useState<FlashCardQuiz[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    
    const [showCreate, setShowCreate] = useState<boolean>(false);
    const [showDelete, setShowDelete] = useState<boolean>(false);
    const [quizToDelete, setQuizToDelete] = useState<FlashCardQuiz | null>(null);

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

    const handleDelete = async (quizId: number) => {
        try {
            const response = await fetch(`${API_URL}/api/flashcardquizapi/delete/${quizId}`, {
                method: "DELETE"
            });
            setQuizzes(prevQuizzes => prevQuizzes.filter(quiz => quiz.flashCardQuizId !== quizId));
            console.log("Quiz deleted: ", quizId)
            handleShowDelete(null, false);
        } catch (error) {
            console.error("Error deleting flash card quiz: ", error)
            setError("Failed to delete quiz")
        }
    }

    const handleEdit = (quizId: number) => {
        navigate(`/manageFlashCardQuiz/${quizId}`)
    }

    useEffect(() => {
        console.log("Fetching data...")
        fetchQuizzes();
    }, []);

    const handleShowCreate = (value: boolean) => {
        setShowCreate(value);
    }

    const handleShowDelete = (quiz: FlashCardQuiz | null, show: boolean) => {
        setQuizToDelete(quiz);
        setShowDelete(show)
    }

    const handleShowMoreOptions = (quizId: number | null) => {
        setQuizzes(prevQuizzes =>
            prevQuizzes.map(quiz =>
                quiz.flashCardQuizId === quizId || quiz.showOptions === true
                ? { ...quiz, showOptions: !quiz.showOptions} 
                : quiz
            )
        )
    }

    

    return(
        <div className="quizzes-page" onClick={() => handleShowMoreOptions(null)}>
            <h1>Flash card quizzes</h1>
            <div className="flash-card-quiz-container">
                {quizzes.map(quiz => (
                    <div className="flash-card-quiz-entry" key={quiz.flashCardQuizId}>
                        <QuizCard
                            id={quiz.flashCardQuizId}
                            name={quiz.name}
                            description={quiz.description}
                            numOfQuestions={quiz.numOfQuestions}
                            showOptions={quiz.showOptions!}
                            />
                        <div className="flash-card-quiz-options" onClick={(e) => e.stopPropagation()}>
                            <div className="flash-card-quiz-edit" onClick={() => handleEdit(quiz.flashCardQuizId!)}><Settings /></div>
                            <div className="flash-card-quiz-delete" onClick={() => handleShowDelete(quiz, true)}><Delete /></div>
                        </div>
                        <button className={"flash-card-quiz-more-button"} onClick={(e) => {e.stopPropagation(); handleShowMoreOptions(quiz.flashCardQuizId!)}}>
                            {quiz.showOptions ? <Close /> : <MoreVert/>}
                        </button>
                    </div>
                ))}
            </div>
            <button className="create-flash-card-quiz-button" onClick={() => handleShowCreate(true)}><Add /></button>
            <div className={`${showCreate ? "create-flash-card-quiz-popup" : ""}`} onClick={() => handleShowCreate(false)}>
                {showCreate ? <CreateForm onQuizChanged={handleCreate} handleCancel={handleShowCreate}/> : ""}
            </div>
            {showDelete 
            ? <div className="confirm-delete" onClick={() => handleShowDelete(null, false)}>
                <div className="confirm-delete-content" onClick={(e) => e.stopPropagation()}>
                    <p>Do you want to delete the quiz</p>
                    <p>{quizToDelete?.name}</p>
                    <button className="popup-button" onClick={() => handleShowDelete(null, false)}>Cancel</button>
                    <button className="popup-button" onClick={() => handleDelete(quizToDelete?.flashCardQuizId!)} >Delete</button>
                </div>
            </div> 
            : ""}
        </div>
    )
}

export default Quizzes;