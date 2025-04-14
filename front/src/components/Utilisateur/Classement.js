// Ce composant affiche le classement des familles par couleur et par points.
// Il lit les points depuis le localStorage et les affiche de manière visuelle, sans possibilité de modification.
// Il est destiné à la consultation publique des scores.

import React, { useState, useEffect } from 'react';
import './Classement.css';

const couleurs = [
    { nom: 'Bleu', classe: 'bleu' },
    { nom: 'Rouge', classe: 'rouge' },
    { nom: 'Orange', classe: 'orange' },
    { nom: 'Vert', classe: 'vert' },
    { nom: 'Jaune', classe: 'jaune' },
];

const Classement = () => {
    const [points, setPoints] = useState({
        Bleu: 0,
        Rouge: 0,
        Orange: 0,
        Vert: 0,
        Jaune: 0,
    });

    useEffect(() => {
        const sauvegarde = localStorage.getItem('classementPoints');
        if (sauvegarde) {
            setPoints(JSON.parse(sauvegarde));
        }
    }, []);

    return (
        <div className="classement-container">
            {/* <img src="/logo.png" alt="Logo" className="logo" /> */}
            <h2 className="titre">Classement</h2>

            {couleurs.map((c) => (
                <div key={c.nom} className={`ligne ${c.classe}`}>
                    <div className="nom-couleur">{c.nom}</div>
                    <div className="points">{points[c.nom]} pts</div>
                </div>
            ))}
        </div>
    );
};

export default Classement;
