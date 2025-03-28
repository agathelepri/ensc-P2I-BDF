using System.ComponentModel.DataAnnotations.Schema;

public class Famille 
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}
    public string Nom {get;set;}
    public string CouleurHexa {get;set;}
    public int Points { get; set; }
    public Famille(){}

    public Famille(string nom, string couleurHexa, int points = 0)
    {
        Nom = nom;
        CouleurHexa = couleurHexa;
        Points=points;
    }

    public Famille(FamilleDTO familleDTO)
    {
        this.Id = familleDTO.Id;
        this.Nom=familleDTO.Nom;
        this.CouleurHexa=familleDTO.CouleurHexa;
        this.Points = familleDTO.Points;
    }
}