import { useNavigate } from "react-router-dom";

interface QuizCardProps {
    id: number;
    name: string;
    description?: string;
    numOfQuestions: number;
}

const QuizCard: React.FC<QuizCardProps> = ({ id, name, description, numOfQuestions}) => {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate(`/flashCardQuiz/${id}`);
    }

    return(
        <div className="flash-card-quiz-card" onClick={handleClick}>
            <div className="flash-card-quiz-title">
                <h3>{name}</h3>
            </div>
            <div className="flash-card-quiz-description">
                <div className="flash-card-quiz-description-top-bar">
                    <h5>Description:</h5>
                </div>
                <p>{description}</p>
            </div>
        </div>
    )
}

export default QuizCard;