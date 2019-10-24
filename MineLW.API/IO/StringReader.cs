using System;
using System.Globalization;
using System.Text;

namespace MineLW.API.IO
{
    public class StringReader
    {
        private static readonly char[] LineSeparator = {'\r', '\n'};
        
        private const char SyntaxEscape = '\\';
        private const char SyntaxQuote = '"';

        public int Available => _input.Length - Position;
        public string InputRead => _input.Substring(0, Position);
        public string InputRemaining => _input.Substring(Position);

        public string Line
        {
            get
            {
                var read = InputRead;
                var index = read.LastIndexOfAny(LineSeparator);
                return index == -1 ? read : read.Substring(index);
            }
        }

        private readonly string _input;
        
        public int Position;

        public StringReader(string input)
        {
            _input = input;
        }

        public bool CanRead(int length = 1)
        {
            return Position + length <= _input.Length;
        }

        public char Next(int offset = 0)
        {
            return _input[Position + offset];
        }

        public char Read()
        {
            return _input[Position++];
        }

        public void Skip()
        {
            Position++;
        }

        public void ConsumeWhitespaces()
        {
            while (CanRead() && char.IsWhiteSpace(Next()))
                Skip();
        }

        public void Skip(char c)
        {
            while (CanRead() && Next() == c)
                Skip();
        }

        public void EnsureNext(char expected)
        {
            var next = Read();
            if (next != expected)
            {
                throw new FormatException(
                    "Invalid character (expected: \"" + expected + "\", found: \"" + next + "\")"
                );
            }
        }

        public void EnsureNext(params char[] expected)
        {
            foreach (var c in expected)
            {
                var next = Read();
                if (next != c)
                {
                    throw new FormatException(
                        "Invalid character (expected: \"" + expected + "\", found: \"" + next + "\")"
                    );
                }
            }
        }

        public string ReadUnquotedString()
        {
            var start = Position;
            while (CanRead())
            {
                var c = Next();
                if (char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.' || c == '+')
                    Skip();
                else
                    break;
            }

            return _input.Substring(start, Position - start);
        }

        public string ReadQuotedString()
        {
            if (!CanRead())
                return string.Empty;

            var c = Read();
            if (c != SyntaxQuote)
                throw new FormatException("Expected start of quote");

            var result = new StringBuilder();
            var escaped = false;
            while (CanRead())
            {
                c = Read();
                if (escaped)
                {
                    if (c == SyntaxQuote || c == SyntaxEscape)
                    {
                        result.Append(c);
                        escaped = false;
                    }
                    else
                    {
                        Position--;
                        throw new FormatException("Invalid escape: " + c.ToString(CultureInfo.InvariantCulture));
                    }
                }
                else if (c == SyntaxEscape)
                    escaped = true;
                else if (c == SyntaxQuote)
                    return result.ToString();
                else
                    result.Append(c);
            }

            throw new FormatException("Expected end of quote");
        }

        public string ReadString()
        {
            if (!CanRead())
                throw new FormatException("Empty string");

            return Next() == SyntaxQuote ? ReadQuotedString() : ReadUnquotedString();
        }

        public float ReadFloat()
        {
            if (!CanRead())
                throw new FormatException("Float expected");
            
            var start = Position;
            
            var c = Next();
            if (char.IsDigit(c) || c == '.' || c == '-')
            {
                Skip();
                while (CanRead() && (char.IsDigit(c = Next()) || c == '.'))
                    Skip();
            }

            var rawNum = _input.Substring(start, Position - start);
            if (float.TryParse(
                rawNum,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent,
                NumberFormatInfo.InvariantInfo,
                out var number
            ))
                return number;

            Position = start;
            throw new FormatException("Invalid float \"" + rawNum + '"');
        }

        public int ReadInteger()
        {
            var start = Position;

            if (!CanRead())
                throw new FormatException("Integer expected");

            var c = Next();
            if (char.IsDigit(c) || c == '-')
            {
                Skip();
                while (CanRead() && char.IsDigit(Next()))
                    Skip();
            }

            var rawNum = _input.Substring(start, Position - start);
            if (int.TryParse(
                rawNum, 
                NumberStyles.AllowLeadingSign | NumberStyles.AllowExponent,
                NumberFormatInfo.InvariantInfo,
                out var number
            ))
                return number;

            Position = start;
            throw new FormatException("Invalid integer \"" + rawNum + '"');
        }

        public double ReadDouble()
        {
            var start = Position;

            if (!CanRead())
                throw new FormatException("Double expected");

            var c = Next();
            if (char.IsDigit(c) || c == '.' || c == '-')
            {
                Skip();
                while (CanRead() && (char.IsDigit(c = Next()) || c == '.'))
                    Skip();
            }

            var rawNum = _input.Substring(start, Position - start);
            if (double.TryParse(rawNum,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent,
                NumberFormatInfo.InvariantInfo,
                out var number
            ))
                return number;

            Position = start;
            throw new FormatException("Invalid double \"" + rawNum + '"');
        }

        public bool ReadBoolean()
        {
            var start = Position;

            var value = ReadString();
            if (value.Length == 0)
                throw new FormatException("Boolean expected");

            if (value.Equals("true", StringComparison.Ordinal))
                return true;

            if (value.Equals("false", StringComparison.Ordinal))
                return false;

            Position = start;
            throw new FormatException("Invalid boolean");
        }
    }
}