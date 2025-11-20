import { MultipleChoice } from "../../types/multipleChoice";
import { MultiplechoiceAttempt } from "../../types/MultipleChoiceAttempt";


const API_URL = import.meta.env.VITE_API_URL;

const headers = {
  'Content-Type': 'application/json',
};


const handleResponse = async (response: Response) => {
  if (response.ok) {
    if (response.status === 204) {
      return null;
    }
    return response.json();
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

export const fetchMultipleChoiceQuestions = async (quizId: number) => {
  const response = await fetch(`${API_URL}/api/MultipleChoiceAPI/getQuestions/${quizId}`);
  console.log(response);
  return handleResponse(response);
};


export const fetchMultipleChoiceById = async (id: number) => {
  const response = await fetch(`${API_URL}/api/MultipleChoiceAPI/getById/${id}`);
  return handleResponse(response);
};

// Post submit question
export const submitQuestion = async (mcAttempt: MultiplechoiceAttempt) => {
  const response = await fetch(`${API_URL}/api/MultipleChoiceAPI/submitQuestion`, {
    method: "POST",
    headers,
    body: JSON.stringify(mcAttempt)
  });
  return handleResponse(response);
};

export const createMultipleChoice = async (question: MultipleChoice) => {
  const response = await fetch(`${API_URL}/api/MultipleChoiceAPI/create`, {
    method: 'POST',
    headers: getAuthHeaders(),
    body: JSON.stringify(question),
  });
  return handleResponse(response);
};


export const updateMultipleChoice = async (id: number, question: MultipleChoice) => {
  const response = await fetch(`${API_URL}/api/MultipleChoiceAPI/update/${id}`, {
    method: 'PUT',
    headers: getAuthHeaders(),
    body: JSON.stringify(question),
  });
  return handleResponse(response);
};


export const deleteMultipleChoice = async (id: number, qNum: number, quizId: number) => {
  const response = await fetch(`${API_URL}/api/MultipleChoiceAPI/delete/${id}?qNum=${qNum}&quizId=${quizId}`, {
    method: 'DELETE',
    headers: getAuthHeaders()
  });
  return handleResponse(response);
};