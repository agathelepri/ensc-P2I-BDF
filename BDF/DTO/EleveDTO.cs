public class EleveDTO
{

    /* public int Id {get;set;}
    public string Nom {get;set;}= "Inconnu";
    public string Prenom {get;set;}=null!;
    public string Login {get;set;}=null!;
    public string MDP {get;set;} = null!;
    /* public PromotionDTO Promotion {get;set;} */
    //public int Promotion { get; set; }
    /* public FamilleDTO Famille {get;set;} = null!; */
    //public int Famille { get; set; }
    //public byte[] Photo { get; set; } = null!;
   /*  public EleveDTO EleveParrain {get;set;} = null!; */
   //public int EleveParrain { get; set; }
    public int Id { get; set; }
    public string Nom { get; set; } = "Inconnu";
    public string Prenom { get; set; } = "Inconnu";
    public string Login { get; set; } = "default_login";
    public string MDP { get; set; } = "";
    public int PromotionId { get; set; }
    public int FamilleId { get; set; }
    public byte[] Photo { get; set; } = new byte[0];  // Evite NULL
    public int EleveParrainId { get; set; }
    public EleveDTO EleveParrain {get;set;}
    public List<EleveDTO> Filleuls {get;set;}
    /* public EleveDTO(Eleve x) 
    {
        Id=x.Id;
        Nom=x.Nom;
        Prenom=x.Prenom;
        Login=x.Login;
        MDP=x.MDP;
        Promotion = x.Promotion.Id;
        Famille = x.Famille.Id!;
        Photo = x.Photo;
        EleveParrain = x.EleveParrain.Id!;

    } */
    public EleveDTO(){}
    public EleveDTO(Eleve x)
    {
        if (x == null)
            throw new ArgumentNullException(nameof(x), "L'objet Eleve fourni est null");

        Id = x.Id;
        Nom = x.Nom ?? "Inconnu";  // Évite l'erreur si NULL
        Prenom = x.Prenom ?? "Inconnu";
        Login = x.Login ?? "default_login";
        MDP = x.MDP ?? "";

        PromotionId = x.Promotion != null ? x.Promotion.Id : 0;  // Évite NULL
        FamilleId = x.Famille != null ? x.Famille.Id : 0;  // Évite NULL
        Photo = x.Photo ?? new byte[0];  // Évite NULL
        EleveParrainId = x.EleveParrain != null ? x.EleveParrain.Id : 0;  // Évite NULL
        EleveParrain=x.EleveParrain != null ? new EleveDTO(x.EleveParrain) : null;
    }


}