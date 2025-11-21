export interface RankingAttempt {
    rankingAttemptId?: number;
    rankingId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}