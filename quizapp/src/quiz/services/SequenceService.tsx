import { SequenceAttempt } from "../../types/sequenceAttempt";

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

// Get sequences
export const fetchSequences = async (quizId: number) => {
  console.log(quizId);
  const response = await fetch(`${API_URL}/api/SequenceAPI/getQuestions/${quizId}`);
  return handleResponse(response);
};

// Post create sequence
export const createSequence = async (sequence: any) => {
  const response = await fetch(`${API_URL}/api/sequenceapi/create`, {
    method: 'POST',
    headers,
    body: JSON.stringify(sequence),
  });
  return handleResponse(response);
};

//Submit sequence attempt
export const submitQuestion= async (sequenceAttempt: SequenceAttempt) => {
  const response = await fetch(`${API_URL}/api/sequenceapi/submitQuestion"`, {
    method: 'POST',
    headers,
    body: JSON.stringify(sequenceAttempt),
  });
  return handleResponse(response);
};

// Put update sequence
export const updateSequence = async (sequenceId: number, sequence: any) => {
  const response = await fetch(`${API_URL}/api/sequenceapi/update/${sequenceId}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(sequence),
  });
  return handleResponse(response);
};

// Delete sequence
export const deleteSequence = async (sequenceId: number, quizQuestionNum: number, quizId: number) => {
  const response = await fetch(`${API_URL}/api/sequenceapi/delete/${sequenceId}?quizQuestionNum=${quizQuestionNum}&quizId=${quizId}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};