import { FlashCardQuiz } from "../../types/flashCardQuiz";
import FlashCardQuizForm from "../FlashCardQuizForm";

interface CreateFormProps {
    onQuizChanged: (newQuiz: FlashCardQuiz) => void;
    flashCardQuizId?: number;
    handleCancel: (value: boolean) => void;
}

type QuizFormData = {
    name: string;
    description: string;
}

const CreateForm: React.FC<CreateFormProps> = ({ onQuizChanged, flashCardQuizId, handleCancel }) => {

    const onSubmit = async (data: QuizFormData) => {
        const quiz: FlashCardQuiz = { flashCardQuizId, ...data};
        onQuizChanged(quiz);
    }

    return(
        <FlashCardQuizForm 
            onSubmit={onSubmit} 
            onCancel={handleCancel} 
            isUpdate={false} />
    )
}

export default CreateForm;

