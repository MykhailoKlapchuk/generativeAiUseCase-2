using Bogus;
using CsvHelper;
using generativeAiUseCase_2;
using System.Globalization;

string[] Genres = { "Action", "Adventure", "Comedy", "Drama", "Fantasy", "Horror", "Mystery", "Romance", "Thriller", "Sci-Fi" };
string[] AgeCertifications = { "G", "PG", "PG-13", "R", "NC-17", "U", "U/A", "A", "S", "AL", "6", "9", "12", "12A", "15", "18", "18R", "R18", "R21", "M", "MA15+", "R16", "R18+", "X18", "T", "E", "E10+", "EC", "C", "CA", "GP", "M/PG", "TV-Y", "TV-Y7", "TV-G", "TV-PG", "TV-14", "TV-MA" };
string[] Roles = { "Director", "Producer", "Screenwriter", "Actor", "Actress", "Cinematographer", "Film Editor", "Production Designer", "Costume Designer", "Music Composer" };

var titlesFaker = new Faker<Title>()
                .RuleFor(t => t.Id, f => f.UniqueIndex + 1)
                .RuleFor(t => t.TitleName, f => f.Lorem.Sentence(3))
                .RuleFor(t => t.Description, f => f.Lorem.Sentence(15))
                .RuleFor(t => t.ReleaseYear, f => f.Random.Int(1900, DateTime.Now.Year))
                .RuleFor(t => t.AgeCertification, f => f.PickRandom(AgeCertifications))
                .RuleFor(t => t.Runtime, f => f.Random.Int(60, 240))
                .RuleFor(t => t.Genres, f => f.PickRandom(Genres, f.Random.Int(2, 5)).ToList())
                .RuleFor(t => t.ProductionCountry, f => f.Address.CountryCode())
                .RuleFor(t => t.Seasons, f => f.Random.Bool(0.8f) ? null : f.Random.Int(1, 10));

var creditsFaker = new Faker<Credit>()
    .RuleFor(c => c.Id, f => f.UniqueIndex + 1)
    .RuleFor(c => c.RealName, f => f.Name.FullName())
    .RuleFor(c => c.CharacterName, f => f.Name.FullName())
    .RuleFor(c => c.Role, f => f.PickRandom(Roles));

var titles = titlesFaker.Generate(new Random().Next(100, 150));
var credits = new List<Credit>();

foreach (var title in titles)
{
    var numOfCredits = new Random().Next(1, 5);
    for (var i = 0; i < numOfCredits; i++)
    {
        var credit = creditsFaker.Generate();
        credit.TitleId = title.Id;
        credits.Add(credit);
    }
}

using (var writer = new StreamWriter("titles.csv"))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(titles);
}

using (var writer = new StreamWriter("credits.csv"))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(credits);
}

Console.WriteLine($"Files were generated. Titles: {titles.Count}, Credits: {credits.Count}.");
