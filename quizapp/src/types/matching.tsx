import { QuestionBase } from "./Question";

export interface Matching extends QuestionBase {
    questionType: "matching";

    matchingCardId: number;
    question: string;
    questionText: string;
    answer: string;
    quizId: number;
    quizQuestionNum: number;
    keys: string[];
    values: string[];
}
