import { FillInTheBlankAttempt } from "./fillInTheBlankAttempt";
import { MatchingAttempt } from "./matchingAttempt";
import { MultiplechoiceAttempt } from "./MultipleChoiceAttempt";
import { RankingAttempt } from "./rankingAttempt";
import { SequenceAttempt } from "./sequenceAttempt";
import { TrueFalseAttempt } from "./trueFalseAttempt";


export interface QuestionAttemptBase {
    questionType: QuestionType;
}

export type QuestionType = 
    | "fillInTheBlank"
    | "matching"
    | "sequence"
    | "ranking"
    | "multipleChoice"
    | "trueFalse";

export type QuestionAttempt =
    | FillInTheBlankAttempt
    | MatchingAttempt
    | RankingAttempt
    | SequenceAttempt
    | MultiplechoiceAttempt
    | TrueFalseAttempt;