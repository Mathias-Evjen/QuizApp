import { QuestionAttemptBase } from "./questionAttempt";

export interface SequenceAttempt extends QuestionAttemptBase {
    questionType: "sequence";

    sequenceAttemptId?: number;
    sequenceId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}