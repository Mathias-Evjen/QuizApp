import { QuestionBase } from "./question";

export interface Ranking extends QuestionBase {
    questionType: "ranking";

    rankingId?: number;
    question: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;

    isDirty?: boolean;
    isNew?: boolean;
    tempId?: number;
}
