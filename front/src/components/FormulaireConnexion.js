import React, { useState, Image } from 'react';
import './FormulaireConnexion.css';
// import NouveauLogo from '../public/NouveauLogo';

const FormulaireConnexion = () => {
    const [login, setLogin] = useState('');
    const [mdp, setMdp] = useState('');
    const [showPassword, setShowPassword] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const response = await fetch('http://localhost:3000/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ login, mdp })
        });

        const data = await response.json();
        alert(data.message || data.error);
    };

    return (
        <div className="container">
            {/* <img src={NouveauLogo} alt="Logo" className="logo" /> */}
            {/* <Image src={NouveauLogo}></Image> */}
            <h1>Bienvenue</h1>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    placeholder="identifiant"
                    value={login}
                    onChange={(e) => setLogin(e.target.value)}
                />
                <div className="password-container">
                    <input
                        type={showPassword ? "text" : "password"}
                        placeholder="mot de passe"
                        value={mdp}
                        onChange={(e) => setMdp(e.target.value)}
                    />
                    <button
                        type="button"
                        className="toggle-password"
                        onClick={() => setShowPassword(!showPassword)}
                    >
                        👁
                    </button>
                </div>
                <button type="submit">Se connecter</button>
            </form>
        </div>
    );
};

export default FormulaireConnexion;
