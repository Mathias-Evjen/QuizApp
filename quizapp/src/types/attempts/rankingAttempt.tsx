import { QuestionAttemptBase } from "./questionAttempt";

export interface RankingAttempt extends QuestionAttemptBase {
    questionType: "ranking";

    rankingAttemptId?: number;
    rankingId: number;
    quizAttemptId: number;
    quizQuestionNum: number;
    userAnswer: string;
    answeredCorrectly?: boolean;
}