import { QuestionBase } from "./question";

export interface Matching extends QuestionBase {
    questionType: "matching";

    matchingId?: number;
    question: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;

    isDirty?: boolean;
    isNew?: boolean;
    tempId?: number;
}
