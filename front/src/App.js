import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Connexion from './pages/Connexion';
import Accueil from './pages/Accueil';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Connexion/>} />
        <Route path="/accueil" element={<Accueil/>} />
      </Routes>
    </Router>
  );
}

export default App;

