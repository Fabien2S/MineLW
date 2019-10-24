using System;
using MineLW.API.Commands;
using MineLW.API.Commands.Attributes;

namespace MineLW.Commands.Impl
{
    public class CommandTest : ICommandNode
    {
        public string Name { get; } = "test";

        [CmdEndpoint("display")]
        public void DisplayWhatIEnter(string messageToPrint, string additionalText = "")
        {
            Console.WriteLine("Message is \"" + messageToPrint + additionalText + '"');
        }

        private class Jump : ICommandNode
        {
            public string Name { get; } = "Jump";

            [CmdEndpoint("over_the_hill")]
            public void OverTheHill(int num = 4, string messageToPrint = "msg_to_print")
            {
                Console.WriteLine("Had jump over the hill : " + num + " and said \"" + messageToPrint + "\")");
            }
        }
    }
}