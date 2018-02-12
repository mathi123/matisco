using System;

namespace Example.BusinessApp.Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Matisco.Server.Host.Program.Main<BootstrapInfo>(args);
        }
    }
}
