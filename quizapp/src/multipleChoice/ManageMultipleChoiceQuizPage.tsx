import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import { MultipleChoice, Option } from "../types/multipleChoice";
import * as MultipleChoiceService from "../services/MultipleChoiceService";
import "../multipleChoice/MultipleChoice.css";

const ManageMultipleChoiceQuizPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [questions, setQuestions] = useState<MultipleChoice[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [questionErrors, setQuestionErrors] = useState<{
        [key: number]: { question?: string; options?: string };
    }>({});

    const fetchQuestions = async () => {
        setLoading(true);
        setError(null);

        try {
            const data = await MultipleChoiceService.fetchMultipleChoiceQuestions(quizId);
            setQuestions(data);
        } catch (error) {
            console.error(error);
            setError("Failed to fetch questions");
        } finally {
            setLoading(false);
        }
    };

    const validateQuestion = (q: MultipleChoice) => {
        const errors: { question?: string; options?: string } = {};

        if (!q.question || q.question.trim() === "")
            errors.question = "Question is required.";

        if (!q.options.some(o => o.isCorrect))
            errors.options = "At least one option must be correct.";

        return errors;
    };

    const handleQuestionTextChange = (id: number, text: string) => {
        setQuestions(prev =>
            prev.map(q =>
                (q.multipleChoiceId ?? q.tempId) === id
                    ? { ...q, question: text, isDirty: true }
                    : q
            )
        );
    };

    const handleOptionChange = (qId: number, index: number, field: "text" | "isCorrect", value: string | boolean) => {
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

    const handleAddQuestion = () => {
        const newQuestion: MultipleChoice = {
            question: "",
            quizId,
            quizQuestionNum: questions.length + 1,
            options: [
                { text: "", isCorrect: false },
                { text: "", isCorrect: false },
                { text: "", isCorrect: false },
                { text: "", isCorrect: false },
            ],
            isNew: true,
            tempId: Date.now() + Math.random()
        };

        setQuestions(prev => [...prev, newQuestion]);
    };

    const handleSave = async () => {
        const allErrors: typeof questionErrors = {};
        questions.forEach(q => {
            const errs = validateQuestion(q);
            if (Object.keys(errs).length > 0)
                allErrors[q.multipleChoiceId ?? q.tempId!] = errs;
        });

        if (Object.keys(allErrors).length > 0) {
            setQuestionErrors(allErrors);
            return;
        }

        const dirty = questions.filter(q => q.isDirty);
        const newlyCreated = questions.filter(q => q.isNew);

        await Promise.all([
            ...dirty.map(q => MultipleChoiceService.updateMultipleChoice(q.multipleChoiceId!, q)),
            ...newlyCreated.map(q => MultipleChoiceService.createMultipleChoice(q))
        ]);

        setQuestionErrors({});
        fetchQuestions();
    };

    const handleDelete = async (id: number) => {
        await MultipleChoiceService.deleteMultipleChoice(id, 0, quizId);
        fetchQuestions();
    };

    useEffect(() => {
        fetchQuestions();
    }, []);

    if (loading) return <p>Loading questions...</p>;

    return (
        <div className="manage-mc-container">
            <h1>Manage Multiple Choice Questions</h1>

            <button className="add-button" onClick={handleAddQuestion}>Add Question</button>

            {questions.length === 0 && <p>No questions yet. Add one!</p>}

            {questions.map(q => (
                <div className="mc-entry" key={q.multipleChoiceId ?? q.tempId}>
                    <input
                        value={q.question}
                        onChange={e => handleQuestionTextChange(q.multipleChoiceId ?? q.tempId!, e.target.value)}
                        className={questionErrors[q.multipleChoiceId ?? q.tempId!]?.question ? "input-error" : ""}
                        placeholder="Question text"
                    />

                    {q.options.map((opt, i) => (
                        <div key={i} className="mc-option">
                            <input
                                value={opt.text}
                                onChange={e => handleOptionChange(q.multipleChoiceId ?? q.tempId!, i, "text", e.target.value)}
                                placeholder={`Option ${i + 1}`}
                            />
                            <input
                                type="checkbox"
                                checked={opt.isCorrect}
                                onChange={e => handleOptionChange(q.multipleChoiceId ?? q.tempId!, i, "isCorrect", e.target.checked)}
                            />
                        </div>
                    ))}

                    <button onClick={() => handleDelete(q.multipleChoiceId!)} className="delete-button">Delete</button>

                    {questionErrors[q.multipleChoiceId ?? q.tempId!]?.options && (
                        <p className="error">{questionErrors[q.multipleChoiceId ?? q.tempId!].options}</p>
                    )}
                </div>
            ))}

            <button className="save-button" onClick={handleSave}>Save</button>
        </div>
    );
};

export default ManageMultipleChoiceQuizPage;
