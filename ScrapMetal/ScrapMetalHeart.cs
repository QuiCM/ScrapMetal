using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.models;

namespace ScrapMetal
{
    /// <summary>
    /// Manages Discord heartbeat operations
    /// </summary>
    public class ScrapMetalHeart
    {
        private readonly ScrapMetalBot _scrapMetal;
        private int _beatDelay;
        private CancellationToken _token;

        internal ScrapMetalHeart(ScrapMetalBot scrapMetal)
        {
            _scrapMetal = scrapMetal;
        }

        internal void Beat()
        {
            //We fire and forget this because we don't really need to keep control of it
            BeatAsync(_beatDelay, _token);
        }

        internal async Task FirstBeat(int delay, CancellationToken token)
        {
            Debug.WriteLine("ScrapMetalHeart starts beating.");
            Random r = new();
            _beatDelay = delay;
            _token = token;

            int randomizedDelay = (int)(delay * r.NextDouble());
            Debug.WriteLine($"Waiting {randomizedDelay}ms then heartbeating. Regular heartbeat interval: {delay}ms");

            await Task.Delay(randomizedDelay, token);
            await _scrapMetal.SendAsync(JsonSerializer.Serialize(new gateway_heartbeat { d = _scrapMetal._brain.LastSequence }));
        }

        private async Task BeatAsync(int delay, CancellationToken token)
        {
            //This beat should just recursively call itself forever until the token is cancelled
            Debug.WriteLine("Beating normally.");

            await Task.Delay(delay, token);

            if (token.IsCancellationRequested)
            {
                return;
            }

            await _scrapMetal.SendAsync(JsonSerializer.Serialize(new gateway_heartbeat { d = _scrapMetal._brain.LastSequence }));
            await BeatAsync(delay, token);
        }
    }
}