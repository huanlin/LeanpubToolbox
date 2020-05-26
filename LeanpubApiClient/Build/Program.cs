using System;
using FlubuCore.Scripting;

namespace Build
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                args = new string[] { "compile" };
            }

            var engine = new FlubuEngine();
            engine.RunScript<BuildScript>(args);
        }
    }
}
