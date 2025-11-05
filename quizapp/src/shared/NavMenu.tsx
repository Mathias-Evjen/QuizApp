import { NavLink } from "react-router-dom";

const NavMenu: React.FC = () => {
    return(
        <nav className="nav-bar">
            <NavLink to="/" className="nav-item nav-home">QuizApp</NavLink>
            <NavLink to="/flashCards" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Flash cards</NavLink>
        </nav>
    )
}

export default NavMenu;