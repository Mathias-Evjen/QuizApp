import { QuestionBase } from "./Question";

export interface Sequence extends QuestionBase {
    questionType: "sequence";

    sequenceId?: number;
    question: string;
    questionText: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;

    isDirty?: boolean;
    isNew?: boolean;
    tempId?: number;
}
