public class Questionnaire
{
    public int Id {get;set;}
    public Eleve IdEleve {get;set;}
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
    public Questionnaire(){}

}