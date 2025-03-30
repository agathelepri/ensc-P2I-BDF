import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Connexion from './pages/Connexion';
import Accueil from './pages/Accueil';
import Questionnaire from './components/Questionnaire';
import Voeu from './components/Voeu';
import AccueilAdmin from './components/AccueilAdmin';
import ClassementAdmin from './components/ClassementAdmin';
import GestionEtudiant from './components/GestionEtudiant';
import MatchingParrainFilleul from './components/MatchingParrainFilleul';
import MaPerleRare from './components/MaPerleRare';

function App() {
  const isAdmin = localStorage.getItem("role") === "admin";

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Connexion />} />
        <Route path="/accueil" element={<Accueil />} />
        <Route path="/questionnaire" element={<Questionnaire />} />
        <Route path="/voeux" element={<Voeu />} />
        <Route path="/perle-rare" element={<MaPerleRare />} />
        {/* Bloquer l'accès aux pages admin pour les non-admins */}
        <Route path="/accueilAdmin" element={isAdmin ? <AccueilAdmin /> : <Navigate to="/accueil" />} />
        <Route path="/classement" element={isAdmin ? <ClassementAdmin /> : <Navigate to="/accueil" />} />
        <Route path="/etudiant" element={isAdmin ? <GestionEtudiant /> : <Navigate to="/accueil" />} />
        <Route path="/match" element={isAdmin ? <MatchingParrainFilleul /> : <Navigate to="/accueil" />} />
      </Routes>
    </Router>
  );
}

export default App;


