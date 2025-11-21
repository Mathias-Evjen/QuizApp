import { QuestionBase } from "./quiz/Question";

export interface TrueFalse extends QuestionBase {
  questionType: "trueFalse";

  trueFalseId?: number;
  question: string;
  correctAnswer: boolean;
  quizId: number;
  quizQuestionNum: number;
  
  isNew?: boolean;
  isDirty?: boolean;
  tempId?: number;
}
