import { QuestionAttemptBase } from "./questionAttempt";

export interface MatchingAttempt extends QuestionAttemptBase {
    questionType: "matching";

    matchingAttemptId?: number;
    matchingId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}