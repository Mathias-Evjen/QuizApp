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

// Get matchings
export const fetchMatchings = async (quizId: number) => {
  const response = await fetch(`${API_URL}/api/matchingapi/matchinglist/${quizId}`);
  return handleResponse(response);
};
// Post create matching
export const createMatching = async (matching: any) => {
  const response = await fetch(`${API_URL}/api/matchingapi/create`, {
    method: 'POST',
    headers,
    body: JSON.stringify(matching),
  });
  return handleResponse(response);
};
//Submit matching attempt
export const submitMatchingAttempt = async (quizAttemptId: number, matching: any) => {
  const response = await fetch(`${API_URL}/api/matchingapi/submitAttempts/${quizAttemptId}"`, {
    method: 'POST',
    headers,
    body: JSON.stringify(matching),
  });
  return handleResponse(response);
};
// Put update matching
export const updateMatching = async (matchingId: number, matching: any) => {
  const response = await fetch(`${API_URL}/api/matchingapi/update/${matchingId}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(matching),
  });
  return handleResponse(response);
};
// Delete matching
export const deleteMatching = async (matchingId: number) => {
  const response = await fetch(`${API_URL}/api/matchingapi/delete/${matchingId}`, {
    method: 'DELETE',
  });
  return handleResponse(response);
};