import { InfoOutline, KeyboardArrowLeft, KeyboardArrowRight, SpaceBar } from "@mui/icons-material";


const InfoCard: React.FC = () => {

    return(
        <div className="flash-card-page-info-icon">
            <InfoOutline />
            <div className="flash-card-info-card">
                <div className="flash-card-info-title">
                    <h3>Keyboard controls</h3>
                </div>
                <div className="info-row">
                    <div className="key-info">
                        <div className="keyboard-key">
                            <KeyboardArrowLeft />
                        </div>
                        <p>Prev card</p>
                    </div>
                    <div>
                        <div className="keyboard-key">
                            <KeyboardArrowRight />
                        </div>
                        <p>Next card</p>
                    </div>
                </div>
                <div className="info-row">
                    
                </div>
                <div className="info-row">
                    <div className="key-info">
                        <div className="keyboard-key">
                            <SpaceBar />
                        </div>
                        <p>Show answer</p>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default InfoCard;