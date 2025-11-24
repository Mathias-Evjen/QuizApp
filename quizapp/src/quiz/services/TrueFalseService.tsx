import { TrueFalse } from "../../types/quiz/trueFalse";
import { TrueFalseAttempt } from "../../types/attempts/trueFalseAttempt";


const API_URL = import.meta.env.VITE_API_URL;

const headers = {
    "Content-Type": "application/json",
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
    const token = localStorage.getItem("token");
    const headers: HeadersInit = {
        "Content-Type": "application/json",
    };
    if (token) {
        headers["Authorization"] = `Bearer ${token}`;
    }
    return headers;
};

export const fetchTrueFalseQuestions = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/TrueFalseAPI/getQuestions/${quizId}`);
    return handleResponse(response);
};

// Post submit question
export const submitQuestion = async (trueFalseAttempt: TrueFalseAttempt) => {
    const response = await fetch(`${API_URL}/api/TrueFalseAPI/submitQuestion`, {
        method: "POST",
        headers,
        body: JSON.stringify(trueFalseAttempt)
    });
    return handleResponse(response);
};

export const createTrueFalse = async (question: TrueFalse) => {
    const response = await fetch(`${API_URL}/api/TrueFalseAPI/create`, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(question),
    });
    return handleResponse(response);
};

export const updateTrueFalse = async (id: number, question: TrueFalse) => {
    const response = await fetch(`${API_URL}/api/TrueFalseAPI/update/${id}`, {
        method: "PUT",
        headers: getAuthHeaders(),
        body: JSON.stringify(question),
    });
    return handleResponse(response);
};

export const deleteTrueFalse = async (id: number, qNum: number, quizId: number) => {
    const response = await fetch(
        `${API_URL}/api/TrueFalseAPI/delete/${id}?qNum=${qNum}&quizId=${quizId}`,
        { method: "DELETE", headers: getAuthHeaders() }
    );
    return handleResponse(response);
};
