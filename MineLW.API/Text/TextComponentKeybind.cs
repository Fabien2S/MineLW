namespace MineLW.API.Text
{
    public class TextComponentKeybind : TextComponent
    {
        public override string Id => "keybind";
        
        public TextComponentKeybind(string key = null) : base(key)
        {
        }
    }
}