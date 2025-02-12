using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Questionnaire
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Clé étrangère explicite pour Eleve
    public int EleveId { get; set; }

    [ForeignKey("EleveId")]
    public Eleve Eleve { get; set; } = null!; // Relation avec Eleve

    public string Provenance { get; set; } = null!;
    public string Astro { get; set; } = null!;
    public string Boisson { get; set; } = null!;
    public string Soiree { get; set; } = null!;
    public string Son { get; set; } = null!;
    public string Livre { get; set; } = null!;
    public string Film { get; set; } = null!;
    public string PasseTemps { get; set; } = null!;
    public string Defaut { get; set; } = null!;
    public string Qualite { get; set; } = null!;
    public string Relation { get; set; } = null!;
    public string Preference { get; set; } = null!;

    // Constructeur par défaut requis par Entity Framework
    public Questionnaire() {}

    // Constructeur basé sur QuestionnaireDTO
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
        Eleve=new Eleve(questionnaireDTO.Eleve);
    }

}
