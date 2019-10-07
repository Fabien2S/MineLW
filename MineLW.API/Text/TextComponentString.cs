namespace MineLW.API.Text
{
    public class TextComponentString : TextComponent
    {
        public override string Id => "text";

        public TextComponentString(string text = null) : base(text)
        {
        }
    }
}