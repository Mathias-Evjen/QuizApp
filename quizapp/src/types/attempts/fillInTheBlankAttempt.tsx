import { QuestionAttemptBase } from "./questionAttempt";

export interface FillInTheBlankAttempt extends QuestionAttemptBase {
    questionType: "fillInTheBlank";

    fillInTheBlankAttemptId?: number;
    fillInTheBlankId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}