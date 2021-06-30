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

            Debug.WriteLine($"Using commandline auth token: {authToken}");

            await StayConnectedAsync(authToken);

            Debug.WriteLine("RoadRunner disappearing into the sunset.");
        }

        static async Task StayConnectedAsync(string auth, ScrapMetalBrain savedBrain = null)
        {
            //Attempt to connect, retrieving the brain once the connection ends
            ScrapMetalBrain brain = await ConnectAsync(auth, savedBrain);

            // According to Discord docs: bots should wait 3-5 seconds before trying to reconnect in the event of a disconnect
            // Also this gives plenty of time for cancellations to propagate
            Debug.WriteLine("Waiting for ScrapMetalBot to rust away.");
            await Task.Delay(3000);

            Debug.WriteLine("Building a new ScrapMetalBot!");
            // Reconnect with the same brain
            await StayConnectedAsync(auth, brain);
        }

        static async Task<ScrapMetalBrain> ConnectAsync(string auth, ScrapMetalBrain savedBrain = null)
        {
            // Build a new bot
            using ScrapMetalBot scrapMetal = new ScrapMetalBuilder()
                                                  .WithAuth(auth)
                                                  .WithBrain(savedBrain)
                                                  .Build();
            Debug.WriteLine("RoadRunner has built itself a ScrapMetalBot. Pressing play now!");

            // Connect and poll. PollAsync will return once the bot has disconnected
            await scrapMetal.ConnectAsync();
            await scrapMetal.PollAsync();

            // At this point we have disconnected. Retrieve the brain for reuse, and flag the ScrapMetal instance for closure
            // Technically it should already do this while disposing, but no harm in making sure
            ScrapMetalBrain brain = scrapMetal.Brain;
            scrapMetal.Close();

            Debug.WriteLine("ScrapMetal fell apart.");
            // Pass the brain back for re-use
            return brain;
        }
    }
}
