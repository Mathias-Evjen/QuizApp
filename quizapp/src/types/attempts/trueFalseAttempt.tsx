import { QuestionAttemptBase } from "./questionAttempt";

export interface TrueFalseAttempt extends QuestionAttemptBase {
    questionType: "trueFalse";

    trueFalseAttemptId?: number;
    trueFalseId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: boolean | null;
    answeredCorrectly?: boolean;
}