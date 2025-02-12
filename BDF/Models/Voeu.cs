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
        this.Eleve= new Eleve(voeuDTO.Eleve);
        this.Promotion = new Promotion(voeuDTO.Promotion);
        this.NumVoeux=voeuDTO.NumVoeux;
        if (voeuDTO.EleveChoisi != null)
        {
            this.EleveChoisi = new Eleve(voeuDTO.EleveChoisi);
        }
    }
}