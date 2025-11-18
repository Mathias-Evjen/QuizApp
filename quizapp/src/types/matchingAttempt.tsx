export interface MatchingAttempt {
    matchingAttemptId?: number;
    matchingId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnser: string;
    answeredCorrectly?: boolean;
}