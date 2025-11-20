import { useLocation, useNavigate } from "react-router-dom";

const QuizResultPage: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { score, total } = location.state || { score: 0, total: 0 };

    return (
        <div>
            <h1>Quiz Result</h1>
            <h2>{score} / {total} correct</h2>
            <button>Back to quizzes</button>
        </div>
    );
};

export default QuizResultPage;