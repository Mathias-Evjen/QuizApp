export interface MultiplechoiceAttempt {
    multipleChoiceAttemptId?: number;
    multipleChoiceId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string[];
    answeredCorrectly?: boolean;
}