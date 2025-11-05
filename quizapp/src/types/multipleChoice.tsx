export interface Option {
  text: string;
  isCorrect: boolean;
}

export interface MultipleChoice {
  multipleChoiceId?: number;
  question: string;
  correctAnswer?: string;
  quizId: number;
  quizQuestionNum: number;
  options: Option[];
}
