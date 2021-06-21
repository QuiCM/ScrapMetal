
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using CandyBar.models;
using ScrapMetal.Configuration;

namespace ScrapMetal
{
    public class ScrapMetalBrain
    {
        internal int? _sequence;
        internal string _session;
        internal gateway_identity _identity;
        internal RuntimeConfiguration _configuration;
        private ScrapMetalBot _scrapMetal;

        public int? LastSequence => _sequence;

        public ScrapMetalBrain()
        {
            Debug.WriteLine("ScrapMetalBrain begins thinking.");
        }

        internal void UpdateScrapMetal(ScrapMetalBot scrapMetal)
        {
            _scrapMetal = scrapMetal;
        }

        internal void UpdateConfiguration(RuntimeConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void BuildIdentity()
        {
            _identity = new()
            {
                token = _configuration.AuthToken.Value,
                intents = (int)(gateway_intent_bitflags.GUILDS | gateway_intent_bitflags.GUILD_MEMBERS | gateway_intent_bitflags.GUILD_MESSAGES | gateway_intent_bitflags.GUILD_MESSAGE_REACTIONS | gateway_intent_bitflags.GUILD_MESSAGE_TYPING)
            };
            _identity.properties.Add("$os", "windows");
            _identity.properties.Add("$browser", "ScrapMetal");
            _identity.properties.Add("$device", "ScrapMetal");

            Debug.WriteLine(_identity.intents);
        }

        internal void HandleReadyEvent(gateway_payload payload)
        {
            string session = payload.d.GetProperty("session_id").GetString();
            _configuration.SessionId.Value = session;
            _session = session;
        }

        internal async Task HandleGatewayConnect(bool reidentify = false)
        {
            async Task SendIdentify()
            {
                BuildIdentity();
                await _scrapMetal.SendAsync(JsonSerializer.Serialize(new gateway_payload
                {
                    op = 2,
                    d = _identity
                }));
            }

            async Task SendResume()
            {
                await _scrapMetal.SendAsync(JsonSerializer.Serialize(new gateway_payload
                {
                    op = 6,
                    d = new
                    {
                        token = _configuration.AuthToken.Value,
                        session_id = _session ?? _configuration.SessionId.Value, //trust brain session over config session
                        seq = _sequence
                    }
                }));
            }

            if (reidentify)
            {
                await SendIdentify();
            }
            //If we have an old session ID, we can try reconnecting with it
            else if (_configuration.SessionId.Value != null || _session != null)
            {
                await SendResume();
            }
            else
            {
                await SendIdentify();
            }
        }
    }
}