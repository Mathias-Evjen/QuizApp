import { RankingAttempt } from "../../types/rankingAttempt";
import { Ranking } from "../../types/ranking";

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

// Get rankings
export const fetchRankings = async (quizId: number) => {
  console.log(quizId);
  const response = await fetch(`${API_URL}/api/RankingAPI/getQuestions/${quizId}`);
  return handleResponse(response);
};

// Post create ranking
export const createRanking = async (ranking: Ranking) => {
  const response = await fetch(`${API_URL}/api/rankingapi/create`, {
    method: 'POST',
    headers: getAuthHeaders(),
    body: JSON.stringify(ranking),
  });
  return handleResponse(response);
};

//Submit ranking attempt
export const submitQuestion = async (rankingAttempt: RankingAttempt) => {
  const response = await fetch(`${API_URL}/api/rankingapi/submitQuestion`, {
    method: 'POST',
    headers,
    body: JSON.stringify(rankingAttempt),
  });
  return handleResponse(response);
};

// Put update ranking
export const updateRanking = async (rankingId: number, ranking: Ranking) => {
  const response = await fetch(`${API_URL}/api/rankingapi/update/${rankingId}`, {
    method: 'PUT',
    headers: getAuthHeaders(),
    body: JSON.stringify(ranking),
  });
  return handleResponse(response);
};

// Delete ranking
export const deleteRanking = async (rankingId: number, quizQuestionNum: number, quizId: number) => {
  const response = await fetch(`${API_URL}/api/rankingapi/delete/${rankingId}?quizQuestionNum=${quizQuestionNum}&quizId=${quizId}`, {
    method: 'DELETE',
    headers: getAuthHeaders()
  });
  return handleResponse(response);
};


export function shuffleQuestion(values: string[]): string[] {
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

    return values;
}