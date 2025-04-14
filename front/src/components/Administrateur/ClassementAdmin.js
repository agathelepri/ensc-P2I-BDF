// Ce composant permet à l’administrateur de gérer les scores des familles par couleur.
// Il permet d’entrer, valider ou réinitialiser les points attribués aux équipes.
// Les scores sont enregistrés dans le localStorage pour être persistants côté client.

import React, { useState, useEffect } from 'react';
import './ClassementAdmin.css';

const couleurs = [
    { nom: 'Bleu', classe: 'bleu' },
    { nom: 'Rouge', classe: 'rouge' },
    { nom: 'Orange', classe: 'orange' },
    { nom: 'Vert', classe: 'vert' },
    { nom: 'Jaune', classe: 'jaune' },
];

const ClassementAdmin = () => {
    const [points, setPoints] = useState({
        Bleu: 0,
        Rouge: 0,
        Orange: 0,
        Vert: 0,
        Jaune: 0,
    });

    const [ajouts, setAjouts] = useState({
        Bleu: '',
        Rouge: '',
        Orange: '',
        Vert: '',
        Jaune: '',
    });

    useEffect(() => {
        const sauvegarde = localStorage.getItem('classementPoints');
        if (sauvegarde) {
            setPoints(JSON.parse(sauvegarde));
        }
    }, []);

    const handleChange = (e, couleur) => {
        setAjouts({ ...ajouts, [couleur]: e.target.value });
    };

    const validerPoints = () => {
        const newPoints = { ...points };
        for (let couleur of Object.keys(points)) {
            const ajout = parseInt(ajouts[couleur]) || 0;
            newPoints[couleur] += ajout;
        }
        setPoints(newPoints);
        localStorage.setItem('classementPoints', JSON.stringify(newPoints));
        setAjouts({
            Bleu: '',
            Rouge: '',
            Orange: '',
            Vert: '',
            Jaune: '',
        });
    };

    const resetPoints = () => {
        const reset = {
            Bleu: 0,
            Rouge: 0,
            Orange: 0,
            Vert: 0,
            Jaune: 0,
        };
        setPoints(reset);
        localStorage.setItem('classementPoints', JSON.stringify(reset));
    };

    return (
        <div className="classement-container">
            {/* <img src="/logo.png" alt="Logo" className="logo" /> */}
            <h2 className="titre">Classement (Admin)</h2>

            {couleurs.map((c) => (
                <div key={c.nom} className={`ligne ${c.classe}`}>
                    <div className="nom-couleur">{c.nom}</div>
                    <input
                        className="input-points"
                        type="number"
                        placeholder="Pts"
                        value={ajouts[c.nom]}
                        onChange={(e) => handleChange(e, c.nom)}
                    />
                    <div className="points">{points[c.nom]} pts</div>
                </div>
            ))}

            <button className="btn-valider" onClick={validerPoints}>
                Valider
            </button>
            <button className="btn-reset" onClick={resetPoints}>
                Réinitialiser
            </button>
        </div>
    );
};

export default ClassementAdmin;
