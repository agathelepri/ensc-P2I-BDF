import axios from 'axios';

const API_URL = 'http://localhost:5000/api'; // Assurez-vous que l'URL correspond à votre API

const api = {
    // Récupérer la liste des élèves
    getEleves: async () => {
        try {
            const response = await axios.get(API_URL);
            return response.data;
        } catch (error) {
            console.error('Erreur lors de la récupération des élèves:', error);
            throw error;
        }
    },

    // Récupérer un élève par son ID
    getEleve: async (id) => {
        try {
            const response = await axios.get(`${API_URL}/${id}`);
            return response.data;
        } catch (error) {
            console.error(`Erreur lors de la récupération de l'élève avec ID ${id}:`, error);
            throw error;
        }
    },

    // Ajouter un nouvel élève
    postEleve: async (eleve) => {
        try {
            const response = await axios.post(API_URL, eleve);
            return response.data;
        } catch (error) {
            console.error(`Erreur lors de l'ajout de l'élève:`, error);
            throw error;
        }
    },

    // Modifier un élève existant
    putEleve: async (id, eleve) => {
        try {
            await axios.put(`${API_URL}/${id}`, eleve);
        } catch (error) {
            console.error(`Erreur lors de la mise à jour de l'élève avec ID ${id}:`, error);
            throw error;
        }
    },

    // Supprimer un élève par son ID
    deleteEleve: async (id) => {
        try {
            await axios.delete(`${API_URL}/${id}`);
        } catch (error) {
            console.error(`Erreur lors de la suppression de l'élève avec ID ${id}:`, error);
            throw error;
        }
    }
};

export default api;
