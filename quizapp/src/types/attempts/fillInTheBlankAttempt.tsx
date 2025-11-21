export interface FillInTheBlankAttempt {
    fillInTheBlankAttemptId?: number;
    fillInTheBlankId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}