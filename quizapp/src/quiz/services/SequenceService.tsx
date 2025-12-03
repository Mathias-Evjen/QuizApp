import { SequenceAttempt } from "../../types/attempts/sequenceAttempt";
import { Sequence } from "../../types/quiz/sequence";

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

// Get sequences
export const fetchSequences = async (quizId: number) => {
  const response = await fetch(`${API_URL}/api/SequenceAPI/getQuestions/${quizId}`);
  return handleResponse(response);
};

// Post create sequence
export const createSequence = async (sequence: Sequence) => {
  const response = await fetch(`${API_URL}/api/sequenceapi/create`, {
    method: 'POST',
    headers: getAuthHeaders(),
    body: JSON.stringify(sequence),
  });
  return handleResponse(response);
};

//Submit sequence attempt
export const submitQuestion= async (sequenceAttempt: SequenceAttempt) => {
  const response = await fetch(`${API_URL}/api/sequenceapi/submitQuestion`, {
    method: 'POST',
    headers,
    body: JSON.stringify(sequenceAttempt),
  });
  return handleResponse(response);
};

// Put update sequence
export const updateSequence = async (sequenceId: number, sequence: Sequence) => {
  const response = await fetch(`${API_URL}/api/sequenceapi/update/${sequenceId}`, {
    method: 'PUT',
    headers: getAuthHeaders(),
    body: JSON.stringify(sequence),
  });
  return handleResponse(response);
};

// Delete sequence
export const deleteSequence = async (sequenceId: number, quizQuestionNum: number, quizId: number) => {
  const response = await fetch(`${API_URL}/api/sequenceapi/delete/${sequenceId}?quizQuestionNum=${quizQuestionNum}&quizId=${quizId}`, {
    method: 'DELETE',
    headers: getAuthHeaders()
  });
  return handleResponse(response);
};


export function shuffleQuestion(values: string[]): string[] {
    if (!values || values.length === 0) {
        throw new Error("Values list is empty or null");
    }

    const shuffledValues = [...values];

    for (let i = shuffledValues.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        const temp = shuffledValues[i];
        shuffledValues[i] = shuffledValues[j];
        shuffledValues[j] = temp;
    }
    return shuffledValues;
}