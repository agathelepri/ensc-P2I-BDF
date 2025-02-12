import React from 'react';
import { useNavigate } from 'react-router-dom';
import './Acceuil.css';
//import logo from '../../public/img/NouveauLogo.png'; 

const Accueil = () => {
    const navigate = useNavigate();

    return (
        <div className="accueil-container">
           {/*  <img src={logo} alt="Logo" className="logo" /> */}
            <div className="menu">
                <button onClick={() => navigate('/questionnaire')}>Questionnaire</button>
                <button onClick={() => navigate('/voeux')}>Voeux</button>
                <button onClick={() => navigate('/perle-rare')}>Qui est ma perle rare ?</button>
                <button onClick={() => navigate('/profils')}>Profils</button>
                <button onClick={() => navigate('/classement')}>Classement</button>
                <button onClick={() => navigate('/histoire-famille')}>Histoire de famille</button>
            </div>
        </div>
    );
};

export default Accueil;
