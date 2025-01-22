public class EleveDTO
{
    public int Id {get;set;}
    public string Nom {get;set;}
    public string Prenom {get;set;}
    public string Login {get;set;}
    public string MDP {get;set;}
    public Promotion IdPromotion {get;set;}
    public Famille IdFamille {get;set;} = null!;
    public byte[] Photo { get; set; }
    public Eleve IdEleveParrain {get;set;} = null!;

    public EleveDTO(){}
}