import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './ClassementAdmin.css';

const ClassementAdmin = () => {
    const navigate = useNavigate();
    const [classement, setClassement] = useState([]);
    const isAdmin = localStorage.getItem("role") === "admin";

    useEffect(() => {
        const fetchClassement = async () => {
            try {
                console.log("Chargement du classement...");
                const response = await fetch('http://localhost:5166/api/classement');

                if (!response.ok) throw new Error(`Erreur serveur: ${response.status}`);

                const data = await response.json();
                console.log("Classement reçu:", data);
                setClassement(data);
            } catch (error) {
                console.error("Erreur:", error);
            }
        };

        fetchClassement();
    }, []);

    const handlePointsChange = (id, newPoints) => {
        if (!isAdmin) return;
        setClassement(classement.map(famille =>
            famille.id === id ? { ...famille, points: newPoints } : famille
        ));
    };

    const handleSubmit = async () => {
        if (!isAdmin) return;

        try {
            await fetch('http://localhost:5166/api/classement', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'role': localStorage.getItem("role")
                },
                body: JSON.stringify(classement),
            });

            alert("Classement mis à jour !");
        } catch (error) {
            console.error("Erreur lors de la mise à jour :", error);
        }
    };

    return (
        <div className="classement-container">
            {/* <img src="/img/logo.png" alt="Logo" className="classement-logo" /> */}
            <h2>Classement</h2>

            <div className="classement-list">
                {classement.map((famille) => (
                    <div key={famille.id} className="classement-item" style={{ backgroundColor: famille.couleur }}>
                        <span className="classement-nom">{famille.nom}</span>
                        <div className="classement-input-container">
                            <input
                                type="number"
                                className="classement-input"
                                value={famille.points}
                                onChange={(e) => handlePointsChange(famille.id, parseInt(e.target.value))}
                            />
                            <span className="classement-pts">Pts</span>
                        </div>
                        <span className="classement-total-points">{famille.points} pts</span>
                    </div>
                ))}
            </div>

            {isAdmin && <button className="classement-valider" onClick={handleSubmit}>Valider</button>}
        </div>
    );
};

export default ClassementAdmin;
