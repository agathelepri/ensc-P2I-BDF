public class QuestionnaireDTO
{
    public int Id {get;set;}
    /* public EleveDTO Eleve {get;set;} */
    public int EleveId {get;set;}
    public string Provenance {get;set;} = null!;
    public string Astro {get;set;} = null!;
    public string Boisson {get;set;} = null!;
    public string Soiree {get;set;} = null!;
    public string Son {get;set;} = null!;
    public string Livre {get;set;} = null!;
    public string Film {get;set;} = null!;
    public string PasseTemps {get;set;} = null!;
    public string Defaut {get;set;} = null!;
    public string Qualite {get;set;} = null!;
    public string Relation {get;set;} = null!;
    public string Preference {get;set;} = null!;
    public QuestionnaireDTO(){}
    public QuestionnaireDTO(Questionnaire x) 
    {
        Id=x.Id;
        EleveId = x.Eleve != null ? x.Eleve.Id : 0;
        Provenance=x.Provenance;
        Astro=x.Astro;
        Boisson=x.Boisson;
        Soiree=x.Soiree;
        Son=x.Son;
        Livre=x.Livre;
        Film=x.Film;
        PasseTemps=x.PasseTemps;
        Defaut=x.Defaut;
        Qualite=x.Qualite;
        Relation=x.Relation;
        Preference=x.Preference;
    }

}