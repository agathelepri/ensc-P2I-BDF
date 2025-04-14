// Ce composant permet aux élèves de sélectionner leurs 3 vœux de parrain ou filleul.
// Il charge dynamiquement les promotions, filtre les options pour éviter les doublons et envoie les vœux au serveur.
// Les vœux sont envoyés via trois requêtes distinctes, une pour chaque choix.

import React, { useEffect, useState } from "react";
import "./Voeu.css";

const Voeu = () => {
    const [eleves, setEleves] = useState([]);
    const [userId, setUserId] = useState(null);
    const [userPromoId, setUserPromoId] = useState(null);
    const [targetPromoId, setTargetPromoId] = useState(null);
    const [voeu, setVoeu] = useState({
        choix1Id: "",
        choix2Id: "",
        choix3Id: ""
    });
    const [filteredEleves, setFilteredEleves] = useState({
        choix1: [],
        choix2: [],
        choix3: [],
    });

    useEffect(() => {
        const storedUserId = localStorage.getItem("userId");
        if (!storedUserId) {
            console.error("Aucun userId trouvé");
            return;
        }

        setUserId(parseInt(storedUserId));

        const fetchUser = async () => {
            try {
                const response = await fetch(`http://localhost:5166/api/eleve/${storedUserId}`);
                if (!response.ok) throw new Error("Erreur récupération élève connecté");
                const data = await response.json();
                setUserPromoId(data.promotionId);

                const promoRes = await fetch("http://localhost:5166/api/promotion");
                const promos = await promoRes.json();
                const sortedPromos = promos.sort((a, b) => b.annee - a.annee);
                const [promoMax, promoMin] = sortedPromos.slice(0, 2);
                setTargetPromoId(data.promotionId === promoMax.id ? promoMin.id : promoMax.id);
            } catch (err) {
                console.error(err);
            }
        };

        fetchUser();
    }, []);

    useEffect(() => {
        if (userPromoId !== null && targetPromoId !== null) {
            fetchEleves();
        }
    }, [userPromoId, targetPromoId]);

    const fetchEleves = async () => {
        try {
            const response = await fetch("http://localhost:5166/api/eleve");
            if (!response.ok) throw new Error(`Erreur: ${response.status}`);
            const data = await response.json();

            const autresEleves = data.filter(e =>
                e.promotionId === targetPromoId && e.id !== userId
            );

            setEleves(autresEleves);
            setFilteredEleves({
                choix1: autresEleves,
                choix2: autresEleves,
                choix3: autresEleves,
            });
        } catch (error) {
            console.error("Erreur récupération élèves :", error);
        }
    };

    const handleVoeuChange = (e) => {
        const { name, value } = e.target;
        setVoeu((prev) => ({
            ...prev,
            [name]: value,
        }));

        if (["choix1Id", "choix2Id", "choix3Id"].includes(name)) {
            const alreadySelected = {
                choix1Id: name === "choix1Id" ? value : voeu.choix1Id,
                choix2Id: name === "choix2Id" ? value : voeu.choix2Id,
                choix3Id: name === "choix3Id" ? value : voeu.choix3Id,
            };

            setFilteredEleves({
                choix1: eleves.filter(e => e.id !== parseInt(alreadySelected.choix2Id) && e.id !== parseInt(alreadySelected.choix3Id)),
                choix2: eleves.filter(e => e.id !== parseInt(alreadySelected.choix1Id) && e.id !== parseInt(alreadySelected.choix3Id)),
                choix3: eleves.filter(e => e.id !== parseInt(alreadySelected.choix1Id) && e.id !== parseInt(alreadySelected.choix2Id)),
            });
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const voeuxToSend = [
            { numVoeux: 1, eleveChoisiId: parseInt(voeu.choix1Id) },
            { numVoeux: 2, eleveChoisiId: parseInt(voeu.choix2Id) },
            { numVoeux: 3, eleveChoisiId: parseInt(voeu.choix3Id) }
        ];

        try {
            for (const v of voeuxToSend) {
                const response = await fetch("http://localhost:5166/api/voeu", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        eleveId: userId,
                        promotionId: userPromoId,
                        numVoeux: v.numVoeux,
                        eleveChoisiId: v.eleveChoisiId
                    })
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    throw new Error(errorText || "Erreur lors de l’envoi d’un voeu.");
                }
            }

            alert("Tes vœux ont bien été enregistrés !");
        } catch (error) {
            console.error("Erreur soumission :", error);
            alert("Erreur lors de l'enregistrement des voeux.");
        }
    };

    return (
        <div className="voeu-container">
            <h2>Formulaire de Vœux</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Choix 1 :</label>
                    <select name="choix1Id" value={voeu.choix1Id} onChange={handleVoeuChange} required>
                        <option value="">Sélectionnez</option>
                        {filteredEleves.choix1.map((e) => (
                            <option key={e.id} value={e.id}>
                                {e.prenom} {e.nom}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="form-group">
                    <label>Choix 2 :</label>
                    <select name="choix2Id" value={voeu.choix2Id} onChange={handleVoeuChange} required>
                        <option value="">Sélectionnez</option>
                        {filteredEleves.choix2.map((e) => (
                            <option key={e.id} value={e.id}>
                                {e.prenom} {e.nom}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="form-group">
                    <label>Choix 3 :</label>
                    <select name="choix3Id" value={voeu.choix3Id} onChange={handleVoeuChange} required>
                        <option value="">Sélectionnez</option>
                        {filteredEleves.choix3.map((e) => (
                            <option key={e.id} value={e.id}>
                                {e.prenom} {e.nom}
                            </option>
                        ))}
                    </select>
                </div>

                <button type="submit" className="voeu-button">Envoyer</button>
            </form>
        </div>
    );
};

export default Voeu;
