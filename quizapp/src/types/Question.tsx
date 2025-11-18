import { FillInTheBlank } from "./fillInTheBlank";
import { Matching } from "./matching";
import { Ranking } from "./ranking";
import { Sequence } from "./sequence";

export interface QuestionBase {
    quizQuestionNum: number;
    questionType: QuestionType;
}

export type QuestionType = 
    | "fillInTheBlank"
    | "matching"
    | "sequence"
    | "ranking"
    | "trueFalse"
    | "multipleChoice";

export type Question =
    | FillInTheBlank
    | Matching
    | Ranking
    | Sequence;