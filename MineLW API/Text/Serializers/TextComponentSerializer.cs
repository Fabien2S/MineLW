using System;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                StyleNames[i] = name.ToLowerInvariant();
            }
        }

        public override TextComponent Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }


        public override void Write(Utf8JsonWriter writer, TextComponent component, JsonSerializerOptions options)
        {
            var current = component;
            while (current.Parent != null)
                current = current.Parent;

            WriteComponent(writer, current, TextColor.White, TextStyles.None);
        }

        private static void WriteComponent(Utf8JsonWriter writer, TextComponent component, TextColor currentColor,
            TextStyles currentStyle)
        {
            writer.WriteStartObject();
            writer.WriteString(component.Id, component.Value);

            if (component.HasColor)
            {
                var color = component.Color;
                if (color != currentColor)
                    writer.WriteString("color", component.Color.Name);
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

                    writer.WriteBoolean(StyleNames[i], hasStyle);
                }
            }

            if (component.ChildCount > 0)
            {
                writer.WriteStartArray("extra");
                foreach (var child in component)
                    WriteComponent(writer, child, component.Color, component.Style);
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
    }
}