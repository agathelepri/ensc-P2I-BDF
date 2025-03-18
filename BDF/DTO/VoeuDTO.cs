public class VoeuDTO 
{
    public int Id {get;set;}
    /* public EleveDTO Eleve {get;set;} */
    public int EleveId {get;set;}
   /*  public PromotionDTO Promotion {get;set;}  */
   public int PromotionId {get;set;}
    public int NumVoeux {get;set;}
    /* public EleveDTO EleveChoisi {get;set;} */
    public int EleveChoisiId {get;set;}
    
    public VoeuDTO(){}
    public VoeuDTO(Voeu x) 
    {
        Id=x.Id;
        EleveId=x.Eleve.Id;
        PromotionId = x.Promotion != null ? x.Promotion.Id : 0;
        NumVoeux=x.NumVoeux;
        EleveChoisiId=x.EleveChoisi.Id;

    }
}