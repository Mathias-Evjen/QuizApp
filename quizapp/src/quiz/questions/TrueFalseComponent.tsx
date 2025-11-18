interface TrueFalseProps {
    handleAnswer: (trueFalseId: number, newAnswer: boolean) => void;
    trueFalseId: number;
    quizQuestionNum: number;
    question: string;
    userAnswer: boolean | null | undefined;
}

const TrueFalseComponent: React.FC<TrueFalseProps> = ({ trueFalseId, quizQuestionNum, question, userAnswer, handleAnswer }) => {

    return(
        <div className="tf-quiz-wrapper">
            <br /><br />
            <div className="tf-card">
                <div>
                    <h3>Question {quizQuestionNum}</h3>
                    <p>{question}</p>
                    <hr />
                    <div className="tf-options-wrapper">
                    <label className="tf-option">
                        <input type="radio" name={`tf-${trueFalseId}`} checked={userAnswer === true} onChange={() => handleAnswer(trueFalseId, true)}/>
                        True
                    </label>
                    <label className="tf-option">
                        <input type="radio" name={`tf-${trueFalseId}`} checked={userAnswer === false} onChange={() => handleAnswer(trueFalseId, false)}/>
                        False
                    </label>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default TrueFalseComponent;