import { NavLink } from "react-router-dom";
import AuthSection from "../auth/AuthSection";
import "../App.css";

const NavMenu: React.FC = () => {
    return(
        <nav className="nav-bar">
            <div className="nav-items">
                <NavLink to="/" className={() => "nav-item nav-home"}>QuizApp</NavLink>
                <NavLink to="/quiz" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Quiz</NavLink>
                <NavLink to="/flashCards" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Flash cards</NavLink>
            </div>
            <div className="login">
                <AuthSection />
            </div>
        </nav>
    )
}

export default NavMenu;
