import { FlashCardQuiz } from "../types/flashCardQuiz";

const API_URL = import.meta.env.VITE_API_URL;

const headers = {
    "Content-Type": "application/json",
};

const handleResponse = async (response: Response) => {
    if (response.ok) {
        if (response.status === 204)
            return null;
        return response.json();
    } else {
        const errorText = await response.json();
        throw new Error(errorText || "Netork response was not ok");
    }
}

// Get quizzes
export const fetchQuizzes = async () => {
    const response = await fetch(`${API_URL}/api/flashcardquizapi/getQuizzes`);
    return handleResponse(response);
}

// Get quiz by id
export const fetchQuizById = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/flashcardquizapi/getQuiz/${quizId}`);
    return handleResponse(response);
}

// Post create quiz
export const createQuiz = async (quiz: FlashCardQuiz) => {
    const response = await fetch(`${API_URL}/api/flashcardquizapi/create`, {
        method: "POST",
        headers,
        body: JSON.stringify(quiz),
    });
    return handleResponse(response);
}

// Put update quiz
export const updateQuiz = async (quizId: number, quiz: FlashCardQuiz) => {
    const response = await fetch(`${API_URL}/api/flashcardquizapi/update/${quizId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(quiz)
    });
    return handleResponse(response);
}

// Delete quiz
export const deleteQuiz = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/flashcardquizapi/delete/${quizId}`, {
        method: "DELETE"
    });
    return handleResponse(response);
}