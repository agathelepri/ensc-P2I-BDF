import React, { useState, useEffect } from 'react';
import './Classement.css';

const Classement = () => {
    const [classement, setClassement] = useState([]);

    // Charger le classement depuis l'API
    useEffect(() => {
        const fetchClassement = async () => {
            try {
                const response = await fetch('http://localhost:5166/api/classement');
                if (!response.ok) throw new Error('Erreur lors du chargement du classement');

                const data = await response.json();
                setClassement(data);
            } catch (error) {
                console.error('Erreur:', error);
            }
        };

        fetchClassement();

        // Mettre à jour toutes les 10 secondes pour refléter les changements admin
        const interval = setInterval(fetchClassement, 10000);
        return () => clearInterval(interval);
    }, []);

    return (
        <div className="classement-container">
            <img src="/img/logo.png" alt="Logo" className="classement-logo" />
            <h2>Classement</h2>

            <div className="classement-list">
                {classement.map((famille) => (
                    <div key={famille.id} className="classement-item" style={{ backgroundColor: famille.couleur }}>
                        <span className="classement-nom">{famille.nom}</span>
                        <span className="classement-points">{famille.points} pts</span>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Classement;
