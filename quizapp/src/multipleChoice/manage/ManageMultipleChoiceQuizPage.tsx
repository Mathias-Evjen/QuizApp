import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import { MultipleChoice } from "../../types/multipleChoice";
import * as MultipleChoiceService from "../../services/MultipleChoiceService";
import "./ManageMultipleChoice.css";

const ManageMultipleChoiceQuizPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [questions, setQuestions] = useState<MultipleChoice[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const [questionErrors, setQuestionErrors] = useState<{
        [key: number]: { question?: string; options?: string };
    }>({});

    const handleFetchQuestions = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await MultipleChoiceService.fetchMultipleChoiceQuestions(quizId);
            setQuestions(data);
        } catch {
            setError("Failed to fetch questions.");
        } finally {
            setLoading(false);
        }
    };

    const handleAddQuestion = () => {
        const newQuestion: MultipleChoice = {
            question: "",
            quizId,
            quizQuestionNum: questions.length + 1,
            options: [
                { text: "", isCorrect: false },
                { text: "", isCorrect: false },
                { text: "", isCorrect: false },
                { text: "", isCorrect: false }
            ],
            isNew: true,
            tempId: Date.now() + Math.random()
        };

        setQuestions(prev => [...prev, newQuestion]);
    };

    const handleQuestionTextChanged = (id: number, text: string) => {
        setQuestions(prev =>
            prev.map(q =>
                (q.multipleChoiceId ?? q.tempId) === id
                    ? { ...q, question: text, isDirty: true }
                    : q
            )
        );
    };

    const handleOptionChanged = (
        qId: number,
        index: number,
        field: "text" | "isCorrect",
        value: string | boolean
    ) => {
        setQuestions(prev =>
            prev.map(q =>
                (q.multipleChoiceId ?? q.tempId) === qId
                    ? {
                        ...q,
                        options: q.options.map((o, i) =>
                            i === index ? { ...o, [field]: value } : o
                        ),
                        isDirty: true
                    }
                    : q
            )
        );
    };

    const handleAddOption = (qId: number) => {
        setQuestions(prev =>
            prev.map(q =>
                (q.multipleChoiceId ?? q.tempId) === qId
                    ? { ...q, options: [...q.options, { text: "", isCorrect: false }], isDirty: true }
                    : q
            )
        );
    };

    const handleDeleteOption = (qId: number, index: number) => {
        setQuestions(prev =>
            prev.map(q =>
                (q.multipleChoiceId ?? q.tempId) === qId
                    ? { ...q, options: q.options.filter((_, i) => i !== index), isDirty: true }
                    : q
            )
        );
    };

    const handleSave = async () => {
        const errors: typeof questionErrors = {};

        questions.forEach(q => {
            const qErrors: { question?: string; options?: string } = {};
            if (!q.question?.trim()) qErrors.question = "Question is required.";
            if (!q.options.some(o => o.isCorrect)) qErrors.options = "At least one option must be correct.";
            if (Object.keys(qErrors).length > 0) errors[q.multipleChoiceId ?? q.tempId!] = qErrors;
        });

        if (Object.keys(errors).length > 0) {
            setQuestionErrors(errors);
            return;
        }

        const newOnes = questions.filter(q => q.isNew);
        const edited = questions.filter(q => q.isDirty && !q.isNew);

        await Promise.all([
            ...newOnes.map(q => MultipleChoiceService.createMultipleChoice(q)),
            ...edited.map(q => MultipleChoiceService.updateMultipleChoice(q.multipleChoiceId!, q))
        ]);

        setQuestionErrors({});
        handleFetchQuestions();
    };

    const handleDeleteQuestion = async (id: number) => {
        await MultipleChoiceService.deleteMultipleChoice(id, 0, quizId);
        handleFetchQuestions();
    };

    useEffect(() => {
        handleFetchQuestions();
    }, []);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <div className="manage-mc-container">
            <h1>Manage Multiple Choice Quiz</h1>

            <button className="add-button" onClick={handleAddQuestion}>Add Question</button>

            {questions.map(q => (
                <div key={q.multipleChoiceId ?? q.tempId} className="mc-entry">

                    <input
                        value={q.question}
                        onChange={e => handleQuestionTextChanged(q.multipleChoiceId ?? q.tempId!, e.target.value)}
                        placeholder="Question text"
                        className={questionErrors[q.multipleChoiceId ?? q.tempId!]?.question ? "input-error" : ""}
                    />

                    {q.options.map((opt, i) => (
                        <div key={i} className="mc-option">
                            <input
                                value={opt.text}
                                onChange={e => handleOptionChanged(q.multipleChoiceId ?? q.tempId!, i, "text", e.target.value)}
                                placeholder={`Option ${i + 1}`}
                            />
                            <input
                                type="checkbox"
                                checked={opt.isCorrect}
                                onChange={e => handleOptionChanged(q.multipleChoiceId ?? q.tempId!, i, "isCorrect", e.target.checked)}
                            />
                            <button className="delete-option" onClick={() => handleDeleteOption(q.multipleChoiceId ?? q.tempId!, i)}>X</button>
                        </div>
                    ))}

                    {questionErrors[q.multipleChoiceId ?? q.tempId!]?.options && (
                        <p className="error">{questionErrors[q.multipleChoiceId ?? q.tempId!].options}</p>
                    )}

                    <button onClick={() => handleAddOption(q.multipleChoiceId ?? q.tempId!)}>Add Option</button>
                    <button className="delete-question" onClick={() => handleDeleteQuestion(q.multipleChoiceId!)}>Delete Question</button>
                </div>
            ))}

            <button className="save-button" onClick={handleSave}>Save</button>
        </div>
    );
};

export default ManageMultipleChoiceQuizPage;
