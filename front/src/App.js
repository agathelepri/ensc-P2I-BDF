import React from 'react'
import {BrowserRouter as Router, Routes, Route} from 'react-router-dom'
import Accueil from './pages/Accueil'
import Connexion from './pages/Connexion'


 const App = () => {
  return (
    <Router>
      <Routes>
        <Route path='/' element={<Accueil/>}/>
        <Route path='/login' element={<Connexion/>} />

      </Routes>
    </Router>
  )
}

export default App
