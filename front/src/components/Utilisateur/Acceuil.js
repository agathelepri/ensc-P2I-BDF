// Ce composant est la page d'accueil accessible aux utilisateurs connectés.
// Il affiche le menu général avec les accès aux modules Questionnaire, Vœux, Profil, Classement, etc.
// Il filtre dynamiquement les accès en fonction de l’année de promotion (seuls les promos les plus récentes, 1A et 2A, peuvent participer).

import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Acceuil.css';

const Accueil = () => {
    const navigate = useNavigate();
    const [anneeAutorisees, setAnneeAutorisees] = useState([]);
    const [anneeUser, setAnneeUser] = useState(null);

    useEffect(() => {
        const userId = JSON.parse(localStorage.getItem("userId"));
        if (!userId) return;

        const fetchUser = async () => {
            try {
                const response = await fetch(`http://localhost:5166/api/eleve/${userId}`);
                if (!response.ok) throw new Error("Erreur récupération élève connecté");
                const data = await response.json();
                setAnneeUser(data.promotion.annee);
            } catch (err) {
                console.error(err);
            }
        };
        fetchUser();
    }, []);
    useEffect(() => {

        const fetchPromotions = async () => {
            try {
                const res = await fetch("http://localhost:5166/api/promotion");
                const data = await res.json();
                const anneesTriees = data.map(p => p.annee).sort((a, b) => b - a); // descendante
                const autorisees = anneesTriees.slice(0, 2); // max et max-1
                setAnneeAutorisees(autorisees);


            } catch (err) {
                console.error("Erreur chargement promotions :", err);
            }
        };



        fetchPromotions();
    }, []);

    const handleLogout = () => {
        localStorage.removeItem("role");
        localStorage.removeItem("userId");
        navigate('/');
    };

    const estAutorise = anneeUser && anneeAutorisees.includes(anneeUser);

    return (
        <div className="accueil-container">
            <div className="logout-text" onClick={handleLogout}>
                Déconnexion
            </div>

            <div className="menu">
                {estAutorise && (
                    <>
                        <button onClick={() => navigate('/questionnaire')}>Questionnaire</button>
                        <button onClick={() => navigate('/voeux')}>Voeux</button>
                        <button onClick={() => navigate('/perle-rare')}>Qui est ma perle rare ?</button>
                        <button onClick={() => navigate('/profil')}>Profils</button>
                    </>
                )}
                <button onClick={() => navigate('/classement')}>Classement</button>
                <button onClick={() => navigate('/histoire-famille')}>Histoire de famille</button>
            </div>
        </div>
    );
};

export default Accueil;
