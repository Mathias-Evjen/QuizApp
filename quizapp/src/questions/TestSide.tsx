import { useEffect, useState } from "react";
import { FillInTheBlank } from "../types/fillInTheBlank";
import * as FillInTheBlankService from "./FillInTheBlankService";
import FillInTheBlankComponent from "./FillInTheBlankComponent";
import FillInTheBlankEdit from "./FillInTheBlankEdit";

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

    const fakeHandleAnswer = (userAnwer: string) => {}

    const fakeQuestionChanged = (fillInTheBlankId: number, newQuetsion: string) => {}

    const fakeAnwerChanged = (fillInTheBlankId: number, newAnswer: string) => {}

    const fakeDeletePressed = (fillInTheBlankId: number, quizQuestionNum: number) => {}

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

            <FillInTheBlankEdit 
                fillInTheblankId={1}
                quizQuestionNum={1}
                question="What is the capital of Norway?"
                answer="Oslo"
                onQuestionChanged={fakeQuestionChanged}
                onAnswerChanged={fakeAnwerChanged} 
                onDeletePressed={fakeDeletePressed}/>
        </div>
    )
}

export default TestSide;