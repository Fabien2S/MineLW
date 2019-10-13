using System;
using System.Linq;
using MineLW.API.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var root = JObject.Load(reader);
            
            TextComponent component;

            if(root.ContainsKey("text"))
            {
                var value = root.Value<string>("text");
                component = new TextComponentString(value);
            }
            else if (root.ContainsKey("translate"))
            {
                var value = root.Value<string>("translate");
                var parameters = root.Values<string>("with").ToArray();
                component = new TextComponentTranslate(value, parameters);
            }
            else if (root.ContainsKey("keybind"))
            {
                var value = root.Value<string>("keybind");
                component = new TextComponentKeybind(value);
            }
            else
                throw new JsonException("Invalid text component");

            if (root.ContainsKey("color"))
            {
                var color = root.Value<string>("color");
                component.Color = TextColorExtensions.FromName(color);
            }

            for (var i = 0; i < Styles.Length; i++)
            {
                var styleName = StyleNames[i];
                if (!root.ContainsKey(styleName))
                    continue;
                
                var hasStyle = root.Value<bool>(styleName);
                var style = Styles[i];
                if (hasStyle)
                    component.Style |= style;
                else
                    component.Style &= ~style;
            }

            if (!root.ContainsKey("extra"))
                return component;
            
            var array = root.Value<JArray>("extra");
            foreach (var token in array)
                component.Children.Add(token.ToObject<TextComponent>());
            return component;
        }

        public static void WriteComponent(
            JsonWriter writer,
            TextComponent component,
            TextColor currentColor = TextColor.White,
            TextStyles currentStyle = TextStyles.None
        )
        {
            writer.WriteStartObject();
            
            switch (component)
            {
                case TextComponentString _:
                    writer.WritePropertyName("text");
                    writer.WriteValue(component.Value);
                    break;
                case TextComponentTranslate componentTranslate:
                {
                    writer.WritePropertyName("translate");
                    writer.WriteValue(component.Value);
                    
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
                case TextComponentKeybind _:
                    writer.WritePropertyName("keybind");
                    writer.WriteValue(component.Value);
                    break;
            }
            
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

            writer.WriteEndObject();
        }
    }
}