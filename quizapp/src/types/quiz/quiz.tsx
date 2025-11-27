export interface Quiz {
    quizId?: number;
    name: string;
    description: string;
    numOfQuestions?: number;

    showOptions?: boolean;
    
    isDirty?: boolean;
}