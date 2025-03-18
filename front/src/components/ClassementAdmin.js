import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './ClassementAdmin.css';

const ClassementAdmin = () => {
    const navigate = useNavigate();

    // États pour stocker les points de chaque famille
    const [classement, setClassement] = useState([
        { id: 1, nom: "Bleu", points: 300, couleur: "#5A71E8" },
        { id: 2, nom: "Rouge", points: 275, couleur: "#E85A5A" },
        { id: 3, nom: "Orange", points: 120, couleur: "#E89B5A" },
        { id: 4, nom: "Vert", points: 15, couleur: "#5AE87A" },
        { id: 5, nom: "Jaune", points: 2, couleur: "#E8D15A" },
    ]);

    // Gérer la modification des points
    const handlePointsChange = (id, newPoints) => {
        setClassement(classement.map(famille =>
            famille.id === id ? { ...famille, points: newPoints } : famille
        ));
    };

    // Gérer la soumission des points
    const handleSubmit = () => {
        console.log("Classement mis à jour :", classement);
        alert("Classement mis à jour avec succès !");
        navigate('/accueilAdmin'); // Retour à l'accueil admin après validation
    };

    return (
        <div className="classement-container">
           {/*  <img src="/img/logo.png" alt="Logo" className="classement-logo" /> */}
            <h2>Classement</h2>

            <div className="classement-list">
                {classement.map((famille) => (
                    <div key={famille.id} className="classement-item" style={{ backgroundColor: famille.couleur }}>
                        <span className="classement-nom">{famille.nom}</span>
                        <input
                            type="number"
                            className="classement-input"
                            value={famille.points}
                            onChange={(e) => handlePointsChange(famille.id, parseInt(e.target.value))}
                        />
                        <span className="classement-pts">pts</span>
                    </div>
                ))}
            </div>

            <button className="classement-valider" onClick={handleSubmit}>Valider</button>
        </div>
    );
};

export default ClassementAdmin;
