import { QuestionBase } from "./Question";

export interface Ranking extends QuestionBase {
    questionType: "ranking";

    rankingCardId: number;
    question: string;
    questionText: string;
    answer: string;
    correctAnswer: string;
    quizId: number;
    quizQuestionNum: number;
}
