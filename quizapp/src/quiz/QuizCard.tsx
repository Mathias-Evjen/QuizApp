import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import "./style/Quiz.css";
import { User } from "../types/user";

interface QuizCardProps {
    openManageQuiz?: (quizId: number) => void;
    quizId: number;
    name: string;
    description: string;
    numOfQuestions: number;
    user?: User | null;
}

const QuizCard: React.FC<QuizCardProps> = ({ quizId, name, description, numOfQuestions, user, openManageQuiz }) => {
    const navigate = useNavigate();

    const handleOpen = () => {
        navigate(`/quiz/${quizId}`);
    }

    return(
        <div className="quiz-card-box" onClick={handleOpen}>
            <p className="quiz-card-name">{name}</p>
            <p className="quiz-card-desc">"{description}"</p>
            <p className="quiz-card-num-questions">Questions: {numOfQuestions}</p>
            <div className="quiz-card-buttons">
            {user && (
                <button className="quiz-card-btn-manage" onClick={() => openManageQuiz && openManageQuiz(quizId)}>Manage</button>
            )}
            </div>
        </div>
    )
}

export default QuizCard;