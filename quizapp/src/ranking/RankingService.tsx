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

// Get rankings
export const fetchRankings = async (quizId: number) => {
  console.log(quizId);
  const response = await fetch(`${API_URL}/api/RankingAPI/getQuestions/${quizId}`);
  return handleResponse(response);
};
// Post create ranking
export const createRanking = async (ranking: any) => {
  const response = await fetch(`${API_URL}/api/rankingapi/create`, {
    method: 'POST',
    headers,
    body: JSON.stringify(ranking),
  });
  return handleResponse(response);
};
//Submit ranking attempt
export const submitRankingAttempt = async (quizAttemptId: number, ranking: any) => {
  const response = await fetch(`${API_URL}/api/rankingapi/submitAttempts/${quizAttemptId}"`, {
    method: 'POST',
    headers,
    body: JSON.stringify(ranking),
  });
  return handleResponse(response);
};
// Put update ranking
export const updateRanking = async (rankingId: number, ranking: any) => {
  const response = await fetch(`${API_URL}/api/rankingapi/update/${rankingId}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(ranking),
  });
  return handleResponse(response);
};
// Delete ranking
export const deleteRanking = async (rankingId: number, quizQuestionNum: number, quizId: number) => {
  const response = await fetch(`${API_URL}/api/rankingapi/delete/${rankingId}?quizQuestionNum=${quizQuestionNum}&quizId=${quizId}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};