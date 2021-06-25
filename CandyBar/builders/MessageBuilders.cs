using System;
using CandyBar.models.objects;
using CandyBar.models.objects.embeds;

namespace CandyBar.builders
{
    public class MessageBuilder
    {
        private readonly message_object _message;
        public message_object Message => _message;

        private MessageBuilder()
        {
            _message = new message_object();
        }

        public static MessageBuilder Create()
        {
            return new MessageBuilder();
        }

        public MessageBuilder ToChannel(string channelId)
        {
            _message.channel_id = channelId;
            return this;
        }

        public MessageBuilder ReplyTo(message_object message) => ToChannel(message.channel_id);

        public MessageBuilder WithContent(string content)
        {
            _message.content = content;
            return this;
        }

        public MessageBuilder WithEmbed(Action<EmbedBuilder> builderFunc)
        {
            if (_message.embeds == null)
            {
                _message.embeds = new();
            }
            EmbedBuilder builder = EmbedBuilder.Create();
            builderFunc(builder);

            _message.embeds.Add(builder.Embed);
            return this;
        }

        public MessageBuilder WithAttachment(attachment_object attachment)
        {
            if (_message.attachments == null)
            {
                _message.attachments = new();
            }

            _message.attachments.Add(attachment);
            return this;
        }

        public MessageBuilder WithComponent(Action<ActionRowBuilder> builderFunc)
        {
            if (_message.components == null)
            {
                _message.components = new();
            }

            ActionRowBuilder builder = ActionRowBuilder.Create();
            builderFunc(builder);
            _message.components.Add(builder.Component);
            return this;
        }
    }
}