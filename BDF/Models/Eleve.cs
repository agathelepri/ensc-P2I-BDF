using System.ComponentModel.DataAnnotations.Schema;

public class Eleve
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    public Eleve(string nom, string prenom, string login, Promotion promotion)
    {
        Nom = nom;
        Prenom = prenom;
        Login = login;
        Promotion = promotion;
    }

    public Eleve(EleveDTO eleveDTO)
    {
        this.Nom = eleveDTO.Nom;
        this.Prenom = eleveDTO.Prenom;
        this.Login = eleveDTO.Login;
        if (eleveDTO.Promotion != null)
        {
            this.Promotion = new Promotion(eleveDTO.Promotion);
        }
    }
}