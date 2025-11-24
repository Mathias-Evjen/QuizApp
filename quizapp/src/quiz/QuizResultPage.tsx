import { useLocation, useNavigate, useParams } from "react-router-dom";
import { Question } from "../types/quiz/question";
import { FillInTheBlankAttempt } from "../types/attempts/fillInTheBlankAttempt";
import { TrueFalseAttempt } from "../types/attempts/trueFalseAttempt";
import { MultiplechoiceAttempt } from "../types/attempts/MultipleChoiceAttempt";
import { MatchingAttempt } from "../types/attempts/matchingAttempt";
import { RankingAttempt } from "../types/attempts/rankingAttempt";
import { SequenceAttempt } from "../types/attempts/sequenceAttempt";
import { useEffect, useState } from "react";
import { QuizAttempt } from "../types/attempts/quizAttempt";
import { QuestionAttempt } from "../types/attempts/questionAttempt";
import * as QuizService from "./services/QuizService";
import "./style/Quiz.css";
import { Quiz } from "../types/quiz/quiz";
import { FillInTheBlank } from "../types/quiz/fillInTheBlank";
import { Matching } from "../types/quiz/matching";
import { Ranking } from "../types/quiz/ranking";
import { MultipleChoice } from "../types/quiz/multipleChoice";
import { TrueFalse } from "../types/quiz/trueFalse";
import { Sequence } from "../types/quiz/sequence";


const QuizResultPage: React.FC = () => {
    const navigate = useNavigate();

    const { id, attemptId } = useParams<{ id: string, attemptId: string }>();
    const quizId = Number(id);
    const quizAttemptId = Number(attemptId);

    const [quiz, setQuiz] = useState<Quiz>();
    const [allQuestions, setAllQuestions] = useState<Question[]>([]);

    const [loadingQuiz, setLoadingQuiz] = useState<boolean>(false);
    const [quizError, setQuizError] = useState<string | null>(null);


    const [quizAttempt, setQuizAttempt] = useState<QuizAttempt>();
    const [allQuestionAttempts, setAllQuestionAttempts] = useState<QuestionAttempt[]>([]);
    const [fibAttempts, setFibAttempts] = useState<FillInTheBlankAttempt[]>([]);
    const [matchingAttempts, setMatchingAttempts] = useState<MatchingAttempt[]>([]);
    const [rankingAttempts, setRankingAttempts] = useState<RankingAttempt[]>([]);
    const [sequenceAttempts, setSequenceAttempts] = useState<SequenceAttempt[]>([]);
    const [trueFalseAttempts, setTrueFalseAttempts] = useState<TrueFalseAttempt[]>([]);
    const [multipleChoiceAttempts, setMultipleChoiceAttempts] = useState<MultiplechoiceAttempt[]>([]);

    const [loadingQuizAttempt, setLoadingQuizAttempt] = useState<boolean>(false);
    const [quizAttemptError, setQuizAttemptError] = useState<string | null>(null);
    
    // -----------------------
    //     CRUD Operations 
    // -----------------------

    const fetchQuiz = async () => {
        setLoadingQuiz(true);
        setQuizError(null);

        try {
            const data = await QuizService.fetchQuiz(quizId);
            setQuiz(data);
            handleSetAllQuestions(
                data.fillInTheBlankQuestions,
                data.matchingQuestions,
                data.rankingQuestions,
                data.sequenceQuestions,
                data.multipleChoiceQuestions,
                data.trueFalseQuestions);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setQuizError("Failed to fetch quiz");
        } finally {
            setLoadingQuiz(false);
        }
    };
    
    const fetchQuizAttempt = async () => {
        setLoadingQuizAttempt(true);
        setQuizAttemptError(null);

        try {
            const data = await QuizService.fetchQuizAttempt(quizAttemptId);
            setQuizAttempt(data);
            console.log(data);
            handleSetAllQuestionAttempts(
                data.fillInTheBlankAttempts,
                data.matchingAttempts,
                data.rankingAttempts,
                data.sequenceAttempts,
                data.multipleChoiceAttempts,
                data.trueFalseAttempts);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setQuizAttemptError("Failed to fetch quiz attempt");
        } finally {
            setLoadingQuizAttempt(false);
        }
    };

    const handleSetAllQuestions = (
        fib: FillInTheBlank[], matching: Matching[], ranking: Ranking[],
        sequence: Sequence[], multipleChoice: MultipleChoice[], trueFalse: TrueFalse[]
    ) => {

        const combined: Question[] = [
            ...fib.map(q => ({ ...q, questionType: "fillInTheBlank" as const })),
            ...matching.map(q => ({ ...q, questionType: "matching" as const })),
            ...ranking.map(q => ({ ...q, questionType: "ranking" as const })),
            ...sequence.map(q => ({ ...q, questionType: "sequence" as const })),
            ...multipleChoice.map(q => ({ ...q, questionType: "multipleChoice" as const })),
            ...trueFalse.map(q => ({ ...q, questionType: "trueFalse" as const }))
        ];

        combined.sort((a, b) => a.quizQuestionNum - b.quizQuestionNum);

        setAllQuestions(combined);
    };

    const handleSetAllQuestionAttempts = (
        fib: FillInTheBlankAttempt[], matching: MatchingAttempt[], ranking: RankingAttempt[],
        sequence: SequenceAttempt[], multipleChoice: MultiplechoiceAttempt[], trueFalse: TrueFalseAttempt[]
    ) => {    
        const combined: QuestionAttempt[] = [
            ...fib.map(q => ({ ...q, questionType: "fillInTheBlank" as const })),
            ...matching.map(q => ({ ...q, questionType: "matching" as const })),
            ...ranking.map(q => ({ ...q, questionType: "ranking" as const })),
            ...sequence.map(q => ({ ...q, questionType: "sequence" as const })),
            ...multipleChoice.map(q => ({ ...q, questionType: "multipleChoice" as const })),
            ...trueFalse.map(q => ({ ...q, questionType: "trueFalse" as const }))
        ];

        combined.sort((a, b) => a.quizQuestionNum - b.quizQuestionNum);

        setAllQuestionAttempts(combined);
    };


    const getUserAnswer = (q: Question) => {
        if (q.questionType === "fillInTheBlank") {
            return fibAttempts.find((a: FillInTheBlankAttempt) => a.fillInTheBlankId === q.fillInTheBlankId)?.userAnswer;
        }

        if (q.questionType === "trueFalse") {
            return trueFalseAttempts.find((a: TrueFalseAttempt) => a.trueFalseId === q.trueFalseId)?.userAnswer;
        }

        if (q.questionType === "multipleChoice") {
            return multipleChoiceAttempts.find((a: MultiplechoiceAttempt) => a.multipleChoiceId === q.multipleChoiceId)?.userAnswer;
        }

        if (q.questionType === "matching") {
            return matchingAttempts.find((a: MatchingAttempt) => a.matchingId === q.matchingId)?.userAnswer;
        }

        if (q.questionType === "sequence") {
            return sequenceAttempts.find((a: SequenceAttempt) => a.sequenceId === q.sequenceId)?.userAnswer;
        }

        if (q.questionType === "ranking") {
            return rankingAttempts.find((a: RankingAttempt) => a.rankingId === q.rankingId)?.userAnswer;
        }

        return null;
    };

    const getCorrectAnswerText = (q: Question): string => {
        if (q.questionType === "multipleChoice" && "options" in q) {
            const correctOptions = q.options
                .filter((o: any) => o.isCorrect)
                .map((o: any) => o.text);

            return correctOptions.join(", ");
        }

        if (q.correctAnswer === undefined || q.correctAnswer === null) {
            return "";
        }

        return String(q.correctAnswer);
    };

    const formatUserAnswer = (answer: any): string => {
        if (Array.isArray(answer)) {
            return answer.join(", ");
        }

        if (typeof answer === "boolean") {
            return answer ? "True" : "False";
        }

        if (answer === null || answer === undefined) {
            return "";
        }

        return String(answer);
    };

    const isCorrect = (q: Question, userAnswer: any): boolean => {
        if (userAnswer === null || userAnswer === undefined) return false;

        if (q.questionType === "fillInTheBlank") {
            if (!q.correctAnswer || typeof userAnswer !== "string") return false;
            return userAnswer.trim().toLowerCase() === q.correctAnswer.trim().toLowerCase();
        }

        if (q.questionType === "trueFalse") {
            if (typeof userAnswer !== "boolean") return false;
            return userAnswer === q.correctAnswer;
        }

        if (q.questionType === "multipleChoice" && "options" in q) {
            if (!Array.isArray(userAnswer)) return false;

            const correctAnswers = q.options
                .filter((o: any) => o.isCorrect)
                .map((o: any) => o.text)
                .sort();

            const userAnswers = [...userAnswer].sort();

            if (correctAnswers.length === 0) return false;
            if (correctAnswers.length !== userAnswers.length) return false;

            return correctAnswers.every((t, i) => t === userAnswers[i]);
        }

        if (
            q.questionType === "matching" ||
            q.questionType === "sequence" ||
            q.questionType === "ranking"
        ) {
            if (!q.correctAnswer || typeof userAnswer !== "string") return false;
            return userAnswer.trim() === q.correctAnswer.trim();
        }

        return false;
    };

    useEffect(() => {
        fetchQuiz();
        fetchQuizAttempt();
    }, []);

    return (
        <div className="quiz-result-page">
            {loadingQuiz || loadingQuizAttempt ? (
                <p>Loading</p>
            ) : quizError || quizAttemptError ? (
                <>
                    <p>{quizError}</p>
                    <p>{quizAttemptError}</p>
                </>
            ) : (
                <>
                    <h1>{quiz?.name} — Results</h1>
                    <h2 className="quiz-result-score">
                        {quizAttempt?.numOfCorrectAnswers} / {allQuestionAttempts.length} Correct
                    </h2>

                    <div className="quiz-result-question-list">
                        {allQuestions.map((q: Question) => {
                            const attempt = allQuestionAttempts.find(attempt => attempt.quizQuestionNum === q.quizQuestionNum)

                            return (
                                <div
                                    key={`${q.questionType}-${q.quizQuestionNum}`}
                                    className={`quiz-result-question-box ${attempt?.answeredCorrectly ? "correct" : "incorrect"}`}
                                >
                                    <h3>Q{q.quizQuestionNum}: {q.question}</h3>

                                    <p>
                                        <strong>Your answer:</strong>{" "}
                                        <span className="quiz-result-answer">{formatUserAnswer(attempt?.userAnswer)}</span>
                                    </p>

                                    {!attempt?.answeredCorrectly && (
                                        <p className="quiz-result-correct-answer">
                                            <strong>Correct answer:</strong>{" "}
                                            {getCorrectAnswerText(q)}
                                        </p>
                                    )}
                                </div>
                            );
                        })}
                    </div>

                    <button className="quiz-result-btn-back" onClick={() => navigate("/quiz")}>
                        Back to Quizzes
                    </button>
                </>
            )}
            
        </div>
    );
};

export default QuizResultPage;