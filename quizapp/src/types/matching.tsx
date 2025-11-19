import { QuestionBase } from "./Question";

export interface Matching extends QuestionBase {
    questionType: "matching";

    matchingId?: number;
    question: string;
    questionText: string;
    answer: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;

    isDirty?: boolean;
    isNew?: boolean;
    tempId?: number;
}
