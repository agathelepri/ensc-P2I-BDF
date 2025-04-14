// Ce fichier contient la configuration principale du routeur React.
// Il déclare les routes vers toutes les pages de l’application, en différenciant les accès admin et utilisateurs.
// Les routes admin sont protégées : seul un utilisateur avec le rôle 'admin' peut y accéder.

import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Connexion from './pages/Connexion';
import Accueil from './pages/Utilisateur/Accueil';
import MatchingParrainFilleul from './pages/Administrateur/MatchingParrainFilleul';
import ProfilPage from './pages/Utilisateur/Profil';
import Perle from './pages/Utilisateur/MaPerleRare';
import QuestionnairePage from './pages/Utilisateur/Questionnaire';
import GestionEtu from './pages/Administrateur/GestionEtudiants';
import AccueilAdminPage from './pages/Administrateur/AccueilAdmin';
import VoeuPage from './pages/Utilisateur/Voeu';
import ClassementPage from './pages/Utilisateur/Classement';
import ClassementAdminPage from './pages/Administrateur/ClassementAdmin';

function App() {
  const isAdmin = localStorage.getItem("role") === "admin";

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Connexion />} />
        <Route path="/accueil" element={<Accueil />} />
        <Route path="/questionnaire" element={<QuestionnairePage />} />
        <Route path="/voeux" element={<VoeuPage />} />
        <Route path="/perle-rare" element={<Perle />} />
        <Route path="/profil" element={<ProfilPage />} />
        <Route path="/classement" element={<ClassementPage />} />
        {/* Bloquer l'accès aux pages admin pour les non-admins */}
        <Route path="/accueilAdmin" element={isAdmin ? <AccueilAdminPage /> : <Navigate to="/accueil" />} />
        <Route path="/classementAdmin" element={isAdmin ? <ClassementAdminPage /> : <Navigate to="/accueil" />} />
        <Route path="/etudiant" element={isAdmin ? <GestionEtu /> : <Navigate to="/accueil" />} />
        <Route path="/match" element={isAdmin ? <MatchingParrainFilleul /> : <Navigate to="/accueil" />} />
      </Routes>
    </Router>
  );
}

export default App;


