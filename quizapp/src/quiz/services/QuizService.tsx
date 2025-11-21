import { QuizAttempt } from '../../types/attempts/quizAttempt';
import { Quiz } from "../../types/quiz/quiz";

const API_URL = import.meta.env.VITE_API_URL;

const headers = {
  'Content-Type': 'application/json',
};

const handleResponse = async (response: Response) => {
    if (response.ok) {  // HTTP status code success 200-299
        if (response.status === 204) { // Detele returns 204 No content
            return null;
        }
        return response.json(); // other returns response body as JSON
    } else {
        const errorText = await response.text();
        throw new Error(errorText || 'Network response was not ok');
    }
};

const getAuthHeaders = () => {
    const token = localStorage.getItem('token');
    const headers: HeadersInit = {
        "Content-Type": "application/json",
    };
    if (token) {
        headers["Authorization"] = `Bearer ${token}`;
    }
    return headers;
};

// Get quizzes
export const fetchQuizzes = async () => {
    const response = await fetch(`${API_URL}/api/quizapi/getquizzes`);
    return handleResponse(response);
};

// Get quiz
export const fetchQuiz = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/quizapi/getquiz/${quizId}`);
    return handleResponse(response);
};

// Post create quiz
export const createQuiz = async (quiz: Quiz) => {
    const response = await fetch(`${API_URL}/api/quizapi/create`, {
        method: 'POST',
        headers: getAuthHeaders(),
        body: JSON.stringify(quiz),
    });
    return handleResponse(response);
};

//Create quiz attempt
export const createQuizAttempt = async (quizAttempt: QuizAttempt) => {
    const response = await fetch(`${API_URL}/api/quizapi/createAttempt`, {
        method: 'POST',
        headers,
        body: JSON.stringify(quizAttempt),
    });
    return handleResponse(response);
};

// Put update quiz
export const updateQuiz = async (quizId: number, quiz: Quiz) => {
    const response = await fetch(`${API_URL}/api/quizapi/update/${quizId}`, {
        method: 'PUT',
        headers: getAuthHeaders(),
        body: JSON.stringify(quiz),
    });
    return handleResponse(response);
};

// Delete quiz
export const deleteQuiz = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/quizapi/delete/${quizId}`, {
        method: 'DELETE',
        headers: getAuthHeaders()
    });
    return handleResponse(response);
};

export const getQuizRoute = (quiz: any, currentQuestionNum: number): string => {
    const question = quiz.allQuestions?.[currentQuestionNum - 1];
    if (!question) return "/";

    if ("fillInTheBlankId" in question) return "/quizFillInTheBlank";
    if ("matchingId" in question)
        return "/matchingQuiz";
    if ("sequenceId" in question)
        return "/sequenceQuiz";
    if ("rankingId" in question)
        return "/rankingQuiz";
    if ("multipleChoiceId" in question)
        return "/quizMultipleChoice";
    if ("trueFalseId" in question)
        return "/quizTrueFalse";

    return "/";
};