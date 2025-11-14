import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { TrueFalse } from "../types/trueFalse";
import * as TrueFalseService from "../services/TrueFalseService";
import "./ManageTrueFalse.css";

const ManageTrueFalseQuizPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [questions, setQuestions] = useState<TrueFalse[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [questionErrors, setQuestionErrors] = useState<{ [key: number]: string }>({});

    const handleFetchQuestions = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await TrueFalseService.fetchTrueFalseQuestions(quizId);
            setQuestions(data);
        } catch {
            setError("Failed to fetch True/False questions.");
        } finally {
            setLoading(false);
        }
    };

    const handleAddQuestion = () => {
        const newQuestion: TrueFalse = {
            question: "",
            correctAnswer: false,
            quizId,
            quizQuestionNum: questions.length + 1,
            isNew: true,
            tempId: Date.now() + Math.random(),
        };
        setQuestions((prev) => [...prev, newQuestion]);
    };

    const handleQuestionTextChanged = (id: number, text: string) => {
        setQuestions((prev) =>
            prev.map((q) =>
                (q.trueFalseId ?? q.tempId) === id ? { ...q, question: text, isDirty: true } : q
            )
        );
    };

    const handleAnswerChanged = (id: number, value: boolean) => {
        setQuestions((prev) =>
            prev.map((q) =>
                (q.trueFalseId ?? q.tempId) === id
                    ? { ...q, correctAnswer: value, isDirty: true }
                    : q
            )
        );
    };

    const handleSave = async () => {
        const errors: { [key: number]: string } = {};

        questions.forEach((q) => {
            if (!q.question.trim()) errors[q.trueFalseId ?? q.tempId!] = "Question text is required.";
        });

        if (Object.keys(errors).length > 0) {
            setQuestionErrors(errors);
            return;
        }

        const newOnes = questions.filter((q) => q.isNew);
        const edited = questions.filter((q) => q.isDirty && !q.isNew);

        await Promise.all([
            ...newOnes.map((q) => TrueFalseService.createTrueFalse(q)),
            ...edited.map((q) => TrueFalseService.updateTrueFalse(q.trueFalseId!, q)),
        ]);

        setQuestionErrors({});
        handleFetchQuestions();
    };

    const handleDeleteQuestion = async (id: number) => {
        await TrueFalseService.deleteTrueFalse(id, 0, quizId);
        handleFetchQuestions();
    };

    useEffect(() => {
        handleFetchQuestions();
    }, []);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <div className="manage-tf-container">
            <h1>Manage True/False Quiz</h1>

            <button className="add-button" onClick={handleAddQuestion}>
                Add Question
            </button>

            {questions.map((q) => (
                <div key={q.trueFalseId ?? q.tempId} className="tf-entry">
                    <input
                        value={q.question}
                        onChange={(e) => handleQuestionTextChanged(q.trueFalseId ?? q.tempId!, e.target.value)}
                        placeholder="Question text"
                        className={questionErrors[q.trueFalseId ?? q.tempId!] ? "input-error" : ""}
                    />

                    <div className="tf-answer">
                        <label>
                            <input
                                type="radio"
                                name={`answer_${q.trueFalseId ?? q.tempId}`}
                                checked={q.correctAnswer === true}
                                onChange={() => handleAnswerChanged(q.trueFalseId ?? q.tempId!, true)}
                            />
                            True
                        </label>

                        <label>
                            <input
                                type="radio"
                                name={`answer_${q.trueFalseId ?? q.tempId}`}
                                checked={q.correctAnswer === false}
                                onChange={() => handleAnswerChanged(q.trueFalseId ?? q.tempId!, false)}
                            />
                            False
                        </label>
                    </div>

                    <button className="delete-button" onClick={() => handleDeleteQuestion(q.trueFalseId!)}>
                        Delete
                    </button>

                    {questionErrors[q.trueFalseId ?? q.tempId!] && (
                        <p className="error">{questionErrors[q.trueFalseId ?? q.tempId!]}</p>
                    )}
                </div>
            ))}

            <button className="save-button" onClick={handleSave}>
                Save
            </button>
        </div>
    );
};

export default ManageTrueFalseQuizPage;
