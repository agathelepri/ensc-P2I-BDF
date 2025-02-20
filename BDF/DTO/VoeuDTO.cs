public class VoeuDTO 
{
    public int Id {get;set;}
    /* public EleveDTO Eleve {get;set;} */
    public int Eleve {get;set;}
   /*  public PromotionDTO Promotion {get;set;}  */
   public int Promotion {get;set;}
    public int NumVoeux {get;set;}
    /* public EleveDTO EleveChoisi {get;set;} */
    public int EleveChoisi {get;set;}
    
    public VoeuDTO(){}
    public VoeuDTO(Voeu x) 
    {
        Id=x.Id;
        Eleve=x.Eleve.Id;
        Promotion =x.Promotion.Id;
        NumVoeux=x.NumVoeux;
        EleveChoisi=x.EleveChoisi.Id;

    }
}