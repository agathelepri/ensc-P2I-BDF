using System.ComponentModel.DataAnnotations.Schema;

public class Voeu 
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}
    public Eleve Eleve {get;set;}
    public Promotion Promotion {get;set;}
    public int NumVoeux {get;set;}
    public Eleve EleveChoisi {get;set;}
    public Voeu (){}

    public Voeu(Eleve eleve, Promotion promotion, int numVoeux, Eleve eleveChoisi)
    {
        Eleve = eleve;
        Promotion = promotion;
        NumVoeux = numVoeux;
        EleveChoisi = eleveChoisi;
    }

    public Voeu(VoeuDTO voeuDTO)
    {
        this.Id=voeuDTO.Id;
        this.Eleve= new Eleve(voeuDTO.EleveId);
        this.Promotion = new Promotion(voeuDTO.PromotionId);
        this.NumVoeux=voeuDTO.NumVoeux;
        this.EleveChoisi = new Eleve(voeuDTO.EleveChoisiId);
    }
}