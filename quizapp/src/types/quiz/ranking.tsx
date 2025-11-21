import { QuestionBase } from "./quiz/Question";

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
