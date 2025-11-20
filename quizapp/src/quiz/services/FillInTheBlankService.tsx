import { FillInTheBlank } from "../../types/fillInTheBlank";
import { FillInTheBlankAttempt } from "../../types/fillInTheBlankAttempt";


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

const getAuthHeaders = () => {
    const token = localStorage.getItem("token");
    const headers: HeadersInit = {
        "Content-Type": "application/json",
    };
    if (token) {
        headers["Authorization"] = `Bearer ${token}`;
    }
    return headers;
}

// Get questions
export const fetchQuestions = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/fillintheblankapi/getQuestions/${quizId}`);
    return handleResponse(response);
}

// Get question attempts
export const fetchAttempts = async (quizAttemptId: number) => {
    const response = await fetch(`${API_URL}/api/fillintheblankapi/getAttempts/${quizAttemptId}`);
    return handleResponse(response);
}

// Post submit question
export const submitQuestion = async (attempt: FillInTheBlankAttempt) => {
    const response = await fetch(`${API_URL}/api/fillintheblankapi/submitQuestion`, {
        method: "POST",
        headers,
        body: JSON.stringify(attempt)
    });
    return handleResponse(response);
}

// Post create question
export const createQuestion = async (question: FillInTheBlank) => {
    const response = await fetch(`${API_URL}/api/fillintheblankapi/create`, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(question)
    });
    return handleResponse(response);
}

// Put update question
export const updateQuestion = async (fillInTheBlankId: number, question: FillInTheBlank) => {
    const response = await fetch(`${API_URL}/api/fillintheblankapi/update/${fillInTheBlankId}`, {
        method: "PUT",
        headers: getAuthHeaders(),
        body: JSON.stringify(question)
    });
    return handleResponse(response);
}

// Delete question
export const deleteQuestion = async (fillInTheBlankId: number, quizQuestionNum: number, quizId: number) => {
    const params = new URLSearchParams({
        qNum: `${quizQuestionNum}`,
        quizId: `${quizId}`
    });
    const response = await fetch(`${API_URL}/api/fillintheblankapi/delete/${fillInTheBlankId}?${params}`, {
        method: "DELETE",
        headers: getAuthHeaders()
    });
    return handleResponse(response);
}