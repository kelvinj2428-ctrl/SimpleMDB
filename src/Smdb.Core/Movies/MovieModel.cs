namespace Smdb.Core.Movies;

public class Movie
{
    public int    Id          { get; set; }
    public string Title       { get; set; }
    public int    Year        { get; set; }
    public string Genre       { get; set; }
    public double Rating      { get; set; }
    public string Description { get; set; }

    public Movie(int id, string title, int year, string genre, double rating, string description)
    {
        Id          = id;
        Title       = title;
        Year        = year;
        Genre       = genre;
        Rating      = rating;
        Description = description;
    }

    public override string ToString()
        => $"Movie[Id={Id}, Title={Title}, Year={Year}, Genre={Genre}, Rating={Rating}]";
}
