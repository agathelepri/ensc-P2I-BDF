import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Connexion from './pages/Connexion';
import Accueil from './pages/Accueil';
import Questionnaire from './components/Questionnaire';
import Voeu from './components/Voeu';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Connexion/>} />
        <Route path="/accueil" element={<Accueil/>} />
        <Route path="/questionnaire" element={<Questionnaire />} />
        <Route path="/voeux" element={<Voeu />} />
      </Routes>
    </Router>
  );
}

export default App;

