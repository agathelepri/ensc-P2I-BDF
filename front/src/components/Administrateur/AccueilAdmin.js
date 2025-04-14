// Ce composant correspond à la page d’accueil de l’administrateur.
// Il propose un tableau de bord permettant d’accéder aux fonctionnalités clés : gestion du classement, des familles, des vœux et des étudiants.
// Il intègre également une fonction de déconnexion qui vide le localStorage.

import React from 'react';
import { useNavigate } from 'react-router-dom';
import './AccueilAdmin.css';

const AccueilAdmin = () => {
    const navigate = useNavigate();

    return (
        <div className="admin-container">
            {/* <img src="/img/logo.png" alt="Logo Admin" className="admin-logo" /> */}
            <h1>Tableau de Bord Admin</h1>

            <button className="admin-button" onClick={() => navigate('/classementAdmin')}>Gestion des Classements</button>
            <button className="admin-button" onClick={() => navigate('/familles')}>Gérer les Familles</button>
            <button className="admin-button" onClick={() => navigate('/match')}>Gérer les Vœux</button>
            <button className="admin-button" onClick={() => navigate('/etudiant')}>Gérer les étudiants</button>
            <button className="admin-button logout" onClick={() => { localStorage.clear(); navigate('/'); }}>Déconnexion</button>
        </div>
    );
};

export default AccueilAdmin;
