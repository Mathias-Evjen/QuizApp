import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Quiz } from "../types/quiz";
import * as QuizService from "./QuizService";
import * as FillIntheBlankService from "./services/FillInTheBlankService";
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
import { MultipleChoice } from "../types/multipleChoice";
import { TrueFalse } from "../types/trueFalse";
import { TrueFalseAttempt } from "../types/trueFalseAttempt";
import TrueFalseComponent from "./questions/TrueFalseComponent";
import { MultiplechoiceAttempt } from "../types/MultipleChoiceAttempt";
import MultipleChoiceComponent from "./questions/MultipleChoiceComponent";


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
    const [trueFalseAttempts, setTrueFalseAttempts] = useState<TrueFalseAttempt[]>([]);
    const [multipleChoiceAttempts, setMultipleChoiceAttempts] = useState<MultiplechoiceAttempt[]>([]);

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
            setError("Failed to fetch quiz");
        } finally {
            setLoading(false);
        }
    };

    const submitFibAttempt = async (fibAttempt: FillInTheBlankAttempt) => {
        fibAttempt.quizAttemptId = quizAttempt?.quizAttemptId!
        try {
            const data = await FillIntheBlankService.submitQuestion(fibAttempt);
            console.log(`Question ${fibAttempt.quizQuestionNum} submitted`);
        } catch (error) {
            console.error(`There was an error when submitting question ${fibAttempt.quizQuestionNum}: `, error);
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
        sequence: Sequence[], multipleChoice: MultipleChoice[], trueFalse: TrueFalse[]
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

        trueFalse.forEach(q => {
            const trueFalseAttempt: TrueFalseAttempt = {trueFalseId: q.trueFalseId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: null};
            setTrueFalseAttempts(prevAttempts =>
                [...prevAttempts, trueFalseAttempt]
            );
        });

        multipleChoice.forEach(q => {
            const multipleChoiceAttempt: MultiplechoiceAttempt = {multipleChoiceId: q.multipleChoiceId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: ""};
            setMultipleChoiceAttempts(prevAttempts =>
                [...prevAttempts, multipleChoiceAttempt]
            );
        });

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

    const handleAnswerFib = (fibId: number, newAnswer: string) => {
        setFibAttempts(prevAttempts => 
            prevAttempts.map(attempt => 
                attempt.fillInTheBlankId === fibId
                ? {...attempt, userAnswer: newAnswer}
                : attempt
            )
        );
    };

    const handleAnswerTrueFalse = (trueFalseId: number, newAnswer: boolean) => {
        setTrueFalseAttempts(prevAttempts =>
            prevAttempts.map(attempt =>
                attempt.trueFalseId === trueFalseId
                ? {...attempt, userAnswer: newAnswer}
                : attempt
            )
        );
    };

    const handleAnswerMultipleChoice = (multipleChoiceId: number, newAnswer: string) => {
        setMultipleChoiceAttempts(prevAttempts =>
            prevAttempts.map(attempt =>
                attempt.multipleChoiceId === multipleChoiceId
                ? {...attempt, userAnswer: newAnswer}
                : attempt
            )
        );
    };

    const submitQuiz = async () => {
        fibAttempts.forEach(fibAttempt =>
            submitFibAttempt(fibAttempt)
        );
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
                            ) : question.questionType === "trueFalse" ? (
                                <TrueFalseComponent
                                    key={question.trueFalseId}
                                    trueFalseId={question.trueFalseId!}
                                    quizQuestionNum={question.quizQuestionNum}
                                    question={question.question}
                                    userAnswer={(trueFalseAttempts.find(attempt => attempt.trueFalseId === question.trueFalseId))?.userAnswer}
                                    handleAnswer={handleAnswerTrueFalse} />
                            ) : question.questionType === "multipleChoice" ? (
                                <MultipleChoiceComponent
                                    key={question.multipleChoiceId}
                                    multipleChoiceId={question.multipleChoiceId!}
                                    quizQuestionNum={question.quizQuestionNum}
                                    question={question.question}
                                    userAnswer={(multipleChoiceAttempts.find(attempt => attempt.multipleChoiceId === question.multipleChoiceId))?.userAnswer}
                                    options={question.options}
                                    handleAnswer={(handleAnswerMultipleChoice)} />
                            ) : (
                                <h3>Question {question.quizQuestionNum}</h3>
                            )}
                        </>
                    ))}
                </div>

                <button className="button primary-button active" onClick={submitQuiz}>Submit</button>
                </>
            )}
        </>
    )
}

export default QuizPage;