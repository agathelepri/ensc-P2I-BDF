import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './FormulaireConnexion.css';

const FormulaireConnexion = () => {
    const navigate = useNavigate();
    const [identifiant, setIdentifiant] = useState('');
    const [motDePasse, setMotDePasse] = useState('');
    const [isFirstLogin, setIsFirstLogin] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            console.log("Tentative de connexion avec", identifiant);

            // Vérifie si l'utilisateur existe
            const checkResponse = await fetch('http://localhost:5166/api/eleve/check-user', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ login: identifiant })
            });

            if (!checkResponse.ok) {
                throw new Error(`Erreur serveur: ${checkResponse.status}`);
            }

            const checkData = await checkResponse.json();
            console.log("Réponse du serveur (Check User):", checkData);

            if (checkData.firstLogin) {
                setIsFirstLogin(true);

                if (!motDePasse) {
                    alert("Première connexion détectée. Veuillez saisir un mot de passe.");
                    return;
                }

                // Enregistrer le mot de passe
                const setPasswordResponse = await fetch('http://localhost:5166/api/eleve/set-password', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ login: identifiant, mdp: motDePasse })
                });

                if (!setPasswordResponse.ok) {
                    throw new Error(`Erreur serveur: ${setPasswordResponse.status}`);
                }

                const setPasswordData = await setPasswordResponse.json();
                alert(setPasswordData.message || "Mot de passe enregistré avec succès !");

                localStorage.setItem("userId", setPasswordData.userId);
                navigate(identifiant.toLowerCase() === "admin" ? "/accueilAdmin" : "/accueil");
            } else {
                // Connexion normale
                const loginResponse = await fetch('http://localhost:5166/api/eleve/login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ login: identifiant, mdp: motDePasse })
                });

                if (!loginResponse.ok) {
                    throw new Error(`Erreur serveur: ${loginResponse.status}`);
                }

                const loginData = await loginResponse.json();
                console.log("Réponse du serveur (Login):", loginData);

                if (loginData.success) {
                    alert("Connexion réussie !");
                    localStorage.setItem("userId", loginData.userId);
                    localStorage.setItem("role", loginData.role);

                    navigate(loginData.role === "admin" ? "/accueilAdmin" : "/accueil");
                } else {
                    setErrorMessage(loginData.error || "Mot de passe incorrect.");
                }
            }
        } catch (error) {
            console.error("Erreur attrapée:", error);
            setErrorMessage("Une erreur est survenue, veuillez réessayer.");
        }
    };

    return (
        <div className="connexion-container">
            <h2>Bienvenue</h2>
            {errorMessage && <p className="error-message">{errorMessage}</p>}
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    placeholder="Identifiant"
                    value={identifiant}
                    onChange={(e) => setIdentifiant(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder={isFirstLogin ? "Définir un mot de passe" : "Mot de passe"}
                    value={motDePasse}
                    onChange={(e) => setMotDePasse(e.target.value)}
                    required={isFirstLogin}
                />
                <button type="submit">{isFirstLogin ? "Définir le mot de passe" : "Se connecter"}</button>
            </form>
        </div>
    );
};

export default FormulaireConnexion;


