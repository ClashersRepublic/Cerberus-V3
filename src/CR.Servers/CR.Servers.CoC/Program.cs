using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Database;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Logic.Mode;
using CR.Servers.CoC.Logic.Slots;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Core.Consoles;
using CR.Servers.Extensions;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace CR.Servers.CoC
{
    internal class Program
    {
        private const int Width = 140;
        private const int Height = 30;
        public static IConfigurationRoot Configuration { get; set; }

        private static void Main()
        {

            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config.json").Build();
            Console.Title =
                $"Clashers Republic - {Assembly.GetExecutingAssembly().GetName().Name} - {DateTime.Now.Year} ©";

            Console.SetOut(new Prefixed());
            Console.SetWindowSize(Width, Height);

            Servers.Core.Consoles.Colorful.Console.WriteWithGradient(@"
            _________.__                 .__                          __________                   ___.   .__.__        
            \_   ___ \|  | _____    _____|  |__   ___________  ______ \______   \ ____ ______  __ _\_ |__ |  | |__| ____  
            /    \  \/|  | \__  \  /  ___/  |  \_/ __ \_  __ \/  ___/  |       _// __ \\____ \|  |  \ __ \|  | |  |/ ___\ 
            \     \___|  |__/ __ \_\___ \|   Y  \  ___/|  | \/\___ \   |    |   \  ___/|  |_> >  |  / \_\ \  |_|  \  \___ 
             \______  /____(____  /____  >___|  /\___  >__|  /____  >  |____|_  /\___  >   __/|____/|___  /____/__|\___  >
                    \/          \/     \/     \/     \/           \/          \/     \/|__|             \/             \/ 
                                                                                                           Clash Edition
            ", Color.OrangeRed, Color.LimeGreen, 14);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine( @"Clashers Republic's programs are protected by our policies, available only to our partner.");
            Console.WriteLine(@"Clashers Republic's programs are under the 'Proprietary' license.");
            Console.WriteLine(@"Clashers Republic is NOT affiliated to 'Supercell Oy'.");
            Console.WriteLine(@"Clashers Republic does NOT own 'Clash of Clans', 'Boom Beach', 'Clash Royale'.");
            Console.WriteLine();
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " is now starting..." + Environment.NewLine);

            Resources.Initialize();

            Thread.Sleep(-1);
        }

        
    }
}
