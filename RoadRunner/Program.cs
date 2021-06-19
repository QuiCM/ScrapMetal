using System.Diagnostics;
using System.Threading.Tasks;
using ScrapMetal;

/*
    RoadRunner is a simple console application to run a ScrapMetal instance
*/
namespace RoadRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Debug.WriteLine("RoadRunner is putting on its shoes.");

            string authToken = null;
            if (args.Length > 0)
            {
                authToken = args[0];
            }

            Debug.WriteLine($"Shoelaces found: {authToken}");
            ScrapMetalBrain brain;

            using (ScrapMetalBot scrapMetal = new ScrapMetalBuilder()
                                                .WithAuth(authToken)
                                                .Build())
            {
                Debug.WriteLine("RoadRunner has built itself a ScrapMetalBot. Pressing play now!");

                await scrapMetal.ConnectAsync();
                await scrapMetal.PollAsync();

                brain = scrapMetal.Brain;
                scrapMetal.Close();
            }

            Debug.WriteLine("Giving ScrapMetal some time to rust away.");
            await Task.Delay(3000);
            Debug.WriteLine("Attempting to build a new ScrapMetalBot!");

            Debug.WriteLine("We might have saved the brain?");
            using (ScrapMetalBot scrapMetal = new ScrapMetalBuilder()
                                                .WithAuth(authToken)
                                                .WithBrain(brain)
                                                .Build())
            {
                Debug.WriteLine("RoadRunner has built itself a ScrapMetalBot. Pressing play now!");

                await scrapMetal.ConnectAsync();
                await scrapMetal.PollAsync();

                brain = scrapMetal.Brain;
                scrapMetal.Close();
            }
        }
    }
}
