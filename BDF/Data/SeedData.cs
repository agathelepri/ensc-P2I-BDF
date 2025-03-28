using Microsoft.EntityFrameworkCore;

namespace BDF.Data;
public static class SeedData
{
    // Test data for part 1 and 2
    public static void Init()
    {
        using var context = new DataContext();
                /*Promotion premiereA = new Promotion(1)
        {
            Id = 1,
            Annee = 1,
        };
        Promotion deuxiemeA = new Promotion(2)
        {
            Id = 2,
            Annee = 2,
        };
        Promotion troisiemeA = new Promotion(3)
        {
            Id = 3,
            Annee = 3,
        };
        context.Promotions.AddRange(
            premiereA,
            deuxiemeA,
            troisiemeA
        ); 

        // Add eleve
        Eleve leGuillou = new Eleve ("LeGuillou", "Lise", "lleguillou", 3)
        {
            Id = 1,
            Nom = "LeGuillou",
            Prenom = "Lise",
            Login = "lleguillou",
            //Promotion = 3 ,
            //Famille=1,
            Photo=[],

        };

        Eleve lepri = new()
        {
            Id = 2 ,
            Nom = "Lepri",
            Prenom = "Agathe",
            Login = "alepri",
            MDP ="Bonjour",
            //Promotion =2,
            //Famille=1,
            Photo=[],
            //EleveParrain=1,
        };

        Eleve tchoua = new()
        {
            Id = 3,
            Nom = "Tchoua",
            Prenom = "Noémie",
            Login = "ntchoua",
            //Promotion =1,
            //Famille = 1,
            Photo=[],
            //EleveParrain = 2,
        };

        context.Eleves.AddRange(
            lepri,
            leGuillou,
            tchoua
        );*/

        Famille bleu = new()
        {
            Id = 1,
            Nom = "Bleu",
            CouleurHexa = "627AE5"
        };
         Famille rouge = new()
        {
            Id = 2,
            Nom = "Rouge",
            CouleurHexa = "FF3130"
        };
        Famille vert = new()
        {
            Id = 3,
            Nom = "Vert",
            CouleurHexa = "03BF62"
        };
        Famille orange = new()
        {
            Id = 4,
            Nom = "Orange",
            CouleurHexa = "FF914D"
        };
        Famille jaune = new()
        {
            Id = 5,
            Nom = "Jaune",
            CouleurHexa = "FFDE59"
        };   
        context.Familles.AddRange(
            bleu,
            rouge,
            vert,
            jaune,
            orange
        );   


        /*Questionnaire alepri = new()
        {
            Id = 1,
            Eleve = 2,
            Provenance = "Bègles",
            Astro = "Balance",
            Boisson = "Espresso martini",
            Soiree ="Apéro chill avec 3/4 potes",
            Son = "Alpha-Damso",
            Livre = "La femme de ménage",
            Film = "Simone",
            PasseTemps = "Sport",
            Defaut = "Naïve",
            Qualite = "A l'écoute",
            Relation = "Amie",
            Preference = "Peu importe",
        };
        context.Questionnaires.AddRange(
            alepri
        ); 

        Voeu Alepri = new ()
        {
            Id = 1,
            //Eleve = 2,
            //Promotion = 2 ,
            NumVoeux = 1,
            //EleveChoisi = 3,
        };
        context.Voeux.AddRange(
            Alepri
        );*/

         // Commit changes into DB
        context.SaveChanges(); 
    }
}