import { QuestionBase } from "./Question";

export interface FillInTheBlank extends QuestionBase {
    questionType: "fillInTheBlank";

    fillInTheBlankId?: number;
    question: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;
    correctAnswer?: string;

    isDirty?: boolean;
    isNew?: boolean;
    tempId?: number;
}