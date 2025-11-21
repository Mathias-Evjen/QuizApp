export interface MatchingAttempt {
    matchingAttemptId?: number;
    matchingId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}