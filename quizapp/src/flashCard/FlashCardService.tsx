import { FlashCard } from "../types/flashCard";

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

// Get flash cards
export const fetchFlashCards = async (quizId: number) => {
    const response = await fetch(`${API_URL}/api/flashcardapi/getFlashCards/${quizId}`);
    return handleResponse(response);
}

// Post create flash card
export const createFlashCard = async (flashCard: FlashCard) => {
    const response = await fetch(`${API_URL}/api/flashcardapi/create`, {
        method: "POST",
        headers,
        body: JSON.stringify(flashCard)
    });
    return handleResponse(response);
}

// Put update flash card
export const updateFlashCard = async (flashCard: FlashCard) => {
    const response = await fetch(`${API_URL}/api/flashcardapi/update/${flashCard.flashCardId}`, {
        method: "PUT",
        headers,
        body: JSON.stringify(flashCard)
    });
    return handleResponse(response);
}

// Delete flash card
export const deleteFlashCard = async (flashCardId: number) => {
    const response = await fetch(`${API_URL}/api/flashcardapi/delete/${flashCardId}`, {
        method: "DELETE"
    });
    return handleResponse(response);
}