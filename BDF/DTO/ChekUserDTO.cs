// Ce DTO est utilisé pour la vérification des identifiants lors de la connexion d’un utilisateur.
// Il contient le login et éventuellement le mot de passe saisi par l’utilisateur.

public class CheckUserDTO
{
    public string Login{get;set;}
    public string? MDP {get;set;}

    public CheckUserDTO(){}
}