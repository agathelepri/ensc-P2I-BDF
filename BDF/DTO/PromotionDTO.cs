public class PromotionDTO 
{
    public int Id {get;set;}
    public int Annee {get;set;}
    public PromotionDTO(Promotion x) {
        Id=x.Id;
        Annee=x.Annee;
    }
}