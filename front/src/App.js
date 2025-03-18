import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Connexion from './pages/Connexion';
import Accueil from './pages/Accueil';
import Questionnaire from './components/Questionnaire';
import Voeu from './components/Voeu';
import AccueilAdmin from './components/AccueilAdmin';
import ClassementAdmin from './components/ClassementAdmin';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Connexion/>} />
        <Route path="/accueil" element={<Accueil/>} />
        <Route path="/questionnaire" element={<Questionnaire />} />
        <Route path="/voeux" element={<Voeu />} />
        <Route path="/accueilAdmin" element={<AccueilAdmin />} />
        <Route path="/classement" element={<ClassementAdmin />} />
      </Routes>
    </Router>
  );
}

export default App;

