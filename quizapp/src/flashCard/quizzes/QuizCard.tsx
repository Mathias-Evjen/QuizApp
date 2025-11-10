import { useNavigate } from "react-router-dom";
import { PiCards } from "react-icons/pi";

interface QuizCardProps {
    id?: number;
    name: string;
    description?: string;
    numOfQuestions?: number;
    showOptions: boolean;
}

const QuizCard: React.FC<QuizCardProps> = ({ id, name, description, numOfQuestions, showOptions}) => {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate(`/flashCards/${id}`);
    }

    return(
        <div className={`flash-card-quiz-card ${showOptions ? "show-options" : ""}`} onClick={handleClick}>
            <div className="num-of-cards">
                {numOfQuestions}
                <PiCards />
            </div>
            <div className="flash-card-quiz-card-content">
                <div className="flash-card-quiz-title">
                    <h3>{name}</h3>
                </div>
                <div className="flash-card-quiz-description">
                    <p>{description}</p>
                </div>
            </div>
        </div>
    )
}

export default QuizCard;