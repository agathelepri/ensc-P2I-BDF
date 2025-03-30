/* import React, { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import "./Voeu.css";

const Voeu = () => {
    const navigate = useNavigate();
    const [userId, setUserId] = useState(null);
    const [userPromoId, setUserPromoId] = useState(null);
    const [eleves, setEleves] = useState([]); // Liste des élèves récupérée depuis l'API
    const [voeux, setVoeux] = useState({ choix1: null, choix2: null, choix3: null });
    const [filteredEleves, setFilteredEleves] = useState({ choix1: [], choix2: [], choix3: [] });
    const [showDropdown, setShowDropdown] = useState({ choix1: false, choix2: false, choix3: false });
    const voeuRefs = { choix1: useRef(null), choix2: useRef(null), choix3: useRef(null) };

    useEffect(() => {
        const storedUserId = localStorage.getItem("userId");
        if (!storedUserId) {
            alert("Vous devez être connecté pour soumettre vos vœux.");
            navigate("/");
        } else {
            setUserId(parseInt(storedUserId));
            // Récupérer les infos de l'élève connecté
            const fetchUser = async () => {
                try {
                    const response = await fetch(`http://localhost:5166/api/eleve/${storedUserId}`);
                    if (!response.ok) throw new Error("Erreur lors de la récupération de l'élève connecté");
                    const data = await response.json();
                    setUserPromoId(data.promotionId);
                } catch (err) {
                    console.error(err);
                }
            };

            fetchUser();
            fetchEleves();
        }

        document.addEventListener("click", handleClickOutside);
        return () => document.removeEventListener("click", handleClickOutside);
    }, [navigate]);

    // Récupère la liste des élèves depuis l'API
    const fetchEleves = async () => {
        try {
            const response = await fetch("http://localhost:5166/api/eleve");
            if (!response.ok) throw new Error(`Erreur: ${response.status}`);
            const data = await response.json();
            setEleves(data);
            setFilteredEleves({ choix1: data, choix2: data, choix3: data });
        } catch (error) {
            console.error("Erreur lors de la récupération des élèves :", error);
        }
    };

    // Ferme le menu déroulant si l'utilisateur clique en dehors
    const handleClickOutside = (event) => {
        Object.keys(voeuRefs).forEach((choix) => {
            if (voeuRefs[choix].current && !voeuRefs[choix].current.contains(event.target)) {
                setShowDropdown((prev) => ({ ...prev, [choix]: false }));
            }
        });
    };

    // Gère la recherche dynamique
    const handleSearch = (e) => {
        const { name, value } = e.target;
        setVoeux((prev) => ({ ...prev, [name]: value }));
        setFilteredEleves((prev) => ({
            ...prev,
            [name]: eleves.filter((eleve) =>
                (`${eleve.prenom} ${eleve.nom}`).toLowerCase().includes(value.toLowerCase())
            ),
        }));
    };

    // Ouvre le menu déroulant au clic
    const handleFocus = (name) => {
        setShowDropdown((prev) => ({ ...prev, [name]: true }));
        setFilteredEleves((prev) => ({ ...prev, [name]: eleves }));
    };

    // Sélectionner un élève depuis le menu déroulant
    const handleSelect = (name, id) => {
        setVoeux((prev) => ({ ...prev, [name]: id }));
        setShowDropdown((prev) => ({ ...prev, [name]: false }));
    };

    // Vérifie que tous les choix sont remplis avant validation
    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!voeux.choix1 || !voeux.choix2 || !voeux.choix3) {
            alert("Veuillez sélectionner trois élèves avant de valider.");
            return;
        }

        const voeuxData = [
            { eleve: userId, numVoeux: 1, promotion: 1, eleveChoisi: voeux.choix1 },
            { eleve: userId, numVoeux: 2, promotion: 1, eleveChoisi: voeux.choix2 },
            { eleve: userId, numVoeux: 3, promotion: 1, eleveChoisi: voeux.choix3 },
        ];

        console.log("Données envoyées :", JSON.stringify(voeuxData, null, 2)); // Vérification des données

        try {
            for (const voeu of voeuxData) {
                const response = await fetch("http://localhost:5166/api/voeu", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(voeu),
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    throw new Error(`Erreur: ${response.status} - ${errorText}`);
                }
            }

            alert("Vos vœux ont été enregistrés avec succès !");
            navigate("/accueil");
        } catch (error) {
            console.error("Erreur lors de l'enregistrement des vœux :", error);
            alert("Une erreur est survenue, veuillez réessayer.");
        }
    };

    return (
        <div className="voeu-container">
            <h2>Voeux</h2>
            <form onSubmit={handleSubmit}>
                {["choix1", "choix2", "choix3"].map((choix, index) => (
                    <div key={choix} className="autocomplete" ref={voeuRefs[choix]}>
                        <input
                            type="text"
                            name={choix}
                            placeholder={`Choix ${index + 1}`}
                            value={voeux[choix] ? `${eleves.find(e => e.id === voeux[choix])?.prenom} ${eleves.find(e => e.id === voeux[choix])?.nom}` : ""}
                            onChange={handleSearch}
                            onFocus={() => handleFocus(choix)}
                            autoComplete="off"
                        />
                        {showDropdown[choix] && filteredEleves[choix].length > 0 && (
                            <ul className="autocomplete-list">
                                {filteredEleves[choix].map((eleve) => (
                                    <li key={eleve.id} onClick={() => handleSelect(choix, eleve.id)}>
                                        {eleve.prenom} {eleve.nom}
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>
                ))}
                <button type="submit">Valider</button>
            </form>
        </div>
    );
};

export default Voeu;
 */
import React, { useEffect, useState } from "react";
import "./Voeu.css";

const Voeu = () => {
    const [eleves, setEleves] = useState([]);
    const [userId, setUserId] = useState(null);
    const [userPromoId, setUserPromoId] = useState(null);
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
            } catch (err) {
                console.error(err);
            }
        };

        fetchUser();
    }, []);

    useEffect(() => {
        if (userPromoId !== null) {
            fetchEleves();
        }
    }, [userPromoId]);

    const fetchEleves = async () => {
        try {
            const response = await fetch("http://localhost:5166/api/eleve");
            if (!response.ok) throw new Error(`Erreur: ${response.status}`);
            const data = await response.json();

            const autresEleves = data.filter(e =>
                e.promotionId !== userPromoId && e.id !== userId
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
