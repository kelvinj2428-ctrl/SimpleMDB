namespace Smdb.Core.Db;

using Smdb.Core.Movies;
using Smdb.Core.Actors;
using Smdb.Core.ActorMovies;
using Smdb.Core.Users;

public class MemoryDatabase
{
    public List<Movie> Movies { get; }
    public List<Actor> Actors { get; }
    public List<ActorMovie> ActorMovies { get; }
    public List<User> Users { get; }

    private int nextMovieId;
    private int nextActorId;
    private int nextActorMovieId;
    private int nextUserId;

    public MemoryDatabase()
    {
        Movies = [];
        Actors = [];
        ActorMovies = [];
        Users = [];

        SeedMovies();
        SeedActors();
        SeedUsers();

        nextMovieId = Movies.Count;
        nextActorId = Actors.Count;
        nextActorMovieId = ActorMovies.Count;
        nextUserId = Users.Count;
    }

    private void SeedMovies()
    {
        Movies.AddRange(new Movie[]
        {
            new Movie(1,  "The Godfather",                                    1972, "Crime/Drama",      9.2, "A mafia patriarch hands the family empire to his reluctant son."),
            new Movie(2,  "The Godfather Part II",                            1974, "Crime/Drama",      9.0, "Michael consolidates power as flashbacks trace Vito Corleone's rise."),
            new Movie(3,  "The Dark Knight",                                  2008, "Action/Crime",     9.0, "Batman faces the Joker, who pushes Gotham into chaos."),
            new Movie(4,  "The Shawshank Redemption",                         1994, "Drama",            9.3, "An innocent banker forms a life-saving friendship in prison."),
            new Movie(5,  "Pulp Fiction",                                     1994, "Crime/Drama",      8.9, "Interlocking LA crime stories unfold with dark humor."),
            new Movie(6,  "Schindler's List",                                 1993, "Biography/Drama",  9.0, "A businessman saves Jewish workers during the Holocaust."),
            new Movie(7,  "The Lord of the Rings: The Return of the King",    2003, "Adventure",        9.0, "The final push to destroy the One Ring decides Middle-earth's fate."),
            new Movie(8,  "Fight Club",                                       1999, "Drama/Thriller",   8.8, "An insomnia-plagued worker joins a charismatic anarchist's secret club."),
            new Movie(9,  "Forrest Gump",                                     1994, "Drama/Romance",    8.8, "A kind man unwittingly drifts through historic American moments."),
            new Movie(10, "Inception",                                        2010, "Action/Sci-Fi",    8.8, "A thief enters dreams to plant an idea in a target's mind."),
            new Movie(11, "The Matrix",                                       1999, "Action/Sci-Fi",    8.7, "A hacker learns reality is a simulated prison for humanity."),
            new Movie(12, "Se7en",                                            1995, "Crime/Mystery",    8.6, "Two detectives hunt a killer using the seven deadly sins."),
            new Movie(13, "Goodfellas",                                       1990, "Biography/Crime",  8.7, "Henry Hill's rise and fall inside the New York mob."),
            new Movie(14, "The Silence of the Lambs",                         1991, "Crime/Thriller",   8.6, "An FBI trainee consults Hannibal Lecter to catch a serial killer."),
            new Movie(15, "Star Wars: Episode IV - A New Hope",               1977, "Adventure/Sci-Fi", 8.6, "A farm boy joins rebels to destroy the Empire's Death Star."),
            new Movie(16, "The Empire Strikes Back",                          1980, "Adventure/Sci-Fi", 8.7, "The Rebels scatter as Luke confronts Darth Vader."),
            new Movie(17, "Interstellar",                                     2014, "Adventure/Sci-Fi", 8.7, "Astronauts travel through a wormhole to save a dying Earth."),
            new Movie(18, "Parasite",                                         2019, "Drama/Thriller",   8.5, "A poor family infiltrates a wealthy household with unforeseen fallout."),
            new Movie(19, "Spirited Away",                                    2001, "Animation",        8.6, "A girl navigates a spirit bathhouse to free her parents."),
            new Movie(20, "City of God",                                      2002, "Crime/Drama",      8.6, "Two boys take diverging paths amid Rio's gang wars."),
            new Movie(21, "The Lord of the Rings: The Fellowship of the Ring",2001, "Adventure",        8.8, "A hobbit and his companions set out to destroy a powerful ring."),
            new Movie(22, "The Lord of the Rings: The Two Towers",            2002, "Adventure",        8.7, "The Fellowship is broken as war engulfs Middle-earth."),
            new Movie(23, "Saving Private Ryan",                              1998, "Drama/War",        8.6, "A WWII captain leads a squad deep into France to find a soldier."),
            new Movie(24, "The Green Mile",                                   1999, "Drama/Fantasy",    8.6, "A death-row guard befriends a gentle giant with miraculous powers."),
            new Movie(25, "Gladiator",                                        2000, "Action/Drama",     8.5, "A Roman general seeks revenge after being enslaved and forced to fight."),
            new Movie(26, "The Departed",                                     2006, "Crime/Thriller",   8.5, "An undercover cop and a mole race to expose each other in Boston."),
            new Movie(27, "Whiplash",                                         2014, "Drama/Music",      8.5, "A driven drummer endures brutal mentorship at a top music conservatory."),
            new Movie(28, "The Prestige",                                     2006, "Drama/Mystery",    8.5, "Two rival magicians battle to create the ultimate illusion."),
            new Movie(29, "Memento",                                          2000, "Mystery/Thriller", 8.4, "A man with short-term memory loss hunts his wife's killer using tattoos."),
            new Movie(30, "Apocalypse Now",                                   1979, "Drama/War",        8.4, "A soldier travels upriver to terminate a rogue colonel in Vietnam."),
            new Movie(31, "Rear Window",                                      1954, "Mystery/Thriller", 8.5, "A photographer confined to a wheelchair suspects his neighbor of murder."),
            new Movie(32, "Casablanca",                                       1942, "Drama/Romance",    8.5, "A nightclub owner must choose between love and helping a resistance leader."),
            new Movie(33, "Alien",                                            1979, "Horror/Sci-Fi",    8.5, "The crew of a spaceship encounters a deadly extraterrestrial organism."),
            new Movie(34, "Blade Runner 2049",                                2017, "Action/Sci-Fi",    8.0, "A blade runner uncovers a secret that could destabilize society."),
            new Movie(35, "No Country for Old Men",                           2007, "Crime/Thriller",   8.2, "A hunter stumbles upon drug money and is pursued by a relentless killer."),
            new Movie(36, "There Will Be Blood",                              2007, "Drama",            8.2, "An oil prospector's ruthless ambition destroys everything around him."),
            new Movie(37, "2001: A Space Odyssey",                            1968, "Adventure/Sci-Fi", 8.3, "Humanity's evolution is guided by a mysterious monolith across millennia."),
            new Movie(38, "Taxi Driver",                                      1976, "Crime/Drama",      8.2, "A disturbed Vietnam vet becomes a vigilante in a decaying New York City."),
            new Movie(39, "Oldboy",                                           2003, "Action/Mystery",   8.1, "A man imprisoned for 15 years seeks revenge without knowing why he was held."),
            new Movie(40, "Pan's Labyrinth",                                  2006, "Drama/Fantasy",    8.2, "A girl escapes post-war Spain into a dark fairy-tale underworld."),
            new Movie(41, "The Truman Show",                                  1998, "Comedy/Drama",     8.2, "A man discovers his entire life is a reality TV show."),
            new Movie(42, "Eternal Sunshine of the Spotless Mind",            2004, "Drama/Romance",    8.3, "A couple erases each other from their memories after a painful breakup."),
            new Movie(43, "A Beautiful Mind",                                 2001, "Biography/Drama",  8.2, "A brilliant mathematician battles schizophrenia while making groundbreaking discoveries."),
            new Movie(44, "The Pianist",                                      2002, "Biography/Drama",  8.5, "A Polish-Jewish pianist survives the destruction of the Warsaw ghetto."),
            new Movie(45, "Life Is Beautiful",                                1997, "Comedy/Drama",     8.6, "A father uses humor to shield his son from the horrors of a Nazi camp."),
            new Movie(46, "Requiem for a Dream",                              2000, "Drama",            8.3, "Four people's drug addictions spiral into devastating consequences."),
            new Movie(47, "Black Swan",                                       2010, "Drama/Thriller",   8.0, "A ballerina's obsession with perfection leads to a psychological breakdown."),
            new Movie(48, "Mad Max: Fury Road",                               2015, "Action/Adventure", 8.1, "In a post-apocalyptic wasteland, a woman flees a tyrant with a group of prisoners."),
            new Movie(49, "The Grand Budapest Hotel",                         2014, "Comedy/Drama",     8.1, "A legendary concierge and his protege become embroiled in a murder mystery."),
            new Movie(50, "Coco",                                             2017, "Animation",        8.4, "A boy travels to the Land of the Dead to uncover his family's musical history."),
        });
    }

    private void SeedActors()
    {
        Actors.AddRange(new Actor[]
        {
            new Actor(1, "Marlon",   "Brando",   9.5, "Legendary American actor known for The Godfather and Apocalypse Now."),
            new Actor(2, "Al",       "Pacino",   9.3, "Iconic actor famous for The Godfather series and Scarface."),
            new Actor(3, "Christian","Bale",     8.9, "British actor known for The Dark Knight and American Psycho."),
            new Actor(4, "Heath",    "Ledger",   9.8, "Australian actor who gave a legendary performance as the Joker."),
            new Actor(5, "Tom",      "Hanks",    9.0, "Beloved American actor known for Forrest Gump and Cast Away."),
            new Actor(6, "Leonardo", "DiCaprio", 9.1, "Award-winning actor known for Inception and The Revenant."),
            new Actor(7, "Keanu",    "Reeves",   8.7, "Canadian actor famous for The Matrix and John Wick."),
            new Actor(8, "Morgan",   "Freeman",  9.2, "Acclaimed actor known for The Shawshank Redemption and Se7en."),
        });
    }

    private void SeedUsers()
    {
        // Password stored as plain text for demo purposes only.
        // In production, use a proper hashing algorithm (e.g., BCrypt).
        Users.AddRange(new User[]
        {
            new User(1, "admin",   "admin123",   "admin"),
            new User(2, "user1",   "user123",    "user"),
            new User(3, "user2",   "user456",    "user"),
        });
    }

    public int NextMovieId()     => ++nextMovieId;
    public int NextActorId()     => ++nextActorId;
    public int NextActorMovieId()=> ++nextActorMovieId;
    public int NextUserId()      => ++nextUserId;
}
