import React, { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import "./Voeu.css";

const Voeu = () => {
    const navigate = useNavigate();
    const [userId, setUserId] = useState(null);
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
