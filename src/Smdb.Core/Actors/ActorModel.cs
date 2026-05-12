namespace Smdb.Core.Actors;

public class Actor
{
    public int    Id        { get; set; }
    public string FirstName { get; set; }
    public string LastName  { get; set; }
    public double Rating    { get; set; }
    public string Bio       { get; set; }

    public Actor(int id, string firstName, string lastName, double rating, string bio)
    {
        Id        = id;
        FirstName = firstName;
        LastName  = lastName;
        Rating    = rating;
        Bio       = bio;
    }

    public override string ToString()
        => $"Actor[Id={Id}, Name={FirstName} {LastName}, Rating={Rating}]";
}
