public class VoeuDTO 
{
    public int Id {get;set;}
    public Eleve IdEleve {get;set;}
    public Promotion IdPromotion {get;set;}
    public int NumVoeux {get;set;}
    public Eleve IdEleveChoisi {get;set;}

    public VoeuDTO(Voeu x) {}
}