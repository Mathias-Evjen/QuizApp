import React, { useState } from 'react';
import { NavLink } from 'react-router-dom';
import { useAuth } from './AuthContext';
import { Dropdown } from 'react-bootstrap';
import "../App.css";

const AuthSection: React.FC = () => {
    const { user, logout } = useAuth();
    
    const [showMenu, setShowMenu] = useState<boolean>(false);

    const handleShowMenu = () => {
        setShowMenu(!showMenu);
    };

    return (
        <>
            {user ? (
                <div className={showMenu ? "dropdown" : "nav-item"}>
                    <div className='nav-item' onClick={handleShowMenu}>
                        <p>Welcome, {user.sub}</p>
                    </div>
                    {showMenu ? (
                        <div className='nav-item' onClick={logout}>
                            <p>Logout</p>
                        </div>
                    ) : (
                        <></>
                    )}
                </div>
            ) : (
                <>
                    <NavLink className='nav-item' to="/login">Login</NavLink>
                    <NavLink className='nav-item' to="/register">Register</NavLink>  
                </>
            )}
        </>
    );
};

export default AuthSection;