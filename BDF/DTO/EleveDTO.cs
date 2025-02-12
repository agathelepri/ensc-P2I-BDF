public class EleveDTO
{
    public int Id {get;set;}
    public string Nom {get;set;}
    public string Prenom {get;set;}
    public string Login {get;set;}
    public string MDP {get;set;}
    public PromotionDTO Promotion {get;set;}
    public FamilleDTO Famille {get;set;} = null!;
    public byte[] Photo { get; set; }
    public EleveDTO EleveParrain {get;set;} = null!;

    public EleveDTO(Eleve x) 
    {
        Id=x.Id;
        Nom=x.Nom;
        Prenom=x.Prenom;
        Login=x.Login;
        MDP=x.MDP;
        Promotion = new PromotionDTO(x.Promotion!);
        Famille = new FamilleDTO(x.Famille!);
        Photo = x.Photo;
        EleveParrain = new EleveDTO(x.EleveParrain!);

    }
}