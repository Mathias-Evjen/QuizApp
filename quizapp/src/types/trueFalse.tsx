export interface TrueFalse {
  trueFalseId?: number;
  question: string;
  correctAnswer: boolean;
  quizId: number;
  quizQuestionNum: number;
  isNew?: boolean;
  isDirty?: boolean;
  tempId?: number;
}
