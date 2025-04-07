import React, { useEffect, useState } from 'react';
import './Profil.css';

const Profil = () => {
    const [promotions, setPromotions] = useState([]);
    const [eleves, setEleves] = useState([]);
    const [promotionCibleId, setPromotionCibleId] = useState(null);
    const [eleveSelectionne, setEleveSelectionne] = useState(null);

    useEffect(() => {
        const user = JSON.parse(localStorage.getItem("user"));
        if (!user) return;

        const fetchPromotions = async () => {
            try {
                const res = await fetch("http://localhost:5166/api/promotion");
                const data = await res.json();
                setPromotions(data);

                const promoUser = data.find(p => p.id === user.promotionId);
                if (!promoUser) return;

                const anneesTriees = data.map(p => p.annee).sort((a, b) => b - a);
                const anneeOpposee = promoUser.annee === anneesTriees[0] ? anneesTriees[1] : anneesTriees[0];
                const promoCible = data.find(p => p.annee === anneeOpposee);
                if (promoCible) setPromotionCibleId(promoCible.id);
            } catch (err) {
                console.error("Erreur chargement promotions :", err);
            }
        };

        fetchPromotions();
    }, []);

    useEffect(() => {
        if (promotionCibleId) {
            const fetchEleves = async () => {
                try {
                    const res = await fetch(`http://localhost:5166/api/eleve/promotion/${promotionCibleId}`);
                    const data = await res.json();
                    setEleves(data);
                } catch (err) {
                    console.error("Erreur chargement élèves :", err);
                }
            };

            fetchEleves();
        }
    }, [promotionCibleId]);

    return (
        <div className="profil-container">
            <h2>Profils</h2>
            <div className="profil-grid">
                {eleves.map(eleve => (
                    <div className="profil-card" key={eleve.id} onClick={() => setEleveSelectionne(eleve)}>
                        <img src="/profil-placeholder.png" alt="profil" />
                        <span>{eleve.prenom}</span>
                    </div>
                ))}
            </div>

            {eleveSelectionne && (
                <div className="modal-overlay" onClick={() => setEleveSelectionne(null)}>
                    <div className="modal-content" onClick={e => e.stopPropagation()}>
                        <div className="modal-close" onClick={() => setEleveSelectionne(null)}>×</div>
                        <h3>{eleveSelectionne.prenom} {eleveSelectionne.nom}</h3>
                        <ul>
                            {eleveSelectionne.reponses && eleveSelectionne.reponses.length > 0 ? (
                                eleveSelectionne.reponses.map((rep, i) => (
                                    <li key={i}><strong>{rep.question}:</strong> {rep.reponse}</li>
                                ))
                            ) : (
                                <li>Aucune réponse disponible.</li>
                            )}
                        </ul>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Profil;

