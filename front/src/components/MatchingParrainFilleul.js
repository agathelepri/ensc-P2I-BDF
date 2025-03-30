import React, { useEffect, useState } from 'react';
import './MatchingParrainFilleul.css';

const MatchingParrainFilleul = () => {
    const [parrains, setParrains] = useState([]);
    const [filleuls, setFilleuls] = useState([]);
    const [matchs, setMatchs] = useState([]);
    const [nonMatchesParrains, setNonMatchesParrains] = useState([]);
    const [nonMatchesFilleuls, setNonMatchesFilleuls] = useState([]);

    useEffect(() => {
        const loadData = async () => {
            try {
                const [promo1, promo4] = await Promise.all([
                    fetch("http://localhost:5166/api/eleve/promotion/1").then(res => res.json()),
                    fetch("http://localhost:5166/api/eleve/promotion/4").then(res => res.json())
                ]);
                setParrains(promo1);
                setFilleuls(promo4);
            } catch (error) {
                console.error("Erreur lors du chargement des élèves :", error);
            }
        };

        loadData();
    }, []);

    const estDejaParrain = (parrainId, matches) =>
        matches.some(m => m.parrain.id === parrainId);

    const lancerMatching = () => {
        const resultats = [];
        const filleulsNonMatches = filleuls.map(f => ({ ...f }));
        const parrainsUtilisés = new Set();

        const tryMatch = (condition) => {
            filleulsNonMatches.forEach((filleul) => {
                if (!filleul.idEleveParrain) {
                    for (let parrain of parrains) {
                        if (!parrainsUtilisés.has(parrain.id) && condition(filleul, parrain)) {
                            filleul.idEleveParrain = parrain.id;
                            resultats.push({ parrain, filleul });
                            parrainsUtilisés.add(parrain.id);
                            break;
                        }
                    }
                }
            });
        };

        // Matching logique croisée : choix puis affinités
        tryMatch((f, p) => f.choix1 === p.choix1);
        tryMatch((f, p) => f.choix1 === p.choix2);
        tryMatch((f, p) => f.choix2 === p.choix1);
        tryMatch((f, p) => f.choix2 === p.choix2);
        tryMatch((f, p) => f.choix2 === p.choix3);
        tryMatch((f, p) => f.choix3 === p.choix2);
        tryMatch((f, p) => f.choix3 === p.choix3);
        tryMatch((f, p) => f.passeTemps === p.passeTemps || f.soiree === p.soiree);

        // Filleuls sans parrain
        const nonMatchesF = filleulsNonMatches.filter(f => !f.idEleveParrain);

        // Parrains qui n’ont pas été utilisés
        const nonMatchesP = parrains.filter(p => !parrainsUtilisés.has(p.id));

        setMatchs(resultats);
        setNonMatchesFilleuls(nonMatchesF);
        setNonMatchesParrains(nonMatchesP);
    };

    const publierMatchs = async () => {
        try {
            for (let match of matchs) {
                await fetch(`http://localhost:5166/api/eleve/${match.filleul.id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        ...match.filleul,
                        eleveParrainId: match.parrain.id
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

            {nonMatchesParrains.length > 0 && (
                <>
                    <h3>Parrains sans filleul :</h3>
                    <ul className="nonmatch-list">
                        {nonMatchesParrains.map(p => (
                            <li key={p.id}>{p.prenom} {p.nom}</li>
                        ))}
                    </ul>
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
        </div>
    );
};

export default MatchingParrainFilleul;

