// Ce composant affiche la correspondance de parrainage de l’élève connecté.
// Il montre soit le parrain si l’élève est filleul, soit les filleuls s’il est parrain.
// Les données sont récupérées automatiquement selon la promotion de l’élève et les relations en base.

import React, { useEffect, useState } from 'react';
import './MaPerleRare.css';

const MaPerleRare = () => {
    const [eleve, setEleve] = useState(null);
    const [perle, setPerle] = useState(null);
    const [isFilleul, setIsFilleul] = useState(false);

    useEffect(() => {
        const userId = localStorage.getItem("userId");
        if (!userId) return;

        const fetchEleve = async () => {
            try {
                const resEleve = await fetch(`http://localhost:5166/api/eleve/${userId}`);
                const data = await resEleve.json();
                setEleve(data);
 
                const resPromos = await fetch("http://localhost:5166/api/promotion");
                const promos = await resPromos.json();
                const promoMax = Math.max(...promos.map(p => p.annee));

                
                if (data.promotion.annee === promoMax) {
                    setIsFilleul(true);

                    if (data.eleveParrainId !== 0) {
                        const resParrain = await fetch(`http://localhost:5166/api/eleve/${data.eleveParrainId}`);
                        const parrain = await resParrain.json();
                        setPerle(parrain);
                    } else {
                        setPerle(null);
                    }
                } else {
                    setIsFilleul(false);

                    const resFilleuls = await fetch(`http://localhost:5166/api/eleve/filleuls/${userId}`);
                    const filleuls = await resFilleuls.json();
                    setPerle(filleuls);
                }

            } catch (error) {
                console.error("Erreur de chargement :", error);
            }
        };

        fetchEleve();
    }, []);

    if (!eleve) return <p style={{ textAlign: "center", marginTop: "2rem" }}>Chargement...</p>;

    return (
        <div className="perle-container">
            <h2>Ma perle rare</h2>

            {isFilleul ? (
                <>
                    <h3>Mon parrain :</h3>
                    {perle ? (
                        <div className="perle-box">
                            <p>{perle.prenom} {perle.nom}</p>
                        </div>
                    ) : (
                        <p>Tu n’as pas encore de parrain.</p>
                    )}
                </>
            ) : (
                <>
                    <h3>Mes filleuls :</h3>
                    {Array.isArray(perle) && perle.length > 0 ? (
                        <ul className="filleuls-list">
                            {perle.map(f => (
                                <li key={f.id}>{f.prenom} {f.nom}</li>
                            ))}
                        </ul>
                    ) : (
                        <p>Tu n’as pas encore de filleuls.</p>
                    )}
                </>
            )}
        </div>
    );
};

export default MaPerleRare;
