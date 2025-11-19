import { QuestionBase } from "./Question";

export interface FillInTheBlank extends QuestionBase {
    questionType: "fillInTheBlank";

    fillInTheBlankId?: number;
    question: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;

    isDirty?: boolean;
    isNew?: boolean;
    tempId?: number;
}