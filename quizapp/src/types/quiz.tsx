import { Question } from "./Question";

export interface Quiz {
    quizId?: number;
    name: string;
    description?: string;
    numOfQuestion?: number;
}