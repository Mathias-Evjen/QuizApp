export interface SequenceAttempt {
    sequenceAttemptId?: number;
    sequenceId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}