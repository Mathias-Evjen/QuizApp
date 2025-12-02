import React, { useState } from 'react';
import { useAuth } from './AuthContext';
import { useLocation, useNavigate } from 'react-router-dom';
import "./Auth.css";

const LoginPage: React.FC = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);

    const { login } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    const fromLocation = location.state?.from || { pathname: "/" };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        try {
            await login({ username, password });
            navigate(fromLocation.pathname, { replace: true });
        } catch (err) {
            console.error(err);
            setError('Invalid username or password.');
        }
    };

    return (
        <div className="auth-login-container">
            <div className="auth-login-card">
                <h1>Login</h1>

                {error && <p className="auth-login-error">{error}</p>}

                <form onSubmit={handleSubmit} className="auth-login-form">
                    <input type="text" placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} required/>
                    <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} required/>
                    <button type="submit" className="auth-login-btn">
                        Login
                    </button>
                </form>
                <p className="auth-login-switch">
                    Don't have an account?
                    <span onClick={() => navigate("/register")}> Register</span>
                </p>
            </div>
        </div>
    );
};


export default LoginPage;
