import { MultipleChoice } from "../types/multipleChoice";

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


export const fetchMultipleChoiceQuestions = async (quizId: number) => {
  const response = await fetch(`${API_URL}/api/multiplechoiceapi/getQuestions/${quizId}`);
  return handleResponse(response);
};


export const fetchMultipleChoiceById = async (id: number) => {
  const response = await fetch(`${API_URL}/api/multiplechoiceapi/getById/${id}`);
  return handleResponse(response);
};


export const createMultipleChoice = async (question: MultipleChoice) => {
  const response = await fetch(`${API_URL}/api/multiplechoiceapi/create`, {
    method: 'POST',
    headers,
    body: JSON.stringify(question),
  });
  return handleResponse(response);
};


export const updateMultipleChoice = async (id: number, question: MultipleChoice) => {
  const response = await fetch(`${API_URL}/api/multiplechoiceapi/update/${id}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(question),
  });
  return handleResponse(response);
};


export const deleteMultipleChoice = async (id: number, qNum: number, quizId: number) => {
  const response = await fetch(`${API_URL}/api/multiplechoice/delete/${id}?qNum=${qNum}&quizId=${quizId}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};