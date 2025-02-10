using System.ComponentModel.DataAnnotations.Schema;

public class Promotion 
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}
    public int Annee {get;set;}
    public Promotion(PromotionDTO promotionDTO) {}
    public Promotion(int annee)
    {
        Annee = annee;
    }
}