import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Acceuil.css';

const Accueil = () => {
    const navigate = useNavigate();
    const [anneeAutorisees, setAnneeAutorisees] = useState([]);
    const [anneeUser, setAnneeUser] = useState(null);

    useEffect(() => {
        const user = JSON.parse(localStorage.getItem("user"));
        if (!user) return;

        const fetchPromotions = async () => {
            try {
                const res = await fetch("http://localhost:5166/api/promotion");
                const data = await res.json();
                const anneesTriees = data.map(p => p.annee).sort((a, b) => b - a); // descendante
                const autorisees = anneesTriees.slice(0, 2); // max et max-1
                setAnneeAutorisees(autorisees);

                const promoUser = data.find(p => p.id === user.promotionId);
                if (promoUser) setAnneeUser(promoUser.annee);
            } catch (err) {
                console.error("Erreur chargement promotions :", err);
            }
        };

        fetchPromotions();
    }, []);

    const handleLogout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
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
                        <button onClick={() => navigate('/profils')}>Profils</button>
                    </>
                )}
                <button onClick={() => navigate('/classement')}>Classement</button>
                <button onClick={() => navigate('/histoire-famille')}>Histoire de famille</button>
            </div>
        </div>
    );
};

export default Accueil;
