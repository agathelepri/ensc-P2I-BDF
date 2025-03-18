import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Accueil from './pages/Accueil';
import AccueilAdmin from './pages/AccueilAdmin';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/accueil" element={<Accueil />} />
                <Route path="/accueilAdmin" element={<AccueilAdmin />} />
            </Routes>
        </Router>
    );
}

export default App;
