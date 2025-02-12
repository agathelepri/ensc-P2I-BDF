public class VoeuDTO 
{
    public int Id {get;set;}
    public EleveDTO Eleve {get;set;}
    public PromotionDTO Promotion {get;set;}
    public int NumVoeux {get;set;}
    public EleveDTO EleveChoisi {get;set;}

    public VoeuDTO(Voeu x) 
    {
        Id=x.Id;
        Eleve=new EleveDTO(x.Eleve!);
        Promotion=new PromotionDTO(x.Promotion);
        NumVoeux=x.NumVoeux;
        EleveChoisi=new EleveDTO(x.EleveChoisi);

    }
}