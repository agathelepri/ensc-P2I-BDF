using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

public class Promotion 
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}
    public int Annee {get;set;}
    public Promotion(){}
    public Promotion(PromotionDTO promotionDTO) 
    {
        this.Id=promotionDTO.Id;
        this.Annee=promotionDTO.Annee;
    }

    public Promotion(int annee)
    {
        Annee = annee;
    }
    
}