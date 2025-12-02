import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import * as authService from './AuthService';
import "./Auth.css";

const RegisterPage: React.FC = () => {
    const [formData, setFormData] = useState({
        username: '',
        email: '',
        password: '',
    });
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const navigate = useNavigate();
    const location = useLocation();

    const fromLocation = location.state?.from || { pathname: "/" };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        setSuccess(null);
        try {
            await authService.register(formData);
            setSuccess('Registration successful! You can now log in.');
            setTimeout(() => navigate('/login', { state: { from: fromLocation } }), 2000);
        } catch (err) {
            if (err instanceof Error) {
                setError(err.message);
            } else {
                setError('An unknown error occurred.');
            }
            console.error(err);
        }
    };

    return (
        <div className="auth-register-container">
            <div className="auth-register-card">
                <h1>Register</h1>

                {error && <p className="auth-register-error">{error}</p>}
                {success && <p className="auth-register-success">{success}</p>}

                <form className="auth-register-form" onSubmit={handleSubmit} noValidate>
                    <input type="text" name="username" placeholder="Username" value={formData.username} onChange={handleChange} required/>
                    <input type="email" name="email" placeholder="Email" value={formData.email} onChange={handleChange} required/>
                    <input type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} required/>
                    <button type="submit" className="auth-register-btn">
                        Register
                    </button>
                </form>
                <p className="auth-register-switch">
                    Already have an account?
                    <span onClick={() => navigate("/login")}> Login</span>
                </p>
            </div>
        </div>
    );
};

export default RegisterPage;