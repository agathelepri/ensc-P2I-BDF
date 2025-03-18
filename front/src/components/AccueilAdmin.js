import React from 'react';
import { useNavigate } from 'react-router-dom';
import './AccueilAdmin.css';

const AccueilAdmin = () => {
    const navigate = useNavigate();

    return (
        <div className="admin-container">
            {/* <img src="/img/logo.png" alt="Logo Admin" className="admin-logo" /> */}
            <h1>Tableau de Bord Admin</h1>

            <button className="admin-button" onClick={() => navigate('/classement')}>Gestion des Classements</button>
            <button className="admin-button" onClick={() => navigate('/familles')}>Gérer les Familles</button>
            <button className="admin-button" onClick={() => navigate('/voeux')}>Gérer les Vœux</button>
            <button className="admin-button logout" onClick={() => { localStorage.clear(); navigate('/'); }}>Déconnexion</button>
        </div>
    );
};

export default AccueilAdmin;
