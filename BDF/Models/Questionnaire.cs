// Ce modèle contient les réponses au questionnaire rempli par un élève.
// Il permet d’enrichir le profil de l’élève pour un matching plus pertinent basé sur les affinités (soiree, passe-temps, etc.).
// Chaque questionnaire est lié à un élève via une clé étrangère.

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Questionnaire
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int EleveId { get; set; }

    [ForeignKey("EleveId")]
    public Eleve? Eleve { get; set; } 

    public string? Provenance { get; set; } = null!;
    public string? Astro { get; set; } = null!;
    public string? Boisson { get; set; } = null!;
    public string? Soiree { get; set; } = null!;
    public string? Son { get; set; } = null!;
    public string? Livre { get; set; } = null!;
    public string? Film { get; set; } = null!;
    public string? PasseTemps { get; set; } = null!;
    public string? Defaut { get; set; } = null!;
    public string? Qualite { get; set; } = null!;
    public string? Relation { get; set; } = null!;
    public string? Preference { get; set; } = null!;

    public Questionnaire() {}

    public Questionnaire(QuestionnaireDTO questionnaireDTO)
    {
        Provenance = questionnaireDTO.Provenance;
        Astro = questionnaireDTO.Astro;
        Boisson = questionnaireDTO.Boisson;
        Soiree = questionnaireDTO.Soiree;
        Son = questionnaireDTO.Son;
        Livre = questionnaireDTO.Livre;
        Film = questionnaireDTO.Film;
        PasseTemps = questionnaireDTO.PasseTemps;
        Defaut = questionnaireDTO.Defaut;
        Qualite = questionnaireDTO.Qualite;
        Relation = questionnaireDTO.Relation;
        Preference = questionnaireDTO.Preference;
        Eleve=new Eleve(questionnaireDTO.EleveId);
    }

}
