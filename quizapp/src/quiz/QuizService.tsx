const API_URL = "http://localhost:5041";

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

// Get quizzes
export const fetchQuizzes = async (quizId: number) => {
  const response = await fetch(`${API_URL}/api/quizapi/quizlist/${quizId}`);
  return handleResponse(response);
};
// Post create quiz
export const createQuiz = async (quiz: any) => {
  const response = await fetch(`${API_URL}/api/quizapi/create`, {
    method: 'POST',
    headers,
    body: JSON.stringify(quiz),
  });
  return handleResponse(response);
};
//Submit quiz attempt
export const submitQuizAttempt = async (quizAttemptId: number, quiz: any) => {
  const response = await fetch(`${API_URL}/api/quizapi/submitAttempts/${quizAttemptId}"`, {
    method: 'POST',
    headers,
    body: JSON.stringify(quiz),
  });
  return handleResponse(response);
};
// Put update quiz
export const updateQuiz = async (quizId: number, quiz: any) => {
  const response = await fetch(`${API_URL}/api/quizapi/update/${quizId}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(quiz),
  });
  return handleResponse(response);
};
// Delete quiz
export const deleteQuiz = async (quizId: number) => {
  const response = await fetch(`${API_URL}/api/quizapi/delete/${quizId}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};