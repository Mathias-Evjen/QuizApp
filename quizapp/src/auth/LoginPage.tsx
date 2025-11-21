import React, { useState } from 'react';
import { useAuth } from './AuthContext';
import { useLocation, useNavigate } from 'react-router-dom';
import { Form, Button, Container, Alert } from 'react-bootstrap';

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
        <Container className="mt-5" style={{ maxWidth: "450px" }}>
            <h2 className="mb-4">Login</h2>

            {error && <Alert variant="danger">{error}</Alert>}

            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3" controlId="formBasicUsername">
                    <Form.Label>Username</Form.Label>
                    <Form.Control
                        type="text"
                        placeholder="Enter username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formBasicPassword">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </Form.Group>

                <Button variant="primary" type="submit" className="w-100">
                    Login
                </Button>
            </Form>
        </Container>
    );
};

export default LoginPage;
