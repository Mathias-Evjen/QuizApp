import { QuestionBase } from "./quiz/Question";

export interface Sequence extends QuestionBase {
    questionType: "sequence";

    sequenceId?: number;
    question: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;

    isDirty?: boolean;
    isNew?: boolean;
    tempId?: number;
}
