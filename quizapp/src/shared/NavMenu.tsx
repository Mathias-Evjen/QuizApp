import { Link } from "react-router-dom";

const NavMenu: React.FC = () => {
    return(
        <div className="nav-bar">
            <Link to="/" className="nav-item nav-home<">QuizApp</Link>
            <Link to="/flashCardQuizzes" className="nav-item">Flash cards</Link>
        </div>
    )
}

export default NavMenu;