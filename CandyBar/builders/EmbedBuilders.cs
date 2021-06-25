using System;
using CandyBar.models.objects.embeds;

namespace CandyBar.builders
{
    public class EmbedBuilder
    {
        private readonly embed_object _embed;
        public embed_object Embed => _embed;

        internal EmbedBuilder()
        {
            _embed = new();
        }

        public static EmbedBuilder Create()
        {
            return new EmbedBuilder();
        }

        public EmbedBuilder WithTitle(string title)
        {
            _embed.title = title;
            return this;
        }

        public EmbedBuilder WithDescription(string description)
        {
            _embed.description = description;
            return this;
        }

        public EmbedBuilder WithUrl(string url)
        {
            _embed.url = url;
            return this;
        }

        public EmbedBuilder WithTimestamp(DateTime timestamp)
        {
            _embed.timestamp = timestamp;
            return this;
        }

        public EmbedBuilder WithColor(int color)
        {
            _embed.color = color;
            return this;
        }

        public EmbedBuilder WithFooter(Action<EmbedFooterBuilder> footerBuilder)
        {
            EmbedFooterBuilder footer = new();
            footerBuilder(footer);
            _embed.footer = footer.Footer;

            return this;
        }

        public EmbedBuilder WithImage(Action<EmbedMediaBuilder> mediaBuilder)
        {
            EmbedMediaBuilder media = new();
            mediaBuilder(media);
            _embed.image = media.Media;

            return this;
        }

        public EmbedBuilder WithThumbnail(Action<EmbedMediaBuilder> mediaBuilder)
        {
            EmbedMediaBuilder media = new();
            mediaBuilder(media);
            _embed.thumbnail = media.Media;

            return this;
        }

        public EmbedBuilder WithVideo(Action<EmbedMediaBuilder> mediaBuilder)
        {
            EmbedMediaBuilder media = new();
            mediaBuilder(media);
            _embed.video = media.Media;

            return this;
        }

        public EmbedBuilder WithProvider(Action<EmbedProviderBuilder> providerBuilder)
        {
            EmbedProviderBuilder provider = new();
            providerBuilder(provider);
            _embed.provider = provider.Provider;

            return this;
        }

        public EmbedBuilder WithAuthor(Action<EmbedAuthorBuilder> authorBuilder)
        {
            EmbedAuthorBuilder author = new();
            authorBuilder(author);
            _embed.author = author.Author;

            return this;
        }

        public EmbedBuilder WithField(Action<EmbedFieldBuilder> fieldBuilder)
        {
            if (_embed.fields == null)
            {
                _embed.fields = new();
            }

            EmbedFieldBuilder field = new();
            fieldBuilder(field);
            _embed.fields.Add(field.Field);

            return this;
        }
    }

    public class EmbedFooterBuilder
    {
        private readonly embed_footer_object _footer;
        public embed_footer_object Footer => _footer;

        internal EmbedFooterBuilder()
        {
            _footer = new();
        }

        public EmbedFooterBuilder WithIconUrl(string iconUrl)
        {
            _footer.icon_url = iconUrl;
            return this;
        }

        public EmbedFooterBuilder WithText(string text)
        {
            _footer.text = text;
            return this;
        }
    }

    public class EmbedMediaBuilder
    {
        private readonly embed_media_object _media;
        public embed_media_object Media => _media;

        internal EmbedMediaBuilder()
        {
            _media = new();
        }

        public EmbedMediaBuilder WithUrl(string url)
        {
            _media.url = url;
            return this;
        }

        public EmbedMediaBuilder WithHeight(int height)
        {
            _media.height = height;
            return this;
        }

        public EmbedMediaBuilder WithWidth(int width)
        {
            _media.width = width;
            return this;
        }
    }

    public class EmbedProviderBuilder
    {
        private readonly embed_provider_object _provider;
        public embed_provider_object Provider => _provider;

        internal EmbedProviderBuilder()
        {
            _provider = new();
        }

        public EmbedProviderBuilder WithName(string name)
        {
            _provider.name = name;
            return this;
        }

        public EmbedProviderBuilder WithUrl(string url)
        {
            _provider.url = url;
            return this;
        }
    }

    public class EmbedAuthorBuilder
    {
        private readonly embed_author_object _author;
        public embed_author_object Author => _author;

        internal EmbedAuthorBuilder()
        {
            _author = new();
        }

        public EmbedAuthorBuilder WithName(string name)
        {
            _author.name = name;
            return this;
        }

        public EmbedAuthorBuilder WithIconUrl(string iconUrl)
        {
            _author.icon_url = iconUrl;
            return this;
        }

        public EmbedAuthorBuilder WithUrl(string url)
        {
            _author.url = url;
            return this;
        }
    }

    public class EmbedFieldBuilder

    {
        private readonly embed_field_object _field;
        public embed_field_object Field => _field;

        internal EmbedFieldBuilder()
        {
            _field = new();
        }

        public EmbedFieldBuilder WithName(string name)
        {
            _field.name = name;
            return this;
        }

        public EmbedFieldBuilder WithValue(string value)
        {
            _field.value = value;
            return this;
        }

        public EmbedFieldBuilder Inline(bool inline = true)
        {
            _field.inline = inline;
            return this;
        }
    }
}