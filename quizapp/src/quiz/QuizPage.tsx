import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Quiz } from "../types/quiz";
import { Question } from "../types/Question";
import { FillInTheBlank } from "../types/fillInTheBlank";
import { Matching } from "../types/matching";
import { Ranking } from "../types/ranking";
import { Sequence } from "../types/sequence";
import { QuizAttempt } from "../types/quizAttempt";
import { FillInTheBlankAttempt } from "../types/fillInTheBlankAttempt";
import { MatchingAttempt } from "../types/matchingAttempt";
import { RankingAttempt } from "../types/rankingAttempt";
import { SequenceAttempt } from "../types/sequenceAttempt";
import { MultipleChoice } from "../types/multipleChoice";
import { TrueFalse } from "../types/trueFalse";
import { TrueFalseAttempt } from "../types/trueFalseAttempt";
import { MultiplechoiceAttempt } from "../types/MultipleChoiceAttempt";
import * as QuizService from "./services/QuizService";
import * as FillIntheBlankService from "./services/FillInTheBlankService";
import * as TrueFalseService from "./services/TrueFalseService";
import * as MultipleChoiceService from "./services/MultipleChoiceService";
import * as MatchingService from "./services/MatchingService";
import * as SequenceService from "./services/SequenceService";
import * as RankingService from "./services/RankingService";
import FillInTheBlankComponent from "./questions/FillInTheBlankComponent";
import TrueFalseComponent from "./questions/TrueFalseComponent";
import MultipleChoiceComponent from "./questions/MultipleChoiceComponent";
import MatchingComponent from "./questions/MatchingComponent";
import SequenceComponent from "./questions/SequenceComponent";
import RankingComponent from "./questions/RankingComponent";
import { useNavigate } from "react-router-dom";



const QuizPage: React.FC = () => {
    const navigate = useNavigate();
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

    const [isDirty, setIsDirty] = useState<boolean>(false);

    // -----------------------
    //     CRUD Operations 
    // -----------------------

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

    const createQuizAttempt = async () => {
        const quizAttempt: QuizAttempt = { quizId: quizId };
        try {
            const data = await QuizService.createQuizAttempt(quizAttempt);
            setQuizAttempt(data);
            console.log(data);
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error);
        }
    };

    const submitFibAttempt = async (fibAttempt: FillInTheBlankAttempt) => {
        fibAttempt.quizAttemptId = quizAttempt?.quizAttemptId!;
        try {
            const data = await FillIntheBlankService.submitQuestion(fibAttempt);
            console.log(`Question ${fibAttempt.quizQuestionNum} submitted successfully: `, data);
        } catch (error) {
            console.error(`There was an error when submitting question ${fibAttempt.quizQuestionNum}: `, error);
        }
    };

    const submitTrueFalseAttempt = async (trueFalseAttempt: TrueFalseAttempt) => {
        trueFalseAttempt.quizAttemptId = quizAttempt?.quizAttemptId!;
        try {
            const data = await TrueFalseService.submitQuestion(trueFalseAttempt);
            console.log(`Question ${trueFalseAttempt.quizQuestionNum} submitted successfully: `, data);
        } catch (error) {
            console.error(`There was an error when submitting question ${trueFalseAttempt.quizQuestionNum}: `, error);
        }
    };

    const submitMultipleChoiceAttempt = async (multipleChoiceAttempt: MultiplechoiceAttempt) => {
        multipleChoiceAttempt.quizAttemptId = quizAttempt?.quizAttemptId!;
        try {
            const data = await MultipleChoiceService.submitQuestion(multipleChoiceAttempt);
            console.log(`Question ${multipleChoiceAttempt.quizQuestionNum} submitted successfully: `, data);
        } catch (error) {
            console.error(`There was an error when submitting question ${multipleChoiceAttempt.quizQuestionNum}: `, error);
        }
    };

    const submitMatchingAttempt = async (matchingAttempt: MatchingAttempt) => {
        matchingAttempt.quizAttemptId = quizAttempt?.quizAttemptId!;
        try {
            const data = await MatchingService.submitQuestion(matchingAttempt);
            console.log(`Question ${matchingAttempt.quizQuestionNum} submitted successfully: `, data);
        } catch (error) {
            console.error(`There was an error when submitting question ${matchingAttempt.quizQuestionNum}: `, error);
        }
    };

    const submitSequenceAttempt = async (sequenceAttempt: SequenceAttempt) => {
        sequenceAttempt.quizAttemptId = quizAttempt?.quizAttemptId!;
        try {
            const data = await SequenceService.submitQuestion(sequenceAttempt);
            console.log(`Question ${sequenceAttempt.quizQuestionNum} submitted successfully: `, data);
        } catch (error) {
            console.error(`There was an error when submitting question ${sequenceAttempt.quizQuestionNum}: `, error);
        }
    };

    const submitRankingAttempt = async (rankingAttempt: RankingAttempt) => {
        rankingAttempt.quizAttemptId = quizAttempt?.quizAttemptId!;
        try {
            const data = await RankingService.submitQuestion(rankingAttempt);
            console.log(`Question ${rankingAttempt.quizQuestionNum} submitted successfully: `, data);
        } catch (error) {
            console.error(`There was an error when submitting question ${rankingAttempt.quizQuestionNum}: `, error);
        }
    };

    // ------------------------------------------
    //     Setting questions and user answers
    // ------------------------------------------

    const handleSetAllQuestions = (
        fib: FillInTheBlank[], matching: Matching[], ranking: Ranking[],
        sequence: Sequence[], multipleChoice: MultipleChoice[], trueFalse: TrueFalse[]
    ) => {
        fib.forEach(q => {
            const fibAttempt: FillInTheBlankAttempt = { fillInTheBlankId: q.fillInTheBlankId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
            setFibAttempts(prevAttempts =>
                [...prevAttempts, fibAttempt]
            );
        });

        matching.forEach(q => {
            const matchingAttempt: MatchingAttempt = { matchingId: q.matchingId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
            setMatchingAttempts(prevAttempts =>
                [...prevAttempts, matchingAttempt]
            );
        });

        ranking.forEach(q => {
            const rankingAttempt: RankingAttempt = { rankingId: q.rankingId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
            setRankingAttempts(prevAttempts =>
                [...prevAttempts, rankingAttempt]
            );
        });

        sequence.forEach(q => {
            const sequenceAttempt: SequenceAttempt = { sequenceId: q.sequenceId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
            setSequenceAttempts(prevAttempts =>
                [...prevAttempts, sequenceAttempt]
            );
        });

        trueFalse.forEach(q => {
            const trueFalseAttempt: TrueFalseAttempt = { trueFalseId: q.trueFalseId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: null };
            setTrueFalseAttempts(prevAttempts =>
                [...prevAttempts, trueFalseAttempt]
            );
        });

        multipleChoice.forEach(q => {
            const multipleChoiceAttempt: MultiplechoiceAttempt = { multipleChoiceId: q.multipleChoiceId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
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
                    ? { ...attempt, userAnswer: newAnswer }
                    : attempt
            )
        );
        setIsDirty(true);
    };

    const handleAnswerTrueFalse = (trueFalseId: number, newAnswer: boolean) => {
        setTrueFalseAttempts(prevAttempts =>
            prevAttempts.map(attempt =>
                attempt.trueFalseId === trueFalseId
                    ? { ...attempt, userAnswer: newAnswer }
                    : attempt
            )
        );
        setIsDirty(true);
    };

    const handleAnswerMultipleChoice = (multipleChoiceId: number, newAnswer: string) => {
        setMultipleChoiceAttempts(prevAttempts =>
            prevAttempts.map(attempt =>
                attempt.multipleChoiceId === multipleChoiceId
                    ? { ...attempt, userAnswer: newAnswer }
                    : attempt
            )
        );
        setIsDirty(true);
    };

    const handleAnswerMatching = (matchingId: number, newAnswer: string) => {
        setMatchingAttempts(prevAttempts =>
            prevAttempts.map(attempt =>
                attempt.matchingId === matchingId
                    ? { ...attempt, userAnswer: newAnswer }
                    : attempt
            )
        );
        setIsDirty(true);
    };

    const handleAnswerSequence = (sequenceId: number, newAnswer: string) => {
        setSequenceAttempts(prevAttempts =>
            prevAttempts.map(attempt =>
                attempt.sequenceId === sequenceId
                    ? { ...attempt, userAnswer: newAnswer }
                    : attempt
            )
        );
        setIsDirty(true);
    };

    const handleAnswerRanking = (rankingId: number, newAnswer: string) => {
        setRankingAttempts(prevAttempts =>
            prevAttempts.map(attempt =>
                attempt.rankingId === rankingId
                    ? { ...attempt, userAnswer: newAnswer }
                    : attempt
            )
        );
        setIsDirty(true);
    };

    const submitQuiz = async () => {
        if (!quizAttempt) {
            console.error("Quizattempt missing");
            return;
        }

        const submitCalls: Promise<any>[] = [];

        fibAttempts.forEach(a => submitCalls.push(submitFibAttempt(a)));
        trueFalseAttempts.forEach(a => submitCalls.push(submitTrueFalseAttempt(a)));
        multipleChoiceAttempts.forEach(a => submitCalls.push(submitMultipleChoiceAttempt(a)));
        matchingAttempts.forEach(a => submitCalls.push(submitMatchingAttempt(a)));
        sequenceAttempts.forEach(a => submitCalls.push(submitSequenceAttempt(a)));
        rankingAttempts.forEach(a => submitCalls.push(submitRankingAttempt(a)));

        await Promise.all(submitCalls);

        let score = 0;
        let total = allQuestions.length;

        allQuestions.forEach(q => {
            if (q.questionType === "fillInTheBlank" && q.correctAnswer) {
                const att = fibAttempts.find(a => a.fillInTheBlankId === q.fillInTheBlankId);
                if (att?.userAnswer?.trim().toLowerCase() === q.correctAnswer.trim().toLowerCase()) {
                    score++;
                }
            }

            if (q.questionType === "trueFalse") {
                const att = trueFalseAttempts.find(a => a.trueFalseId === q.trueFalseId);
                if (att?.userAnswer === q.correctAnswer) {
                    score++;
                }
            }

            if (q.questionType === "multipleChoice") {
                const att = multipleChoiceAttempts.find(a => a.multipleChoiceId === q.multipleChoiceId);
                if (att?.userAnswer === q.correctAnswer) {
                    score++;
                }
            }

            if (q.questionType === "matching") {
                const att = matchingAttempts.find(a => a.matchingId === q.matchingId);
                if (att?.userAnswer === q.correctAnswer) {
                    score++;
                }
            }

            if (q.questionType === "sequence") {
                const att = sequenceAttempts.find(a => a.sequenceId === q.sequenceId);
                if (att?.userAnswer === q.correctAnswer) {
                    score++;
                }
            }

            if (q.questionType === "ranking") {
                const att = rankingAttempts.find(a => a.rankingId === q.rankingId);
                if (att?.userAnswer === q.correctAnswer) {
                    score++;
                }
            }
        });

        navigate('/quiz/${quizId}/result', {
            state: {
                score,
                total
            }
        });
    }





        //        fibAttempts.forEach(attempt =>
        //            submitFibAttempt(attempt)
        //        );
        //
        //        trueFalseAttempts.forEach(attempt =>
        //            submitTrueFalseAttempt(attempt)
        //        );
        //
        //        multipleChoiceAttempts.forEach(attempt =>
        //            submitMultipleChoiceAttempt(attempt)
        //        );
        //
        //        matchingAttempts.forEach(attempt =>
        //            submitMatchingAttempt(attempt)
        //        );
        //
        //        sequenceAttempts.forEach(attempt =>
        //            submitSequenceAttempt(attempt)
        //        );
        //
        //        rankingAttempts.forEach(attempt => 
        //            submitRankingAttempt(attempt)
        //        );
        //    };

        useEffect(() => {
            createQuizAttempt();
            fetchQuiz();
        }, [])

        return (
            <>
                {loading ? (
                    <p className="loading">Loading...</p>
                ) : error ? (
                    <p className="fetch-error">{error}</p>
                ) : (
                    <>
                        <div className="quiz-page-container">
                            <h1>{quiz?.name}</h1>
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
                                    ) : question.questionType === "matching" ? (
                                        <MatchingComponent
                                            key={question.matchingId}
                                            matchingId={question.matchingId!}
                                            quizQuestionNum={question.quizQuestionNum}
                                            question={question.question}
                                            questionText={question.questionText}
                                            userAnswer={(matchingAttempts.find(attempt => attempt.matchingId === question.matchingId))?.userAnswer}
                                            handleAnswer={handleAnswerMatching} />
                                    ) : question.questionType === "sequence" ? (
                                        <SequenceComponent
                                            key={question.sequenceId}
                                            sequenceId={question.sequenceId!}
                                            quizQuestionNum={question.quizQuestionNum}
                                            question={question.question}
                                            questionText={question.questionText}
                                            userAnswer={(sequenceAttempts.find(attempt => attempt.sequenceId === question.sequenceId))?.userAnswer}
                                            handleAnswer={handleAnswerSequence} />
                                    ) : (
                                        <RankingComponent
                                            key={question.rankingId}
                                            rankingId={question.rankingId!}
                                            quizQuestionNum={question.quizQuestionNum}
                                            question={question.question}
                                            questionText={question.questionText}
                                            userAnswer={(rankingAttempts.find(attempt => attempt.rankingId === question.rankingId))?.userAnswer}
                                            handleAnswer={handleAnswerRanking} />
                                    )}
                                </>
                            ))}
                            <button className="button primary-button active" onClick={submitQuiz}>Submit</button>
                        </div>
                    </>
                )}
            </>
        )
    }

    export default QuizPage;