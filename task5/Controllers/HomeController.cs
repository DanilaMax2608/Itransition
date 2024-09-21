using Bogus;
using Microsoft.AspNetCore.Mvc;
using task55.Models;

public class HomeController : Controller
{
    private static readonly Random _random = new Random();

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult GenerateData([FromBody] GenerateDataRequest request)
    {
        try
        {
            var faker = request.Region switch
            {
                "pl_PL" => new Faker("pl"),
                "en_US" => new Faker("en_US"),
                "ru_RU" => new Faker("ru"),
                _ => new Faker("en_US"),
            };

            var data = GenerateFakeData(faker, request.ErrorRate, request.Seed, request.Page);
            return Json(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private List<UserData> GenerateFakeData(Faker faker, double errorRate, int seed, int page)
    {
        var combinedSeed = seed + page;
        var random = new Random(combinedSeed);
        var data = new List<UserData>();

        int recordsToLoad = page == 1 ? 20 : 10;
        int startId = (page - 1) * 10 + 1;

        for (int i = 0; i < recordsToLoad; i++)
        {
            var user = new UserData
            {
                Id = startId + i,
                Number = startId + i,
                FullName = faker.Name.FullName(),
                Address = faker.Address.FullAddress(),
                PhoneNumber = faker.Phone.PhoneNumber()
            };

            user = IntroduceErrors(user, errorRate, random);
            data.Add(user);
        }

        return data;
    }


    private UserData IntroduceErrors(UserData user, double errorRate, Random random)
    {
        var fields = new List<string> { user.FullName, user.Address, user.PhoneNumber };

        for (int i = 0; i < fields.Count; i++)
        {
            var field = fields[i];
            var numberOfErrors = (int)Math.Floor(errorRate);
            var probabilityOfExtraError = errorRate - numberOfErrors;

            for (int j = 0; j < numberOfErrors; j++)
            {
                field = IntroduceError(field, random);
            }

            if (random.NextDouble() < probabilityOfExtraError)
            {
                field = IntroduceError(field, random);
            }

            fields[i] = field;
        }

        user.FullName = fields[0];
        user.Address = fields[1];
        user.PhoneNumber = fields[2];

        return user;
    }

    private string IntroduceError(string text, Random random)
    {
        var errorType = random.Next(3);

        switch (errorType)
        {
            case 0:
                return DeleteRandomCharacter(text, random);
            case 1:
                return AddRandomCharacter(text, random);
            case 2:
                return SwapAdjacentCharacters(text, random);
            default:
                return text;
        }
    }

    private string DeleteRandomCharacter(string text, Random random)
    {
        if (string.IsNullOrEmpty(text)) return text;
        var index = random.Next(text.Length);
        return text.Remove(index, 1);
    }

    private string AddRandomCharacter(string text, Random random)
    {
        var index = random.Next(text.Length + 1);
        var randomChar = (char)random.Next('a', 'z' + 1);
        return text.Insert(index, randomChar.ToString());
    }

    private string SwapAdjacentCharacters(string text, Random random)
    {
        if (text.Length < 2) return text;
        var index = random.Next(text.Length - 1);
        var chars = text.ToCharArray();
        var temp = chars[index];
        chars[index] = chars[index + 1];
        chars[index + 1] = temp;
        return new string(chars);
    }
}

public class GenerateDataRequest
{
    public string Region { get; set; }
    public double ErrorRate { get; set; }
    public int Seed { get; set; }
    public int Page { get; set; }
}
