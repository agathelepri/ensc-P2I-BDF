import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Acceuil.css';

const Accueil = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        // Supprime les informations de session (exemple : token, utilisateur)
        localStorage.removeItem("token");
        localStorage.removeItem("user");

        // Redirige vers la page de connexion
        navigate('/');
    };

    return (
        <div className="accueil-container">
            {/* Lien Déconnexion en haut à droite */}
            <div className="logout-text" onClick={handleLogout}>
                Déconnexion
            </div>

            {/* Contenu de l'accueil */}
            <div className="menu">
                <button onClick={() => navigate('/questionnaire')}>Questionnaire</button>
                <button onClick={() => navigate('/voeux')}>Voeux</button>
                <button onClick={() => navigate('/perle-rare')}>Qui est ma perle rare ?</button>
                <button onClick={() => navigate('/profils')}>Profils</button>
                <button onClick={() => navigate('/classement')}>Classement</button>
                <button onClick={() => navigate('/histoire-famille')}>Histoire de famille</button>
            </div>
        </div>
    );
};

export default Accueil;
