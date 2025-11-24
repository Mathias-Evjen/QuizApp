import { useForm } from "react-hook-form";

interface QuizFormProps {
    onSubmit: (name: string, description: string) => void;
    onCancel: (value: boolean) => void;
    isUpdate: boolean;
    name?: string;
    description?: string;
}

type QuizFormData = {
    name: string;
    description: string;
}

const FlashCardQuizForm: React.FC<QuizFormProps> = ({ isUpdate, name, description, onSubmit, onCancel }) => {
    const { register, handleSubmit, formState: { errors, isDirty } } = useForm<QuizFormData>({ 
            defaultValues: {
                name: name ? name : "", 
                description: description ? description : ""
            } 
        });

    const handleOnSubmit = (data: QuizFormData) => {
        onSubmit(data.name, data.description);
    };
    

    return(
        <div className="flash-card-quiz-popup-content" onClick={(e) => e.stopPropagation()}>
            <form onSubmit={handleSubmit(handleOnSubmit)}>
                <h1>Create quiz</h1>
                <div className="input-name">
                    <label>Name</label>
                    <input
                        type="text"
                        placeholder="Enter a name..."
                        {...register("name", {
                            required: {value: true, message: "Name is required"}, 
                            maxLength: {value: 60, message: "Name too long"}
                        })} />
                    {errors.name && <span className={`error ${errors.name ? "visible" : ""}`}>{errors.name.message}</span>}
                </div>
                <div className="input-description">
                    <label>Description</label>
                    <textarea
                        placeholder="Enter description or leave empty..."
                        {...register("description", {
                            maxLength: {value: 400, message: "Description too long"}
                        })}/>
                    {errors.description && <span className={`error ${errors.description ? "visible" : ""}`}>{errors.description.message}</span>}
                </div>
                <div className="flash-card-quiz-popup-buttons">
                    <button type="button" className="button" onClick={() => onCancel(false)}>Cancel</button>
                    <button 
                        type="submit" 
                        className={`button ${isUpdate ? `primary-button ${isDirty ? "active" : ""}` : ""}`}>{isUpdate ? "Save" : "Create"}</button>
                </div>
            </form>
        </div>
    )
}

export default FlashCardQuizForm;