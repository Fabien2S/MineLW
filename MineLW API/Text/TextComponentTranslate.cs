namespace MineLW.API.Text
{
    public class TextComponentTranslate : TextComponent
    {
        public override string Id => "translate";

        public readonly string[] Parameters;

        public TextComponentTranslate(string key, params string[] parameters) : base(key)
        {
            Parameters = parameters;
        }
    }
}