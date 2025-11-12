import { useEffect, useState } from "react";
import { FillInTheBlank } from "../types/fillInTheBlank";
import * as FillInTheBlankService from "./FillInTheBlankService";
import FillInTheBlankComponent from "./FillInTheBlankComponent";

const TestSide: React.FC = () => {
    const [questions, setQuestions] = useState<FillInTheBlank[]>([]);
    const quizId = 1;

    const fetchQuestions = async () => {
        try {
            const data = await FillInTheBlankService.fetchQuestions(quizId);
            setQuestions(data);
            console.log(data);
        } catch (error: unknown) {
            if (error instanceof Error) {
                console.error(`There was a problem fetching data: ${error.message}`);
            } else {
                console.error("Unknown error", error);
            }
        }
    }

    const fakeHandleAnswer = (userAnwer: string) => {

    }

    useEffect(() => {
        fetchQuestions();
    }, []);

    return(
        <div className="test-side">
            {questions.map(question =>
                <FillInTheBlankComponent
                    key={question.fillInTheBlankId}
                    question={question.question}
                    userAnswer=""
                    quizQuestionNum={question.quizQuestionNum}
                    handleAnswer={fakeHandleAnswer}
                    />
            )}
        </div>
    )
}

export default TestSide;