import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Connexion from './pages/Connexion';
import Accueil from './pages/Accueil';
import ClassementAdmin from './pages/ClassementAdmin';
import MatchingParrainFilleul from './pages/MatchingParrainFilleul';
import ProfilPage from './pages/Profil';
import Perle from './pages/MaPerleRare';
import QuestionnairePage from './pages/Questionnaire';
import GestionEtu from './pages/GestionEtudiants';
import AccueilAdminPage from './pages/AccueilAdmin';
import VoeuPage from './pages/Voeu';

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
        {/* Bloquer l'accès aux pages admin pour les non-admins */}
        <Route path="/accueilAdmin" element={isAdmin ? <AccueilAdminPage /> : <Navigate to="/accueil" />} />
        <Route path="/classement" element={isAdmin ? <ClassementAdmin /> : <Navigate to="/accueil" />} />
        <Route path="/etudiant" element={isAdmin ? <GestionEtu /> : <Navigate to="/accueil" />} />
        <Route path="/match" element={isAdmin ? <MatchingParrainFilleul /> : <Navigate to="/accueil" />} />
      </Routes>
    </Router>
  );
}

export default App;


