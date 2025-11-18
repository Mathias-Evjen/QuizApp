export interface MatchingCard {
    matchingCardId: number;
    question: string;
    questionText: string;
    answer: string;
    quizId: number;
    quizQuestionNum: number;
    keys: string[];
    values: string[];
}
