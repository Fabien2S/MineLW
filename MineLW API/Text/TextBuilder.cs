﻿namespace MineLW.API.Text
{
    public class TextBuilder
    {
        private ITextComponent _root;
        private ITextComponent _current;

        public TextBuilder Text(string text)
        {
            var current = new TextComponentString(text);
            //_current.Append(current);
            _current = current;
            return this;
        }
    }
}