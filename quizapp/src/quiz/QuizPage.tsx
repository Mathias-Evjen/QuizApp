import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Quiz } from "../types/quiz/quiz";
import { Question } from "../types/quiz/question";
import { FillInTheBlank } from "../types/quiz/fillInTheBlank";
import { Matching } from "../types/quiz/matching";
import { Ranking } from "../types/quiz/ranking";
import { TrueFalse } from "../types/quiz/trueFalse";
import { MultipleChoice } from "../types/quiz/multipleChoice";
import { Sequence } from "../types/quiz/sequence";
import { QuizAttempt } from "../types/attempts/quizAttempt";
import { FillInTheBlankAttempt } from "../types/attempts/fillInTheBlankAttempt";
import { MatchingAttempt } from "../types/attempts/matchingAttempt";
import { RankingAttempt } from "../types/attempts/rankingAttempt";
import { SequenceAttempt } from "../types/attempts/sequenceAttempt";
import { useNavigate } from "react-router-dom";
import { TrueFalseAttempt } from "../types/attempts/trueFalseAttempt";
import { MultiplechoiceAttempt } from "../types/attempts/MultipleChoiceAttempt";
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

    const updateQuizAttemptScore = async (score: number) => {
        const updatedAttempt = {
            ...quizAttempt!,
            numOfCorrectAnswers: score
        };

        setQuizAttempt(updatedAttempt)
        
        try {
            const data = await QuizService.updateQuizScore(updatedAttempt.quizAttemptId!, updatedAttempt);
            console.log("Quiz attempt updated successfully:", data);
        } catch (error) {
            console.error("There was a problem with the fetch operation: ", error)
        }
    } 

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
        console.log(matchingAttempt)
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
            const fibAttempt: FillInTheBlankAttempt = { questionType: "fillInTheBlank", fillInTheBlankId: q.fillInTheBlankId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
            setFibAttempts(prevAttempts =>
                [...prevAttempts, fibAttempt]
            );
        });

        matching.forEach(q => {
            const matchingAttempt: MatchingAttempt = { questionType: "matching", matchingId: q.matchingId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
            setMatchingAttempts(prevAttempts =>
                [...prevAttempts, matchingAttempt]
            );
        });

        ranking.forEach(q => {
            const rankingAttempt: RankingAttempt = { questionType: "ranking", rankingId: q.rankingId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
            setRankingAttempts(prevAttempts =>
                [...prevAttempts, rankingAttempt]
            );
        });

        sequence.forEach(q => {
            const sequenceAttempt: SequenceAttempt = { questionType: "sequence", sequenceId: q.sequenceId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
            setSequenceAttempts(prevAttempts =>
                [...prevAttempts, sequenceAttempt]
            );
        });

        trueFalse.forEach(q => {
            const trueFalseAttempt: TrueFalseAttempt = { questionType: "trueFalse", trueFalseId: q.trueFalseId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: null };
            setTrueFalseAttempts(prevAttempts =>
                [...prevAttempts, trueFalseAttempt]
            );
        });

        multipleChoice.forEach(q => {
            const multipleChoiceAttempt: MultiplechoiceAttempt = { questionType: "multipleChoice", multipleChoiceId: q.multipleChoiceId!, quizAttemptId: quizAttempt?.quizAttemptId!, quizQuestionNum: q.quizQuestionNum, userAnswer: "" };
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

        
        let score = 0;
        let total = allQuestions.length;
        
        allQuestions.forEach(q => {
            if (q.questionType === "fillInTheBlank") {
                const att = fibAttempts.find(a => a.fillInTheBlankId === q.fillInTheBlankId);
                if (att?.userAnswer?.trim().toLowerCase() === q.correctAnswer.trim().toLowerCase()) {
                    att.answeredCorrectly = true;
                    score++;
                }
            }
            
            if (q.questionType === "trueFalse") {
                const att = trueFalseAttempts.find(a => a.trueFalseId === q.trueFalseId);
                if (att?.userAnswer === q.correctAnswer) {
                    att.answeredCorrectly = true;
                    score++;
                }
            }
            
            if (q.questionType === "multipleChoice") {
                const att = multipleChoiceAttempts.find(a => a.multipleChoiceId === q.multipleChoiceId);
                const correctOption = q.options.find(o => o.isCorrect);
                if (att?.userAnswer && correctOption && att.userAnswer === correctOption.text) {
                    att.answeredCorrectly = true;
                    score++;
                }
            }
            
            if (q.questionType === "matching") {
                const att = matchingAttempts.find(a => a.matchingId === q.matchingId);
                if (att?.userAnswer === q.correctAnswer) {
                    att.answeredCorrectly = true;
                    score++;
                }
            }
            
            if (q.questionType === "sequence") {
                const att = sequenceAttempts.find(a => a.sequenceId === q.sequenceId);
                if (att?.userAnswer === q.correctAnswer) {
                    att.answeredCorrectly = true;
                    score++;
                }
            }
            
            if (q.questionType === "ranking") {
                const att = rankingAttempts.find(a => a.rankingId === q.rankingId);
                if (att?.userAnswer === q.correctAnswer) {
                    att.answeredCorrectly = true;
                    score++;
                }
            }
        });
        
        const submitCalls = [
            ...fibAttempts.map(submitFibAttempt),
            ...trueFalseAttempts.map(submitTrueFalseAttempt),
            ...multipleChoiceAttempts.map(submitMultipleChoiceAttempt),
            ...matchingAttempts.map(submitMatchingAttempt),
            ...sequenceAttempts.map(submitSequenceAttempt),
            ...rankingAttempts.map(submitRankingAttempt),
            updateQuizAttemptScore(score)
        ];

        await Promise.all(submitCalls);


        navigate(`/quiz/results/${quiz?.quizId}/${quizAttempt.quizAttemptId}`);
    }

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
                            <div key={question.quizQuestionNum}>
                                {question.questionType === "fillInTheBlank" ? (
                                    <FillInTheBlankComponent
                                        fillInTheBlankId={question.fillInTheBlankId!}
                                        quizQuestionNum={question.quizQuestionNum}
                                        question={question.question}
                                        userAnswer={(fibAttempts.find(attempt => attempt.fillInTheBlankId === question.fillInTheBlankId))?.userAnswer!}
                                        handleAnswer={handleAnswerFib} />
                                ) : question.questionType === "trueFalse" ? (
                                    <TrueFalseComponent
                                        trueFalseId={question.trueFalseId!}
                                        quizQuestionNum={question.quizQuestionNum}
                                        question={question.question}
                                        userAnswer={(trueFalseAttempts.find(attempt => attempt.trueFalseId === question.trueFalseId))?.userAnswer}
                                        handleAnswer={handleAnswerTrueFalse} />
                                ) : question.questionType === "multipleChoice" ? (
                                    <MultipleChoiceComponent
                                        multipleChoiceId={question.multipleChoiceId!}
                                        quizQuestionNum={question.quizQuestionNum}
                                        question={question.question}
                                        userAnswer={(multipleChoiceAttempts.find(attempt => attempt.multipleChoiceId === question.multipleChoiceId))?.userAnswer}
                                        options={question.options}
                                        handleAnswer={(handleAnswerMultipleChoice)} />
                                ) : question.questionType === "matching" ? (
                                    <MatchingComponent
                                        matchingId={question.matchingId!}
                                        quizQuestionNum={question.quizQuestionNum}
                                        questionItems={MatchingService.assemble(MatchingService.shuffleQuestion(MatchingService.splitQuestion(question.correctAnswer).keys, MatchingService.splitQuestion(question.correctAnswer).values))}
                                        question={question.question}
                                        handleAnswer={handleAnswerMatching} />
                                ) : question.questionType === "sequence" ? (
                                    <SequenceComponent
                                        sequenceId={question.sequenceId!}
                                        quizQuestionNum={question.quizQuestionNum}
                                        questionItems={SequenceService.shuffleQuestion(question.correctAnswer.split(","))}
                                        question={question.question}
                                        userAnswer={(sequenceAttempts.find(attempt => attempt.sequenceId === question.sequenceId))?.userAnswer}
                                        handleAnswer={handleAnswerSequence} />
                                ) : (
                                    <RankingComponent
                                        rankingId={question.rankingId!}
                                        quizQuestionNum={question.quizQuestionNum}
                                        questionItems={RankingService.shuffleQuestion(question.correctAnswer.split(","))}
                                        question={question.question}
                                        userAnswer={(rankingAttempts.find(attempt => attempt.rankingId === question.rankingId))?.userAnswer}
                                        handleAnswer={handleAnswerRanking} />
                                )}
                            </div>
                        ))}
                        <button className="button primary-button active" onClick={submitQuiz}>Submit</button>
                    </div>
                </>
            )}
        </>
    )
}

export default QuizPage;