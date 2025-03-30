import React, { useEffect, useState } from 'react';
import './MaPerleRare.css';

const MaPerleRare = () => {
    const [eleve, setEleve] = useState(null);
    const [perle, setPerle] = useState(null);

    useEffect(() => {
        const userId = localStorage.getItem("userId");
        if (!userId) return;

        // 1. Charger les infos de l'élève
        const fetchEleve = async () => {
            const res = await fetch(`http://localhost:5166/api/eleve/${userId}`);
            const data = await res.json();
            setEleve(data);

            // 2. Soit son parrain, soit ses filleuls
            if (data.promotionId === 1) {
                // Il est parrain → chercher ses filleuls
                const resFilleuls = await fetch(`http://localhost:5166/api/eleve/filleuls/${userId}`);
                const filleuls = await resFilleuls.json();
                setPerle(filleuls);
            } else {
                // Il est filleul → chercher son parrain
                const resParrain = await fetch(`http://localhost:5166/api/eleve/${data.eleveParrainId}`);
                const parrain = await resParrain.json();
                setPerle(parrain);
            }
        };

        fetchEleve();
    }, []);

    if (!eleve) return <p>Chargement...</p>;

    return (
        <div style={{ textAlign: 'center', padding: '2rem' }}>
            <h2>Ma perle rare</h2>
            {eleve.promotionId === 1 ? (
                <>
                    <h3>Mes filleuls :</h3>
                    {Array.isArray(perle) && perle.length > 0 ? (
                        <ul>
                            {perle.map(f => (
                                <li key={f.id}>{f.prenom} {f.nom}</li>
                            ))}
                        </ul>
                    ) : (
                        <p>Tu n’as pas encore de filleuls.</p>
                    )}
                </>
            ) : (
                <>
                    <h3>Mon parrain :</h3>
                    {perle ? (
                        <p>{perle.prenom} {perle.nom}</p>
                    ) : (
                        <p>Tu n’as pas encore de parrain.</p>
                    )}
                </>
            )}
        </div>
    );
};

export default MaPerleRare;
