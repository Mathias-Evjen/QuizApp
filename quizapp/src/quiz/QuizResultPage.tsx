import { useLocation, useNavigate } from "react-router-dom";

const QuizResultPage: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();

    const { results } = location.state || {};

    return
}