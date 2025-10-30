export interface FlashCard {
    flashCardId?: number;
    question: string;
    answer: string;
    showAnswer?: boolean;
    quizId: number
    quizQuestionNum: number;
    isDirty?: boolean;
    isNew?: boolean
    tempId?: number
}