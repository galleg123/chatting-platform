import React, { useState } from 'react';
import styles from '../styles/Login.module.css';
import { User } from '../types/index'; // Adjust the import path as necessary

interface LoginProps {
    onLogin: (user: User) => void;
}

const Login: React.FC<LoginProps> = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [isRegistering, setIsRegistering] = useState(false); // Toggle between login and register
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        const endpoint = isRegistering ? '/api/User/register' : '/api/User/login';
        const payload = { username, password };

        try {
            const response = await fetch(`http://localhost:5062${endpoint}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                throw new Error(errorMessage);
            }

            if (!isRegistering) {
                // If logging in, call the onLogin function
                const data: User = await response.json();
                onLogin(data);
            } else {
                alert('Registration successful! You can now log in.');
                setIsRegistering(false); // Switch back to login mode
            }
        } catch (err: any) {
            setError(err.message);
            console.error('Error during login/register:', err);
        }
    };

    return (
        <div className={styles.loginContainer}>
            <h2>{isRegistering ? 'Register' : 'Login'}</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Username:</label>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                {error && <p className={styles.error}>{error}</p>}
                <button type="submit">{isRegistering ? 'Register' : 'Login'}</button>
            </form>
            <button
                className={styles.toggleButton}
                onClick={() => setIsRegistering(!isRegistering)}
            >
                {isRegistering ? 'Already have an account? Login' : 'Don’t have an account? Register'}
            </button>
        </div>
    );
};

export default Login;