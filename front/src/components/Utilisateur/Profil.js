// Ce composant affiche tous les profils de la promotion opposée à celle de l’utilisateur.
// Chaque profil peut être consulté en détail avec les réponses au questionnaire.
// Cette interface aide à mieux choisir ses vœux ou à découvrir ses potentiels binômes.

import React, { useEffect, useState } from 'react';
import './Profil.css';

const Profil = () => {
    const [promoUser, setPromoUser] = useState(null);
    const [promoCible, setPromoCible] = useState(null);
    const [eleves, setEleves] = useState([]);
    const [questionnaires, setQuestionnaires] = useState([]);
    const [profilSelectionne, setProfilSelectionne] = useState(null);

    useEffect(() => {
        const loadData = async () => {
            try {
                const userId = JSON.parse(localStorage.getItem("userId"));
                const userRes = await fetch(`http://localhost:5166/api/eleve/${userId}`);
                const userData = await userRes.json();

                const promos = await fetch("http://localhost:5166/api/promotion").then(r => r.json());
                const sorted = promos.sort((a, b) => b.annee - a.annee);
                const [promo1, promo2] = sorted.slice(0, 2);

                const promoCourante = userData.promotion;
                const promoAutre = promoCourante.id === promo1.id ? promo2 : promo1;

                const [elevesRes, questionnairesRes] = await Promise.all([
                    fetch(`http://localhost:5166/api/eleve/promotion/${promoAutre.id}`).then(r => r.json()),
                    fetch("http://localhost:5166/api/questionnaire").then(r => r.json()),
                ]);

                setPromoUser(promoCourante);
                setPromoCible(promoAutre);
                setEleves(elevesRes);
                setQuestionnaires(questionnairesRes);
            } catch (err) {
                console.error("Erreur chargement profil:", err);
            }
        };

        loadData();
    }, []);

    const getQuestionnaire = (eleveId) => questionnaires.find(q => q.eleveId === eleveId);

    return (
        <div className="profil-container">
            <h2 className="profil-titre">Profils de la promotion {promoCible?.annee}</h2>

            {!profilSelectionne ? (
                <div className="profil-grille">
                    {eleves.map((e) => (
                        <button
                            key={e.id}
                            className="profil-carte"
                            onClick={() => setProfilSelectionne(e)}
                        >
                            <span className="profil-nom">{e.prenom}</span>
                        </button>
                    ))}
                </div>
            ) : (
                <div className="profil-detail">
                    <button onClick={() => setProfilSelectionne(null)} className="profil-retour">←</button>
                    <div className="flex items-center mb-4">
                        {/* <img
                            src={`data:image/jpeg;base64,${profilSelectionne.photo}`}
                            alt="avatar"
                            className="profil-detail-avatar"
                        /> */}
                        <div>
                            <h3 className="text-lg font-semibold">{profilSelectionne.prenom} {profilSelectionne.nom}</h3>
                        </div>
                    </div>
                    <ul className="profil-info">
                        {(() => {
                            const q = getQuestionnaire(profilSelectionne.id);
                            if (!q) return <li>Données indisponibles</li>;
                            return (
                                <>
                                    <li>Ville : {q.provenance}</li>
                                    <li>Signe astro : {q.astro}</li>
                                    <li>Cocktail : {q.boisson}</li>
                                    <li>Soirée idéale : {q.soiree}</li>
                                    <li>Son : {q.son}</li>
                                    <li>Livre : {q.livre}</li>
                                    <li>Film : {q.film}</li>
                                    <li>Passe-temps : {q.passeTemps}</li>
                                    <li>Défaut : {q.defaut}</li>
                                    <li>Qualité : {q.qualite}</li>
                                    <li>Relation : {q.relation}</li>
                                    <li>Préférence : {q.preference}</li>
                                </>
                            );
                        })()}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default Profil;