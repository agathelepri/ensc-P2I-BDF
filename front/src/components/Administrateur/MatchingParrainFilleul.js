// Ce composant permet de lancer l’algorithme de matching entre parrains et filleuls.
// Il récupère les vœux, les questionnaires et les promotions, puis applique une stratégie de matching croisé et par affinité.
// L’administrateur peut ensuite publier les résultats, ce qui met à jour en base les relations parrains-filleuls et les familles.

import React, { useEffect, useState } from 'react';
import './MatchingParrainFilleul.css';

const MatchingParrainFilleul = () => {
    const [parrains, setParrains] = useState([]);
    const [filleuls, setFilleuls] = useState([]);
    const [voeux, setVoeux] = useState([]);
    const [questionnaires, setQuestionnaires] = useState([]);
    const [matchs, setMatchs] = useState([]);
    const [nonMatchesParrains, setNonMatchesParrains] = useState([]);
    const [nonMatchesFilleuls, setNonMatchesFilleuls] = useState([]);

    useEffect(() => {
        const loadData = async () => {
            try {
                const resPromos = await fetch("http://localhost:5166/api/promotion");
                const promotions = await resPromos.json();
                const sortedPromos = promotions.sort((a, b) => b.annee - a.annee);
                const [ promoFilleul, promoParrain] = sortedPromos.slice(0, 2);

                const [parrainsRes, filleulsRes, voeuxRes, questionnairesRes] = await Promise.all([
                    fetch(`http://localhost:5166/api/eleve/promotion/${promoParrain.id}`).then(res => res.json()),
                    fetch(`http://localhost:5166/api/eleve/promotion/${promoFilleul.id}`).then(res => res.json()),
                    fetch("http://localhost:5166/api/voeu").then(res => res.json()),
                    fetch("http://localhost:5166/api/questionnaire").then(res => res.json())
                ]);
            
                setParrains(parrainsRes);
                setFilleuls(filleulsRes);
                setVoeux(voeuxRes);
                setQuestionnaires(questionnairesRes);
            } catch (error) {
                console.error("Erreur lors du chargement des données :", error);
            }
        };

        loadData();
    }, []);

    const getQuestionnaire = (eleveId) =>
        questionnaires.find(q => q.eleveId === eleveId);

    const getVoeuxByEleve = (eleveId) =>
        voeux.filter(v => v.eleveId === eleveId).sort((a, b) => a.numVoeux - b.numVoeux);

    const getReciprocalVoeu = (filleulId, parrainId) => {
        return voeux.find(v => v.eleveId === parrainId && v.eleveChoisiId === filleulId);
    };

    const matchConditions = [
        [1, 1], [1, 2], [2, 1], [2, 2], [2, 3], [3, 2], [3, 3]
    ];

    const lancerMatching = () => {
        const resultats = [];
        const matchedFilleulIds = new Set();
        const matchedParrainIds = new Set();

        const tryMatch = (filleul, parrain) => {
            if (matchedFilleulIds.has(filleul.id) || matchedParrainIds.has(parrain.id)) return false;
            resultats.push({ parrain, filleul });
            matchedFilleulIds.add(filleul.id);
            matchedParrainIds.add(parrain.id);
            return true;
        };

        // Étape 1 : Matching par vœux croisés selon priorité
        filleuls.forEach(f => {
            const voeuxF = getVoeuxByEleve(f.id);
            for (let voeuF of voeuxF) {
                const p = parrains.find(p => p.id === voeuF.eleveChoisiId);
                if (!p) continue;
                const voeuP = getReciprocalVoeu(f.id, p.id);
                if (!voeuP) continue;
                const condition = matchConditions.find(([vf, vp]) => voeuF.numVoeux === vf && voeuP.numVoeux === vp);
                if (condition) {
                    tryMatch(f, p);
                    break;
                }
            }
        });

        // Étape 2 : Matching par affinité questionnaire (soiree + passeTemps)
        filleuls.forEach(f => {
            if (matchedFilleulIds.has(f.id)) return;
            const qF = getQuestionnaire(f.id);
            if (!qF) return;
            for (let p of parrains) {
                if (matchedParrainIds.has(p.id)) continue;
                const qP = getQuestionnaire(p.id);
                if (!qP) continue;
                if (qF.soiree === qP.soiree && qF.passeTemps === qP.passeTemps) {
                    if (tryMatch(f, p)) break;
                }
            }
        });

        // Étape 3 : Matching par passeTemps
        filleuls.forEach(f => {
            if (matchedFilleulIds.has(f.id)) return;
            const qF = getQuestionnaire(f.id);
            if (!qF) return;
            for (let p of parrains) {
                if (matchedParrainIds.has(p.id)) continue;
                const qP = getQuestionnaire(p.id);
                if (!qP) continue;
                if (qF.passeTemps === qP.passeTemps) {
                    if (tryMatch(f, p)) break;
                }
            }
        });

        // Étape 4 : Matching par soiree
        filleuls.forEach(f => {
            if (matchedFilleulIds.has(f.id)) return;
            const qF = getQuestionnaire(f.id);
            if (!qF) return;
            for (let p of parrains) {
                if (matchedParrainIds.has(p.id)) continue;
                const qP = getQuestionnaire(p.id);
                if (!qP) continue;
                if (qF.soiree === qP.soiree) {
                    if (tryMatch(f, p)) break;
                }
            }
        });

        const nonMatchesF = filleuls.filter(f => !matchedFilleulIds.has(f.id));
        const nonMatchesP = parrains.filter(p => !matchedParrainIds.has(p.id));

        setMatchs(resultats);
        setNonMatchesFilleuls(nonMatchesF);
        setNonMatchesParrains(nonMatchesP);
    };

    console.log(matchs);

    const publierMatchs = async () => {
        try {
            for (let match of matchs) {
                console.log(match.parrain.id);
                await fetch(`http://localhost:5166/api/eleve/${match.filleul.id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        ...match.filleul,
                        EleveParrainId: match.parrain.id,
                        familleId: match.parrain.familleId 
                    })
                });
            }
            alert("Appariements publiés avec succès !");
        } catch (error) {
            console.error("Erreur lors de la publication :", error);
            alert("Erreur pendant la publication des matchs.");
            window.location.reload();
        }
    };

    return (
        <div className="matching-container">
            <h2>Matching Parrain / Filleul</h2>

            <button className="matching-button" onClick={lancerMatching}>
                Faire les matchs
            </button>

            {matchs.length > 0 && (
                <>
                    <h3>Matchs réalisés :</h3>
                    <table className="matching-table">
                        <thead>
                            <tr>
                                <th>Parrain</th>
                                <th>Filleul</th>
                            </tr>
                        </thead>
                        <tbody>
                            {matchs.map((match, index) => (
                                <tr key={index}>
                                    <td>{match.parrain.prenom} {match.parrain.nom}</td>
                                    <td>{match.filleul.prenom} {match.filleul.nom}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>

                    <button className="matching-button" onClick={publierMatchs}>
                        Publier les résultats
                    </button>
                </>
            )}

            {nonMatchesFilleuls.length > 0 && (
                <>
                    <h3>Filleuls sans parrain :</h3>
                    <ul className="nonmatch-list">
                        {nonMatchesFilleuls.map(f => (
                            <li key={f.id}>{f.prenom} {f.nom}</li>
                        ))}
                    </ul>
                </>
            )}

            {nonMatchesParrains.length > 0 && (
                <>
                    <h3> Parrains sans filleul :</h3>
                    <ul className="nonmatch-list">
                        {nonMatchesParrains.map(p => (
                            <li key={p.id}>{p.prenom} {p.nom}</li>
                        ))}
                    </ul>
                </>
            )}
        </div>
    );
};

export default MatchingParrainFilleul;