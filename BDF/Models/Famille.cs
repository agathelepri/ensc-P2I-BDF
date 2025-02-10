using System.ComponentModel.DataAnnotations.Schema;

public class Famille 
{
    private FamilleDTO familleDTO;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get;set;}
    public string Nom {get;set;}
    public string CouleurHexa {get;set;}
    public Famille(){}
    public Famille(string nom, string couleurHexa)
    {
        Nom = nom;
        CouleurHexa = couleurHexa;
    }

    public Famille(FamilleDTO familleDTO)
    {
        this.familleDTO = familleDTO;
    }
}