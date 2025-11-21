import React, { useState } from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import { useAuth } from './AuthContext';
import "../App.css";

const AuthSection: React.FC = () => {
    const { user, logout } = useAuth();
    const location = useLocation();
    
    const [showMenu, setShowMenu] = useState<boolean>(false);

    const handleShowMenu = () => {
        setShowMenu(!showMenu);
    };

    const handleLogout = () => {
        setShowMenu(false);
        logout();
    };

    return (
        <>
            {user ? (
                <div className={showMenu ? "dropdown" : "nav-item user"}>
                    <div className='nav-item user' onClick={handleShowMenu}>
                        <p>Welcome, {user.sub}</p>
                    </div>
                    {showMenu ? (
                        <div className='nav-item user' onClick={handleLogout}>
                            <p>Logout</p>
                        </div>
                    ) : (
                        <></>
                    )}
                </div>
            ) : (
                <>
                    <NavLink className='nav-item' to="/login" state={{ from: location }}>Login</NavLink>
                    <NavLink className='nav-item' to="/register" state={{ from: location }}>Register</NavLink>  
                </>
            )}
        </>
    );
};

export default AuthSection;