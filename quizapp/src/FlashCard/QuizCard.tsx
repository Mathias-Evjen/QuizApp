
interface QuizCardProps {
    name: string;
    description: string | undefined;
    numOfQuestions: number;
}

const QuizCard: React.FC<QuizCardProps> = ({ name, description, numOfQuestions}) => {
    return(
        <div className="flash-card-quiz-card">
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