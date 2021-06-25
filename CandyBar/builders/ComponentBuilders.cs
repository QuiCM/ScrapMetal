using System;
using CandyBar.models.enums;
using CandyBar.models.objects;

namespace CandyBar.builders
{

    public abstract class ComponentBuilder
    {
        protected component_object _component;
        public component_object Component => _component;
    }

    public class ActionRowBuilder : ComponentBuilder
    {
        internal ActionRowBuilder()
        {
            _component = new()
            {
                type = component_type.ActionRow,
                components = new()
            };
        }

        public static ActionRowBuilder Create()
        {
            return new();
        }

        public ActionRowBuilder WithButton(Action<ButtonBuilder> builderFunc)
        {
            ButtonBuilder builder = new(this);
            builderFunc(builder);
            _component.components.Add(builder.Component);
            return this;
        }
    }

    public class ButtonBuilder : ComponentBuilder
    {
        private readonly ActionRowBuilder _parentBuilder;
        public ActionRowBuilder Parent => _parentBuilder;

        internal ButtonBuilder(ActionRowBuilder parentBuilder)
        {
            _parentBuilder = parentBuilder;
            _component = new()
            {
                type = component_type.Button
            };
        }

        public ButtonBuilder Disable()
        {
            _component.disabled = true;
            return this;
        }

        public ButtonBuilder Enable()
        {
            _component.disabled = false;
            return this;
        }

        public ButtonBuilder WithStyle(button_style style)
        {
            _component.style = style;
            return this;
        }

        public ButtonBuilder WithLabel(string label)
        {
            _component.label = label;
            return this;
        }

        public ButtonBuilder WithCustomId(string customId)
        {
            _component.custom_id = customId;
            return this;
        }

        public ButtonBuilder WithUrl(string url)
        {
            _component.url = url;
            return this;
        }

        public ButtonBuilder WithEmoji(emoji_object emoji)
        {
            _component.emoji = emoji;
            return this;
        }
    }
}