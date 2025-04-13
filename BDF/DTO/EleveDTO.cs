// Ce DTO sert à transférer les données d’un élève entre le client et l’API.
// Il regroupe l’identité de l’élève, sa promotion, sa famille, son parrain, ses filleuls et sa photo.
// C’est l’un des DTO les plus centraux de l’application.

public class EleveDTO
{
    public int Id { get; set; }
    public string Nom { get; set; } = "Inconnu";
    public string Prenom { get; set; } = "Inconnu";
    public string Login { get; set; } = "default_login";
    public string MDP { get; set; } = "";
    public int PromotionId { get; set; }
    public PromotionDTO Promotion {get;set;}
    public int FamilleId { get; set; }
    public byte[] Photo { get; set; } = new byte[0];  
    public int EleveParrainId { get; set; }
    public EleveDTO EleveParrain {get;set;}
    
    public EleveDTO(){}
    public EleveDTO(Eleve x)
    {
        if (x == null)
            throw new ArgumentNullException(nameof(x), "L'objet Eleve fourni est null");

        Id = x.Id;
        Nom = x.Nom ?? "Inconnu";  
        Prenom = x.Prenom ?? "Inconnu";
        Login = x.Login ?? "default_login";
        MDP = x.MDP ?? "";

        PromotionId = x.Promotion != null ? x.Promotion.Id : 0;  
        Promotion=x.Promotion != null ? new PromotionDTO(x.Promotion) : null;
        FamilleId = x.Famille != null ? x.Famille.Id : 0;  
        Photo = x.Photo ?? new byte[0];  
        EleveParrainId = x.EleveParrain != null ? x.EleveParrain.Id : 0;  
        EleveParrain=x.EleveParrain != null ? new EleveDTO(x.EleveParrain) : null;
    }


}