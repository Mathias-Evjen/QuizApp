import { QuestionBase } from "./Question";

export interface FillInTheBlank extends QuestionBase {
    questionType: "fillInTheBlank";

    fillInTheBlankId?: number;
    question: string;
    quizId: number;
    correctAnswer?: string;

    isDirty?: boolean;
    isNew?: boolean;
    tempId?: number;
}