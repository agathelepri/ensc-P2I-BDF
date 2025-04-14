// Ce modèle représente un élève dans la base de données.
// Il contient toutes les informations personnelles (nom, prénom, login, mot de passe),
// ainsi que ses relations : promotion, famille, parrain (et filleuls via navigation inversée).
// C’est l’entité centrale du système de parrainage.

using System.ComponentModel.DataAnnotations.Schema;
public class Eleve
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Login { get; set; }
    public string? MDP { get; set; }
    public int PromotionId {get;set;}
    [ForeignKey("PromotionId")]
    public Promotion Promotion { get; set; }
    public int? FamilleId { get; set; }
    [ForeignKey("FamilleId")]
    public Famille? Famille { get; set; }
    public byte[]? Photo { get; set; }
    [ForeignKey("EleveParrainId")]
    public Eleve? EleveParrain { get; set; }
    public int? EleveParrainId { get; set; }

    public Eleve(int eleveId) {
        this.Id=eleveId;
    }
    public Eleve (){}

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
        this.PromotionId = eleveDTO.PromotionId;
    }
}