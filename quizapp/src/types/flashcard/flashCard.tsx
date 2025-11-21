export interface FlashCard {
    flashCardId?: number;
    question: string;
    answer: string;
    showAnswer?: boolean;
    quizId: number
    quizQuestionNum: number;
    color?: string;
    isDirty?: boolean;
    isNew?: boolean
    tempId?: number
}