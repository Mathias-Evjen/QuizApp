export interface TrueFalseAttempt {
    trueFalseAttemptId?: number;
    trueFalseId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: boolean | null;
    answeredCorrectly?: boolean;
}