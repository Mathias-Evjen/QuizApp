import { NavLink } from "react-router-dom";

const NavMenu: React.FC = () => {
    return(
        <nav className="nav-bar">
            <div className="nav-items">
                <NavLink to="/" className={() => "nav-item nav-home"}>QuizApp</NavLink>
                <NavLink to="/flashCards" className={({ isActive }) => isActive ? "nav-item active " : "nav-item" }>Flash cards</NavLink>
            </div>
            <div className="login">
            </div>
        </nav>
    )
}

export default NavMenu;