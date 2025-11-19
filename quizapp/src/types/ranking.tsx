import { QuestionBase } from "./Question";

export interface Ranking extends QuestionBase {
    questionType: "ranking";

    rankingId?: number;
    question: string;
    questionText: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;

    isDirty?: boolean;
    isNew?: boolean;
}
