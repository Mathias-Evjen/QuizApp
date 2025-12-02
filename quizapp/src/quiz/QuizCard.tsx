import { useNavigate } from "react-router-dom";
import "./style/Quiz.css";

interface QuizCardProps {
    quizId: number;
    name: string;
    description: string;
    numOfQuestions: number;
    showOptions?: boolean;
}

const QuizCard: React.FC<QuizCardProps> = ({ quizId, name, description, numOfQuestions, showOptions}) => {
    const navigate = useNavigate();

    const handleOpen = () => {
        navigate(`/quiz/${quizId}`);
    }

    return(
        <div className={`quiz-card-box ${showOptions ? "show-options" : ""}`} onClick={handleOpen}>
            <p className="quiz-card-name">{name}</p>
            <div className="">
                <p className="quiz-card-desc">"{description}"</p>
            </div>
            <p className="quiz-card-num-questions">Questions: {numOfQuestions}</p>
            <div className="quiz-card-buttons">
            </div>
        </div>
    )
}

export default QuizCard;