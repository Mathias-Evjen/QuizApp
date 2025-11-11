import React from 'react';
import { Link } from 'react-router-dom';
import "../App.css";

const NavMenu: React.FC = () => {
  return (
    <nav className="nav-menu">
      <div className="nav-brand">
        <Link to="/">QuizApp</Link>
      </div>
      <ul className="nav-links">
        <li><Link to="/">Quizzes</Link></li>
        <li><Link to="/flashCardQuizzes">Flashcard quizzes</Link></li>
      </ul>
    </nav>
  );
};

export default NavMenu;
