using System.Text;
using MineLW.API.Server;
using MineLW.API.Text;
using NLog;

namespace MineLW.Server
{
    public class ConsoleHandler : IConsoleHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void SendMessage(TextComponent component)
        {
            var builder = new StringBuilder();
            FormatComponent(builder, component);
            Logger.Info(builder);
        }

        private static void FormatComponent(StringBuilder builder, TextComponent component)
        {
            builder.Append(component.Value);
            foreach (var child in component.Children)
                FormatComponent(builder, child);
        }
    }
}