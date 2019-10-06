using System;
using MineLW.API.Extensions;
using Newtonsoft.Json;

namespace MineLW.API.Text.Serializers
{
    public class TextComponentSerializer : JsonConverter<TextComponent>
    {
        private static readonly TextStyles[] Styles =
        {
            TextStyles.Bold, TextStyles.Italic, TextStyles.Underlined, TextStyles.Strikethrough, TextStyles.Obfuscated
        };

        private static readonly string[] StyleNames = new string[Styles.Length];

        static TextComponentSerializer()
        {
            for (var i = 0; i < Styles.Length; i++)
            {
                var style = Styles[i];
                var name = Enum.GetName(typeof(TextStyles), style);
                if (name == null)
                    throw new NullReferenceException("No name for style " + style);
                StyleNames[i] = name.ToUnderscoreCase();
            }
        }

        public override void WriteJson(JsonWriter writer, TextComponent value, JsonSerializer serializer)
        {
            WriteComponent(writer, value);
        }

        public override TextComponent ReadJson(JsonReader reader, Type objectType, TextComponent existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public static void WriteComponent(
            JsonWriter writer,
            TextComponent component,
            TextColor currentColor = TextColor.White,
            TextStyles currentStyle = TextStyles.None
        )
        {
            writer.WriteStartObject();

            writer.WritePropertyName(component.Id);
            writer.WriteValue(component.Value);

            if (component.HasColor)
            {
                var color = component.Color;
                if (color != currentColor)
                {
                    writer.WritePropertyName("color");
                    writer.WriteValue(color.GetName());
                }
            }

            if (component.HasStyle)
            {
                for (var i = 0; i < Styles.Length; i++)
                {
                    var style = Styles[i];
                    var hasStyle = component.Style.HasFlag(style);
                    var parentHasStyle = currentStyle.HasFlag(style);
                    if (hasStyle == parentHasStyle)
                        continue;

                    writer.WritePropertyName(StyleNames[i]);
                    writer.WriteValue(hasStyle);
                }
            }

            var children = component.Children;
            if (children.Count > 0)
            {
                writer.WritePropertyName("extra");
                writer.WriteStartArray();
                foreach (var child in children)
                    WriteComponent(writer, child, component.Color, component.Style);
                writer.WriteEndArray();
            }

            switch (component)
            {
                case TextComponentTranslate componentTranslate:
                {
                    var parameters = componentTranslate.Parameters;
                    if (parameters.Length > 0)
                    {
                        writer.WritePropertyName("with");
                        writer.WriteStartArray();
                        foreach (var parameter in parameters)
                            writer.WriteValue(parameter);
                        writer.WriteEndArray();
                    }

                    break;
                }
            }

            writer.WriteEndObject();
        }
    }
}