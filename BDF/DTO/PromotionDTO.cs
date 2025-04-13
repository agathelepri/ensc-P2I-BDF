// Ce DTO transporte les informations relatives à une promotion (année et ID).
// Il permet de distinguer les niveaux (promo n / promo n-1) pour le matching et l’affichage ciblé.

public class PromotionDTO 
{
    public int Id {get;set;}
    public int Annee {get;set;}
    public PromotionDTO(){}
    public PromotionDTO(Promotion x) {
        Id=x.Id;
        Annee=x.Annee;
    }
}