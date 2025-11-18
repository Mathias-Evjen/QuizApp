import { NavLink } from "react-router-dom";

const NavMenu: React.FC = () => {
    return(
        <nav className="nav-bar">
            <div className="nav-items">
                <NavLink to="/" className={() => "nav-item nav-home"}>QuizApp</NavLink>
                <NavLink to="/quiz" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Quiz</NavLink>
                <NavLink to="/flashCards" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Flash cards</NavLink>
                <NavLink to="/question" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Test side</NavLink>
            </div>
            <div className="login">
            </div>
        </nav>
    )
}

export default NavMenu;
