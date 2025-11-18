import { NavLink } from "react-router-dom";

const NavMenu: React.FC = () => {
    return(
        <nav className="nav-bar">
            <div className="nav-items">
                <NavLink to="/" className={() => "nav-item nav-home"}>QuizApp</NavLink>
                <NavLink to="/flashCards" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Flash cards</NavLink>
                <NavLink to="/question" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Test side</NavLink>
            </div>
            <ul className="nav-links">
              <li><NavLink to="/">Quizzes</NavLink></li>
              <li><NavLink to="/flashCardQuizzes">Flashcard quizzes</NavLink></li>
            </ul>
            <div className="login">
            </div>
        </nav>
    )
}

export default NavMenu;
