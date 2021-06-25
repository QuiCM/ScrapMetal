
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CandyBar.models;
using CandyBar.models.enums;
using CandyBar.models.objects;
using ScrapMetal.Configuration;

namespace ScrapMetal
{
    public class ScrapMetalBrain
    {
        internal int? _sequence;
        internal string _session;
        internal gateway_identity _identity;
        internal RuntimeConfiguration _configuration;
        internal ScrapMetalHeart _heart;
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
        }

        internal async Task HandlePayload(gateway_payload payload, CancellationToken token)
        {
            Debug.WriteLine($"Payload received: [{payload.s?.ToString() ?? "-"}] {payload.op} | {payload.t ?? "NO EVENT"} | {payload.d}");
            _sequence = payload.s;
            switch (payload.op)
            {
                case 10:
                    _heart = new ScrapMetalHeart(_scrapMetal);
                    //We fire and forget this. This gets the heart beating and will keep it beating until the token is cancelled
                    _heart.FirstBeat(payload.d.GetProperty("heartbeat_interval").GetInt32(), token);

                    await HandleGatewayConnect();
                    break;
                case 9:
                    await HandleGatewayConnect(reidentify: true);
                    break;
                case 0:
                    if (payload.t == "READY")
                    {
                        HandleReadyEvent(payload);
                    }
                    else
                    {
                        await HandleEvent(payload.t, payload.d);
                    }
                    break;
                case 7:
                    HandleReconnect();
                    break;
            }
        }

        internal async Task HandleEvent(string eventType, dynamic data)
        {
            if (eventType == payload_event_types.MESSAGE_CREATE)
            {
                message_object message = ((JsonElement)data).ToObject<message_object>();
                await HandleMessage(message);
            }

            if (eventType == payload_event_types.INTERACTION_CREATE)
            {
                interaction_object interaction = ((JsonElement)data).ToObject<interaction_object>();
                await _scrapMetal._http.AcknowledgeInteraction(interaction, CancellationToken.None);

                message_object message = interaction.message;
                if (interaction.data.custom_id == "__test_button_bad")
                {
                    message.components[0].components[0].disabled = false;
                    message.components[0].components[1].disabled = true;
                    message.embeds[0].color = 15158332; //Red
                }
                else
                {
                    message.components[0].components[0].disabled = true;
                    message.components[0].components[1].disabled = false;
                    message.embeds[0].color = 5763719; //Green
                }

                await _scrapMetal._http.PatchInteraction(interaction, message, CancellationToken.None);
                await _scrapMetal._http.DeleteInteractionResponse(interaction, message, CancellationToken.None);
            }
        }

        internal async Task HandleMessage(message_object message)
        {
            if (message.content.StartsWith("echo") && message.author.id == "164210142789894144")
            {
                await _scrapMetal._http.CreateMessage(msg =>
                        msg.InReplyTo(message)
                        .WithEmbed(embed =>
                            embed.WithTitle("This is an embed title!")
                                 .WithDescription("This is an embed!")
                                 .WithColor(16705372) //Yellow
                        ).WithComponent(row =>
                            row.WithButton(button =>
                                button.WithStyle(button_style.Success)
                                      .WithLabel("Test button!")
                                      .WithCustomId("__test_button")
                                      .Enable()
                            ).WithButton(button =>
                                button.WithStyle(button_style.Danger)
                                      .WithLabel("Bad test button!")
                                      .WithCustomId("__test_button_bad")
                                      .Enable()
                            )
                        ),
                    CancellationToken.None
                );
            }
        }

        internal void HandleReconnect()
        {
            Debug.WriteLine("Received op 7 - reconnect. Cancelling token");
            _scrapMetal.Close();
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