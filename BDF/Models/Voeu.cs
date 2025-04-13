// Ce modèle représente un voeu de parrainage exprimé par un élève.
// Il relie un élève à un autre (celui qu’il choisit comme parrain ou filleul), en précisant la promotion et le rang du voeu.
// Il est utilisé dans l’algorithme de matching pour prioriser les correspondances.

using System.ComponentModel.DataAnnotations.Schema;

public class Voeu 
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}

    public int EleveId {get;set;}
    public Eleve Eleve {get;set;}
    public int PromotionId {get;set;}
    public Promotion Promotion {get;set;}
    public int NumVoeux {get;set;}
    public int EleveChoisiId {get;set;}
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
        Id = voeuDTO.Id;
        NumVoeux = voeuDTO.NumVoeux;
    }
}