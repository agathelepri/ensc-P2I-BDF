// Ce DTO permet de transmettre les vœux de parrainage effectués par les élèves.
// Il contient l’élève demandeur, l’élève choisi, la promotion concernée et le rang du vœu.
// Ces données sont essentielles pour le fonctionnement de l’algorithme de matching.

public class VoeuDTO 
{
    public int Id {get;set;}
    public int EleveId {get;set;}
   public int PromotionId {get;set;}
    public int NumVoeux {get;set;}
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