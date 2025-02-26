import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './FormulaireConnexion.css';

const FormulaireConnexion = () => {
    const navigate = useNavigate();
    const [identifiant, setIdentifiant] = useState('');
    const [motDePasse, setMotDePasse] = useState('');
    const [isFirstLogin, setIsFirstLogin] = useState(false); // Indique si c'est une première connexion

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            // Vérifier si l'utilisateur existe et s'il a déjà un mot de passe
            const checkResponse = await fetch('http://localhost:5166/api/eleve/check-user', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ login: identifiant })
            });

            if (!checkResponse.ok) {
                throw new Error(`Erreur du serveur: ${checkResponse.status} - ${checkResponse.statusText}`);
            }

            const checkData = await checkResponse.json();
            console.log("Réponse du serveur (Check User):", checkData);

            if (checkData.firstLogin) {
                // L'utilisateur doit définir un mot de passe
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
                    throw new Error(`Erreur du serveur: ${setPasswordResponse.status} - ${setPasswordResponse.statusText}`);
                }

                const setPasswordData = await setPasswordResponse.json();
                alert(setPasswordData.message || "Mot de passe enregistré avec succès !");

                // Stocke l'ID de l'utilisateur après la première connexion
                localStorage.setItem("userId", setPasswordData.userId);
                navigate('/accueil');
            } else {
                // Connexion normale
                const loginResponse = await fetch('http://localhost:5166/api/eleve/login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ login: identifiant, mdp: motDePasse })
                });

                if (!loginResponse.ok) {
                    throw new Error(`Erreur du serveur: ${loginResponse.status} - ${loginResponse.statusText}`);
                }

                const loginData = await loginResponse.json();
                console.log("Réponse du serveur (Login):", loginData);

                if (loginData.success) {
                    alert("Connexion réussie !");

                    // Stocke `userId` après connexion
                    localStorage.setItem("userId", loginData.userId);

                    navigate('/accueil');
                } else {
                    alert(loginData.error || "Mot de passe incorrect.");
                }
            }
        } catch (error) {
            console.error("Erreur attrapée:", error);
            alert(`Une erreur est survenue: ${error.message}`);
        }
    };

    return (
        <div className="connexion-container">
            <h2>Bienvenue</h2>
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
                    required={isFirstLogin} // Rend obligatoire si c'est la première connexion
                />
                <button type="submit">{isFirstLogin ? "Définir le mot de passe" : "Se connecter"}</button>
            </form>
        </div>
    );
};

export default FormulaireConnexion;
