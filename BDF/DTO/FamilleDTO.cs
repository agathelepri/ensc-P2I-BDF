public class FamilleDTO 
{
    public int Id {get;set;}
    public string Nom {get;set;}
    public string CouleurHexa {get;set;}
    public int Points { get; set; }
    public FamilleDTO(Famille x) 
    {
        Id = x.Id;
        Nom = x.Nom;
        CouleurHexa = x.CouleurHexa;
        Points = x.Points; 
    }
    public FamilleDTO(){}
}