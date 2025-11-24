import { FlashCard } from "../types/flashcard/flashCard";

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

// Get flash cards
export const fetchFlashCards = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/flashcardapi/getFlashCards/${quizId}`);
    return handleResponse(response);
}

// Post create flash card
export const createFlashCard = async (flashCard: FlashCard) => {
    const response = await fetch(`${API_URL}/api/flashcardapi/create`, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(flashCard)
    });
    return handleResponse(response);
}

// Put update flash card
export const updateFlashCard = async (flashCard: FlashCard) => {
    const response = await fetch(`${API_URL}/api/flashcardapi/update/${flashCard.flashCardId}`, {
        method: "PUT",
        headers: getAuthHeaders(),
        body: JSON.stringify(flashCard)
    });
    return handleResponse(response);
}

// Delete flash card
export const deleteFlashCard = async (flashCardId: number, quizQuestionNum: number, quizId: number) => {
    const params = new URLSearchParams({ 
        qNum: `${quizQuestionNum}`, 
        quizId: `${quizId}`
    });
    const response = await fetch(`${API_URL}/api/flashcardapi/delete/${flashCardId}?${params}`, {
        method: "DELETE",
        headers: getAuthHeaders()
    });
    return handleResponse(response);
}