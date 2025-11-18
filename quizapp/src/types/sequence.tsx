import { QuestionBase } from "./Question";

export interface Sequence extends QuestionBase {
    questionType: "sequence";

    sequenceId?: number;
    question: string;
    questionText: string;
    answer: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;
}
