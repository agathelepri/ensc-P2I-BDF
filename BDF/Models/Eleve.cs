public class Eleve
{
    public int Id {get;set;}
    public string Nom {get;set;}
    public string Prenom {get;set;}
    public string Login {get;set;}
    public string MDP {get;set;} = null!;
    public Promotion Promotion {get;set;}
    public Famille Famille {get;set;} = null!;
    public byte[] Photo { get; set; }
    public Eleve EleveParrain {get;set;} = null!;

    public Eleve(){}
}