// Ce composant permet à un élève de remplir son questionnaire de présentation.
// Les questions sont affichées 2 par 2 avec un système de navigation par page et une barre de progression.
// Les données sont formatées et envoyées au backend à la soumission.

import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./Questionnaire.css";

const questions = [
    { name: "provenance", label: "D'où viens-tu ?" },
    { name: "astro", label: "Quel est ton signe astrologique ?" },
    { name: "boisson", label: "Ta boisson préférée ?" },
    { name: "soiree", label: "Ta soirée idéale ?", options: ["Apéro chill avec 3/4 potes", "Boite jusqu’à 6h", "Netflix dodo", "Le train pour revoir la mif"] },
    { name: "son", label: "Ton son du moment ?" },
    { name: "livre", label: "Ton livre préféré ?" },
    { name: "film", label: "Ton film préféré ?" },
    { name: "passeTemps", label: "Que préfères-tu comme passe-temps ?", options: ["Sport", "Art", "Chillance", "Fête", "Travail"] },
    { name: "defaut", label: "Ton plus grand défaut ?" },
    { name: "qualite", label: "Ta plus grande qualité ?" },
    { name: "relation", label: "Quelle relation cherches-tu ?" },
    { name: "preference", label: "Tu préfères ?", options: ["Garçon", "Fille", "Peu importe"] },
];

const Questionnaire = () => {
    const navigate = useNavigate();
    const [userId, setUserId] = useState(null);
    const [currentPage, setCurrentPage] = useState(0);
    const [formData, setFormData] = useState(questions.reduce((acc, q) => ({
        ...acc, [q.name]: q.options ? [] : ""
    }), {}));

    useEffect(() => {
        const storedUserId = localStorage.getItem("userId");
        console.log("User ID récupéré du localStorage :", storedUserId);
        if (!storedUserId) {
            alert("Vous devez être connecté pour accéder au questionnaire.");
            navigate("/");
        } else {
            setUserId(parseInt(storedUserId)); // Assure que `userId` est bien un `int`
        }
    }, [navigate]);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === "checkbox"
                ? (checked ? [...prev[name], value] : prev[name].filter(v => v !== value))
                : value
        }));
    };

    const nextPage = () => {
        if (isPageValid()) {
            setCurrentPage((prev) => prev + 1);
        } else {
            alert("Veuillez remplir toutes les questions avant de passer à la suivante.");
        }
    };

    const prevPage = () => setCurrentPage((prev) => prev - 1);

    const isLastPage = currentPage === Math.ceil(questions.length / 2) - 1;

    const isPageValid = () => {
        const start = currentPage * 2;
        const end = start + 2;
        return questions.slice(start, end).every(q => {
            const value = formData[q.name];
            return Array.isArray(value) ? value.length > 0 : value.trim() !== "";
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!isPageValid()) {
            alert("Veuillez remplir toutes les questions avant de valider.");
            return;
        }

        const formattedData = Object.keys(formData).reduce((acc, key) => {
            acc[key] = Array.isArray(formData[key]) ? formData[key].join(", ") : formData[key];
            return acc;
        }, {});

        try {
            const response = await fetch("http://localhost:5166/api/questionnaire", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    ...formattedData,
                    eleveId: userId, 
                }),
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Erreur serveur: ${response.status} - ${errorText}`);
            }

            alert("Questionnaire soumis avec succès !");
            navigate("/accueil");
        } catch (error) {
            console.error("Erreur lors de l'envoi du questionnaire :", error);
            alert("Une erreur est survenue : " + error.message);
        }
    };

    const progress = ((currentPage + 1) / Math.ceil(questions.length / 2)) * 100;

    const renderQuestions = () => {
        const start = currentPage * 2;
        const end = start + 2;
        return questions.slice(start, end).map((q, index) => (
            <div className="question-section" key={index}>
                <label>{q.label}</label>
                {q.options ? (
                    <div className="checkbox-group">
                        {q.options.map((option, idx) => (
                            <label key={idx} className="checkbox-label">
                                <input
                                    type="checkbox"
                                    name={q.name}
                                    value={option}
                                    checked={formData[q.name].includes(option)}
                                    onChange={handleChange}
                                />
                                {option}
                            </label>
                        ))}
                    </div>
                ) : (
                    <input type="text" name={q.name} value={formData[q.name]} onChange={handleChange} required />
                )}
            </div>
        ));
    };

    return (
        <div className="questionnaire-page">
            <h2>Questionnaire</h2>
            <h3>À propos de toi</h3>
            <div className="progress-bar">
                <div className="progress" style={{ width: `${progress}%` }}></div>
            </div>

            <form onSubmit={handleSubmit} className="questionnaire-form">
                {renderQuestions()}

                <div className="button-group">
                    {currentPage > 0 && <button type="button" className="prev-btn" onClick={prevPage}>Précédent</button>}
                    {isLastPage ? (
                        <button type="submit" className="submit-btn">Valider</button>
                    ) : (
                        <button type="button" className="next-btn" onClick={nextPage}>Suivant</button>
                    )}
                </div>
            </form>
        </div>
    );
};

export default Questionnaire;
