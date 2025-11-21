import { useState, useEffect } from "react";
import { Quiz} from "../types/quiz";
import { useNavigate } from 'react-router-dom';
import { useAuth } from "../auth/AuthContext";
import { Close, Delete, MoreVert, Settings } from "@mui/icons-material";
import * as QuizService from "./services/QuizService";
import SearchBar from "../shared/SearchBar";
import QuizCard from "./QuizCard";
import "./style/Quiz.css";


function QuizzesPage() {
    const navigate = useNavigate();

    const [quizzes, setQuizzes] = useState<Quiz[]>([]);
    const [loadingQuizzes, setLoadingQuizzes] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [query, setQuery] = useState<string>("");
    const filteredQuizzes = quizzes.filter(quiz => quiz.name.toLocaleLowerCase().includes(query.toLocaleLowerCase()) || quiz.description.toLocaleLowerCase().includes(query.toLocaleUpperCase()));

    const [showDelete, setShowDelete] = useState<boolean>(false);
    const [quizToDelete, setQuizToDelete] = useState<Quiz | null>(null);

    const { user } = useAuth();

    const fetchQuizzes = async () => {
        setLoadingQuizzes(true);
        setError(null);

        try {
            const data = await QuizService.fetchQuizzes();
            setQuizzes(data);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setError("Failed to fetch quizzes");
        } finally {
            setLoadingQuizzes(false);
        }
    };

    const handleDelete = async (quizId: number) => {
        try {
            await QuizService.deleteQuiz(quizId);
            setQuizzes(prevQuizzes => prevQuizzes.filter(quiz => quiz.quizId !== quizId))
            console.log("Quiz deleted: ", quizId);
            handleShowDelete(null, false);
        } catch (error) {
            console.error("Error deleting quiz: ", error);
            setError("Failed to delete quiz");
        }
    }

    useEffect(() => {
        console.log("Fetching quizzes");
        fetchQuizzes();
    }, []);

    if (loadingQuizzes) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    const openQuiz = (quizId: number) => {
        navigate(`/quiz/${quizId}`);
    };

    const manageQuiz = (quizId: number) => {
        navigate(`/quiz/manage/${quizId}`);
    };

    const handleShowDelete = (quiz: Quiz | null, show: boolean) => {
        setQuizToDelete(quiz);
        setShowDelete(show)
    }

    const handleShowMoreOptions = (quizId: number | null) => {
        setQuizzes(prevQuizzes =>
            prevQuizzes.map(quiz =>
                quiz.quizId === quizId || quiz.showOptions === true
                ? { ...quiz, showOptions: !quiz.showOptions} 
                : quiz
            )
        )
    }

    return (
        <div>
            <div className="quizzes-wrapper">
            <h1>Quizzes</h1>
            <SearchBar query={query} placeholder="Search for a quiz" handleSearch={setQuery}/>
            <hr /><br />
            <div className="quiz-cards-container">
                {quizzes.length === 0 ? (
                <h3>There are no quizzes to show</h3>
                ) : filteredQuizzes.length === 0 ? (
                <h3>There are no quizzes matching "{query}"</h3>
                ) : (
                filteredQuizzes.map((quiz) => (
                    <div className="quiz-entry" key={quiz.quizId}>
                        <QuizCard
                            quizId={quiz.quizId!}
                            name={quiz.name}
                            description={quiz.description}
                            numOfQuestions={quiz.numOfQuestions!}
                            showOptions={quiz.showOptions} />
                        <div className="quiz-options" onClick={(e) => e.stopPropagation()}>
                            <div className="quiz-edit" onClick={() => manageQuiz(quiz.quizId!)}><Settings /></div>
                            <div className="quiz-delete" onClick={() => handleShowDelete(quiz, true)}><Delete /></div>
                        </div>
                        {user && (
                            <button className="quiz-more-button" onClick={(e) => {e.stopPropagation(); handleShowMoreOptions(quiz.quizId!)}}>
                                {quiz.showOptions ? <Close /> : <MoreVert />}
                            </button>
                        )}
                    </div>
                ))
                )}
            </div>
            {user && (
                <button className="quiz-create" onClick={() => navigate("/quiz/create")}>Create</button>
            )}
            </div>
            {showDelete 
            ? <div className="confirm-delete" onClick={() => handleShowDelete(null, false)}>
                <div className="confirm-delete-content" onClick={(e) => e.stopPropagation()}>
                    <h2>Do you want to delete this quiz?</h2>
                    <h1>{quizToDelete?.name}</h1>
                    <div className="flash-card-quiz-popup-buttons">
                        <button className="button" onClick={() => handleShowDelete(null, false)}>Cancel</button>
                        <button className="button delete-button" onClick={() => handleDelete(quizToDelete?.quizId!)} >Delete</button>
                    </div>
                </div>
            </div> 
            : ""}
        </div>
    )
}

export default QuizzesPage