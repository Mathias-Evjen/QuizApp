import { QuestionAttemptBase } from "./questionAttempt";

export interface MultiplechoiceAttempt extends QuestionAttemptBase {
    questionType: "multipleChoice";

    multipleChoiceAttemptId?: number;
    multipleChoiceId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}