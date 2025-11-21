import { MatchingAttempt } from "../../types/matchingAttempt";
import { Matching } from "../../types/matching";

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
    const token = localStorage.getItem("token");
    const headers: HeadersInit = {
        "Content-Type": "application/json",
    };
    if (token) {
        headers["Authorization"] = `Bearer ${token}`;
    }
    return headers;
};

// Get matchings
export const fetchMatchings = async (quizId: number) => {
  console.log(quizId);
  const response = await fetch(`${API_URL}/api/MatchingAPI/getQuestions/${quizId}`);
  return handleResponse(response);
};

// Post create matching
export const createMatching = async (matching: Matching) => {
  const response = await fetch(`${API_URL}/api/matchingapi/create`, {
    method: 'POST',
    headers: getAuthHeaders(),
    body: JSON.stringify(matching),
  });
  return handleResponse(response);
};

//Submit matching attempt
export const submitQuestion = async (matchingAttempt: MatchingAttempt) => {
  const response = await fetch(`${API_URL}/api/matchingapi/submitQuestion`, {
    method: 'POST',
    headers,
    body: JSON.stringify(matchingAttempt),
  });
  return handleResponse(response);
};

// Put update matching
export const updateMatching = async (matchingId: number, matching: Matching) => {
  const response = await fetch(`${API_URL}/api/matchingapi/update/${matchingId}`, {
    method: 'PUT',
    headers: getAuthHeaders(),
    body: JSON.stringify(matching),
  });
  return handleResponse(response);
};

// Delete matching
export const deleteMatching = async (matchingId: number, quizQuestionNum: number, quizId: number) => {
  const response = await fetch(`${API_URL}/api/matchingapi/delete/${matchingId}?quizQuestionNum=${quizQuestionNum}&quizId=${quizId}`, {
    method: 'DELETE',
    headers: getAuthHeaders()
  });
  return handleResponse(response);
};

export function splitQuestion(question: string): { keys: string[]; values: string[] } {
  if (!question || question.trim() === "") {
    return { keys: [], values: [] };
  }
  const parts = question.split(",");
  if (parts.length % 2 !== 0) {
    throw new Error("Amount of values in question must be even");
  }

  const keys: string[] = [];
  const values: string[] = [];

  for (let i = 0; i < parts.length; i += 2) {
    keys.push(parts[i].trim());
    values.push(parts[i + 1].trim());
  }

  return { keys, values };
}


export function assemble(splitQuestion: { keys: string[]; values: string[] } | null): string {
  if (!splitQuestion || splitQuestion.keys.length === 0 || splitQuestion.values.length === 0) {
    return "Empty lists!";
  }

  if (splitQuestion.keys.length !== splitQuestion.values.length) {
    return "Lists are not the same length!";
  }

  return splitQuestion.keys
    .map((key, i) => `${key},${splitQuestion.values[i]}`)
    .join(",");
}

export function shuffleQuestion(keys: string[], values: string[]) {
    if (!values || values.length === 0) {
        throw new Error("Values list is empty or null");
    }

    // Lag en kopi slik at originalen ikke muteres
    const shuffledValues = [...values];

    // Fisher-Yates shuffle
    for (let i = shuffledValues.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        const temp = shuffledValues[i];
        shuffledValues[i] = shuffledValues[j];
        shuffledValues[j] = temp;
    }

    return {
        keys: [...keys],        // returnerer uendret kopi
        values: shuffledValues  // returnerer stokket liste
    };
}
