import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Quiz } from "../types/quiz";
import * as QuizService from "./QuizService";
import { Question } from "../types/Question";
import { FillInTheBlank } from "../types/fillInTheBlank";
import { Matching } from "../types/matching";
import { Ranking } from "../types/ranking";
import { Sequence } from "../types/sequence";
import FillInTheBlankComponent from "./questions/FillInTheBlankComponent";
import { QuizAttempt } from "../types/quizAttempt";
import { FillInTheBlankAttempt } from "../types/fillInTheBlankAttempt";
import { MatchingAttempt } from "../types/matchingAttempt";
import { RankingAttempt } from "../types/rankingAttempt";
import { SequenceAttempt } from "../types/sequenceAttempt";


const QuizPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const quizId = Number(id);

    const [quiz, setQuiz] = useState<Quiz>()
    const [allQuestions, setAllQuestions] = useState<Question[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [quizAttempt, setQuizAttempt] = useState<QuizAttempt>();
    const [fibAttempts, setFibAttempts] = useState<FillInTheBlankAttempt[]>([]);
    const [matchingAttempts, setMatchingAttempts] = useState<MatchingAttempt[]>([]);
    const [rankingAttempts, setRankingAttempts] = useState<RankingAttempt[]>([]);
    const [sequenceAttempts, setSequenceAttempts] = useState<SequenceAttempt[]>([]);

    const fetchQuiz = async () => {
        setLoading(true);
        setError(null);

        try {
            const data = await QuizService.fetchQuiz(quizId);
            setQuiz(data);
            handleSetAllQuestions(
                data.fillInTheBlankQuestions, 
                data.matchingQuestions, 
                data.rankingQuestions,
                data.sequenceQuestions);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
            setError("Failed to fetch quiz");
        } finally {
            setLoading(false);
        }
    };

    const createQuizAttempt = async () => {
        const quizAttempt: QuizAttempt = {quizId: quizId};
        try {
            const data = await QuizService.createQuizAttempt(quizAttempt);
            setQuizAttempt(data);
            console.log(data);
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error);
        }
    };

    const handleSetAllQuestions = (
        fib: FillInTheBlank[], matching: Matching[], ranking: Ranking[], 
        sequence: Sequence[]
    ) => {
        fib.forEach(q => {
            const fibAttempt: FillInTheBlankAttempt = {fillInTheBlankId: q.fillInTheBlankId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: ""};
            setFibAttempts(prevAttempts => 
                [...prevAttempts, fibAttempt]
            );
        });

        matching.forEach(q => {
            const matchingAttempt: MatchingAttempt = {matchingId: q.matchingId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnser: ""};
            setMatchingAttempts(prevAttempts => 
                [...prevAttempts, matchingAttempt]
            );
        });

        ranking.forEach(q => {
            const rankingAttempt: RankingAttempt = {rankingId: q.rankingId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: ""};
            setRankingAttempts(prevAttempts =>
                [...prevAttempts, rankingAttempt]
            );
        });

        sequence.forEach(q => {
            const sequenceAttempt: SequenceAttempt = {sequenceId: q.sequenceId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: ""};
            setSequenceAttempts(prevAttempts =>
                [...prevAttempts, sequenceAttempt]
            );
        });

        const combined: Question[] = [
            ...fib.map(q => ({ ...q, questionType: "fillInTheBlank" as const })),
            ...matching.map(q => ({ ...q, questionType: "matching" as const })),
            ...ranking.map(q => ({ ...q, questionType: "ranking" as const })),
            ...sequence.map(q => ({ ...q, questionType: "sequence" as const }))
        ];

        combined.sort((a, b) => a.quizQuestionNum - b.quizQuestionNum);

        setAllQuestions(combined);
    };

    const handleAnswerFib = (fibId: number, newAnswer: string) => {
        setFibAttempts(prevAttempts => 
            prevAttempts.map(attempt => 
                attempt.fillInTheBlankId === fibId
                ? {...attempt, userAnswer: newAnswer}
                : attempt
            )
        );
    };

    const submitQuiz = () => {

    };

    useEffect(() => {
        createQuizAttempt();
        fetchQuiz();
    }, [])

    return(
        <>
            {loading ? (
                <p className="loading">Loading...</p>
            ) : error ? (
                <p className="fetch-error">{error}</p>
            ) : (
                <>
                <div className="quiz-page-container">
                    <h1>{quiz?.name} | attempt: {quizAttempt?.quizAttemptId}</h1>
                    {allQuestions.map(question => (
                        <>
                            {question.questionType === "fillInTheBlank" ? (
                                <FillInTheBlankComponent 
                                    key={question.fillInTheBlankId} 
                                    fillInTheBlankId={question.fillInTheBlankId!}
                                    quizQuestionNum={question.quizQuestionNum} 
                                    question={question.question} 
                                    userAnswer={(fibAttempts.find(attempt => attempt.fillInTheBlankId === question.fillInTheBlankId))?.userAnswer!}
                                    handleAnswer={handleAnswerFib} />
                            ) : (
                                <h3>Question {question.quizQuestionNum}</h3>
                            )}
                        </>
                    ))}
                </div>

                <button className="button primary-button active">Submit</button>
                </>
            )}
        </>
    )
}

export default QuizPage;