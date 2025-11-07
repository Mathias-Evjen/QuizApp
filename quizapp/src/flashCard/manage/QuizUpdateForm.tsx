import FlashCardQuizForm from "../FlashCardQuizForm";

interface QuizUpdateProps {
    handleCancel: (value: boolean) => void;
    onSave: (name: string, description: string) => void;
    name: string;
    description: string;
}

type QuizFormData = {
    name: string;
    description: string;
}

const QuizUpdateForm: React.FC<QuizUpdateProps> = ({ name, description, handleCancel, onSave }) => {
    
    const onSubmit = (data: QuizFormData) => {
        onSave(data.name, data.description);
    }

    return(
        <FlashCardQuizForm
            onSubmit={onSubmit}
            onCancel={handleCancel}
            isUpdate={true}
            name={name}
            description={description} />
    )
}

export default QuizUpdateForm;