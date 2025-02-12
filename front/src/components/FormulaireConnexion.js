import React, { useState, Image } from 'react';
import { useNavigate } from 'react-router-dom';
import './FormulaireConnexion.css';
// import NouveauLogo from '../public/NouveauLogo';

const FormulaireConnexion = () => {
    const navigate = useNavigate();
    const [identifiant, setIdentifiant] = useState('');
    const [motDePasse, setMotDePasse] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        const response = await fetch('http://localhost:3000/', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ identifiant, motDePasse })
        });

        const data = await response.json();
        alert(data.message || data.error);
        navigate('/accueil');
    };

    return (
        <div className="connexion-container">
            <h2>Bienvenue</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    placeholder="Identifiant"
                    value={identifiant}
                    onChange={(e) => setIdentifiant(e.target.value)}
                />
                <input
                    type="password"
                    placeholder="Mot de passe"
                    value={motDePasse}
                    onChange={(e) => setMotDePasse(e.target.value)}
                />
                <button type="submit">Se connecter</button>
            </form>
        </div>
    );
};

export default FormulaireConnexion;