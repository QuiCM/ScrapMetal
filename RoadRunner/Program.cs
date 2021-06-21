using System.Diagnostics;
using System.Threading.Tasks;
using CandyBar.models;
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

            ulong permissions = (ulong)(permission_bitflags.MANAGE_CHANNELS | permission_bitflags.ADD_REACTIONS | permission_bitflags.VIEW_CHANNEL | permission_bitflags.SEND_MESSAGES
                    | permission_bitflags.EMBED_LINKS | permission_bitflags.ATTACH_FILES | permission_bitflags.READ_MESSAGE_HISTORY | permission_bitflags.USE_EXTERNAL_EMOJIS
                    | permission_bitflags.USE_SLASH_COMMANDS | permission_bitflags.USE_PUBLIC_THREADS);
            Debug.WriteLine($"Join URL: https://discord.com/oauth2/authorize?client_id=<client id>&scope=bot&permissions={permissions}");

            Debug.WriteLine($"Shoelaces found: {authToken}");

            await StayConnectedAsync(authToken);

            Debug.WriteLine("Expected exit point reached.");
        }

        static async Task<ScrapMetalBrain> ConnectAsync(string auth, ScrapMetalBrain savedBrain = null)
        {
            using ScrapMetalBot scrapMetal = new ScrapMetalBuilder()
                                                  .WithAuth(auth)
                                                  .WithBrain(savedBrain)
                                                  .Build();
            Debug.WriteLine("RoadRunner has built itself a ScrapMetalBot. Pressing play now!");

            await scrapMetal.ConnectAsync();
            await scrapMetal.PollAsync();

            ScrapMetalBrain brain = scrapMetal.Brain;
            scrapMetal.Close();

            Debug.WriteLine("ScrapMetal fell apart.");

            return brain;
        }

        static async Task StayConnectedAsync(string auth, ScrapMetalBrain savedBrain = null)
        {
            ScrapMetalBrain brain = await ConnectAsync(auth, savedBrain);

            Debug.WriteLine("Waiting for ScrapMetalBot to rust away.");
            await Task.Delay(3000);

            Debug.WriteLine("Building a new ScrapMetalBot!");
            await StayConnectedAsync(auth, brain);
        }
    }
}
