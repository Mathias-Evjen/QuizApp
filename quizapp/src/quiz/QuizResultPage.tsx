import { useLocation, useNavigate } from "react-router-dom";
import "./style/Quiz.css";

import { Question } from "../types/Question";
import { FillInTheBlankAttempt } from "../types/fillInTheBlankAttempt";
import { TrueFalseAttempt } from "../types/trueFalseAttempt";
import { MultiplechoiceAttempt } from "../types/MultipleChoiceAttempt";
import { MatchingAttempt } from "../types/matchingAttempt";
import { RankingAttempt } from "../types/rankingAttempt";
import { SequenceAttempt } from "../types/sequenceAttempt";


const QuizResultPage: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { score, total, quiz, allQuestions, attempts } = location.state || { score: 0, total: 0 };

    if (!quiz || !allQuestions || !attempts) {
        return <p>Error: Missing result data</p>;
    }

    const {
        fibAttempts,
        trueFalseAttempts,
        multipleChoiceAttempts,
        matchingAttempts,
        sequenceAttempts,
        rankingAttempts
    } = attempts;

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
            if (!q.correctAnswer || typeof userAnswer !== "string") return false;
            return userAnswer === q.correctAnswer;
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

    return (
        <div className="quiz-result-page">
            <h1>{quiz.name} — Results</h1>
            <h2 className="quiz-result-score">
                {score} / {total} Correct
            </h2>

            <div className="quiz-result-question-list">
                {allQuestions.map((q: Question) => {
                    const userAnswer = getUserAnswer(q);
                    const correct = isCorrect(q, userAnswer);

                    return (
                        <div
                            key={`${q.questionType}-${q.quizQuestionNum}`}
                            className={`quiz-result-question-box ${correct ? "correct" : "incorrect"}`}
                        >
                            <h3>Q{q.quizQuestionNum}: {q.question}</h3>

                            <p>
                                <strong>Your answer:</strong>{" "}
                                <span className="quiz-result-answer">{formatUserAnswer(userAnswer)}</span>
                            </p>

                            {!correct && (
                                <p className="quiz-result-correct-answer">
                                    <strong>Correct answer:</strong>{" "}
                                    {getCorrectAnswerText(q)}
                                </p>
                            )}
                        </div>
                    );
                })}
            </div>

            <button className="quiz-result-btn-back" onClick={() => navigate("/")}>
                Back to Quizzes
            </button>
        </div>
    );
};

export default QuizResultPage;