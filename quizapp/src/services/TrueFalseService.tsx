import { TrueFalse } from "../types/trueFalse";

const API_URL = import.meta.env.VITE_API_URL;

const headers = {
    "Content-Type": "application/json",
};

const handleResponse = async (response: Response) => {
    if (response.ok) return response.status === 204 ? null : response.json();
    throw new Error(await response.text());
};

export const fetchTrueFalseQuestions = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/TrueFalseAPI/getQuestions/${quizId}`);
    return handleResponse(response);
};

export const createTrueFalse = async (question: TrueFalse) => {
    const response = await fetch(`${API_URL}/api/TrueFalseAPI/create`, {
        method: "POST",
        headers,
        body: JSON.stringify(question),
    });
    return handleResponse(response);
};

export const updateTrueFalse = async (id: number, question: TrueFalse) => {
    const response = await fetch(`${API_URL}/api/TrueFalseAPI/update/${id}`, {
        method: "PUT",
        headers,
        body: JSON.stringify(question),
    });
    return handleResponse(response);
};

export const deleteTrueFalse = async (id: number, qNum: number, quizId: number) => {
    const response = await fetch(
        `${API_URL}/api/TrueFalseAPI/delete/${id}?qNum=${qNum}&quizId=${quizId}`,
        { method: "DELETE" }
    );
    return handleResponse(response);
};
