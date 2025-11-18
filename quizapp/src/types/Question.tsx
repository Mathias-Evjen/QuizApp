import { FillInTheBlank } from "./fillInTheBlank";
import { Matching } from "./matching";
import { MultipleChoice } from "./multipleChoice";
import { Ranking } from "./ranking";
import { Sequence } from "./sequence";
import { TrueFalse } from "./trueFalse";

export interface QuestionBase {
    quizQuestionNum: number;
    questionType: QuestionType;
}

export type QuestionType = 
    | "fillInTheBlank"
    | "matching"
    | "sequence"
    | "ranking"
    | "multipleChoice"
    | "trueFalse";

export type Question =
    | FillInTheBlank
    | Matching
    | Ranking
    | Sequence
    | MultipleChoice
    | TrueFalse;