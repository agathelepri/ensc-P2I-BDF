// Ce modèle représente une promotion d’élèves identifiée par son année.
// Elle est utilisée pour différencier parrains et filleuls, ainsi que pour trier ou filtrer les élèves.
// C’est une entité simple mais essentielle pour la logique du matching.

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

    public Promotion(int id)
    {
        Id = id;
    }
    
}