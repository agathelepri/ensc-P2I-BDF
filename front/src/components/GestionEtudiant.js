
import React, { useState, useEffect } from 'react';
import './GestionEtudiants.css';

const GestionEtudiant = () => {
    const [promotions, setPromotions] = useState([]);
    const [selectedPromo, setSelectedPromo] = useState(null);
    const [eleves, setEleves] = useState([]);
    const [isEditing, setIsEditing] = useState(null);
    const [editedEleve, setEditedEleve] = useState({});
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 10;

    useEffect(() => {
        const fetchPromotions = async () => {
            try {
                const response = await fetch("http://localhost:5166/api/promotion");
                if (!response.ok) throw new Error(`Erreur serveur: ${response.status}`);
                const data = await response.json();
                setPromotions(data);
            } catch (error) {
                console.error("Erreur:", error);
            }
        };
        fetchPromotions();
    }, []);

    useEffect(() => {
        if (selectedPromo) {
            const fetchEleves = async () => {
                try {
                    const response = await fetch(`http://localhost:5166/api/eleve/promotion/${selectedPromo}`);
                    if (!response.ok) throw new Error("Erreur lors du chargement des élèves");
                    const data = await response.json();
                    setEleves(data);
                    setCurrentPage(1); // reset page
                } catch (error) {
                    console.error("Erreur:", error);
                }
            };
            fetchEleves();
        }
    }, [selectedPromo]);

    const handleEditClick = (eleve) => {
        setIsEditing(eleve.id);
        setEditedEleve(eleve);
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setEditedEleve({
            ...editedEleve,
            [name]: name === "eleveParrainId" ? parseInt(value) : value
        });
    };

    const handleSave = async (id) => {
        try {
            const response = await fetch(`http://localhost:5166/api/eleve/${id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(editedEleve)
            });
            if (!response.ok) throw new Error("Erreur lors de la mise à jour");
            setEleves(eleves.map(et => (et.id === id ? editedEleve : et)));
            setIsEditing(null);
        } catch (error) {
            console.error("Erreur:", error);
        }
    };

    const indexOfLast = currentPage * itemsPerPage;
    const indexOfFirst = indexOfLast - itemsPerPage;
    const elevesAffiches = eleves.slice(indexOfFirst, indexOfLast);

    return (
        <div className="gestion-etudiant-container">
            <h2>Gestion des Étudiants</h2>
            <div className="promo-tabs">
                {promotions.map((promo) => (
                    <button
                        key={promo.id}
                        className={`promo-tab ${selectedPromo === promo.id ? "active" : ""}`}
                        onClick={() => {
                            setSelectedPromo(promo.id);
                            setCurrentPage(1);
                            setTimeout(() => {
                                const list = document.querySelector(".eleve-list");
                                if (list) list.scrollIntoView({ behavior: "smooth", block: "start" });
                            }, 100);
                        }}
                    >
                        Promotion {promo.annee}
                    </button>
                ))}
            </div>
            {selectedPromo && (
                <div className="eleve-list">
                    <h3>Élèves de la promotion {selectedPromo}</h3>
                    <div className="etudiants-scroll">
                        <table>
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Nom</th>
                                    <th>Prénom</th>
                                    <th>Login</th>
                                    <th>{selectedPromo === 1 ? "Filleuls" : "Parrain"}</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {elevesAffiches.map((eleve) => (
                                    <tr key={eleve.id}>
                                        <td>{eleve.id}</td>
                                        <td>{isEditing === eleve.id ? (
                                            <input type="text" name="nom" value={editedEleve.nom} onChange={handleChange} />
                                        ) : eleve.nom}</td>
                                        <td>{isEditing === eleve.id ? (
                                            <input type="text" name="prenom" value={editedEleve.prenom} onChange={handleChange} />
                                        ) : eleve.prenom}</td>
                                        <td>{isEditing === eleve.id ? (
                                            <input type="text" name="login" value={editedEleve.login} onChange={handleChange} />
                                        ) : eleve.login}</td>
                                        <td>{selectedPromo === 1 ? (
                                            Array.isArray(eleve.affichage) && eleve.affichage.length > 0 ? (
                                                <ul>{eleve.affichage.map((f, index) => (
                                                    <li key={index}>{f.prenom} {f.nom}</li>
                                                ))}</ul>
                                            ) : "—"
                                        ) : isEditing === eleve.id ? (
                                            <select name="eleveParrainId" value={editedEleve.eleveParrainId || 0} onChange={handleChange}>
                                                <option value={0}>Aucun</option>
                                                {eleves.map((e) => (
                                                    e.id !== eleve.id && (
                                                        <option key={e.id} value={e.id}>{e.prenom} {e.nom}</option>
                                                    )
                                                ))}
                                            </select>
                                        ) : (
                                            eleve.affichage ? `${eleve.affichage.prenom} ${eleve.affichage.nom}` : "—"
                                        )}</td>
                                        <td>{isEditing === eleve.id ? (
                                            <button onClick={() => handleSave(eleve.id)}>Sauvegarder</button>
                                        ) : (
                                            <button onClick={() => handleEditClick(eleve)}>Modifier</button>
                                        )}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                        <div className="pagination">
                            {Array.from({ length: Math.ceil(eleves.length / itemsPerPage) }, (_, i) => (
                                <button key={i + 1} onClick={() => setCurrentPage(i + 1)} className={currentPage === i + 1 ? "active" : ""}>
                                    {i + 1}
                                </button>
                            ))}
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default GestionEtudiant;
