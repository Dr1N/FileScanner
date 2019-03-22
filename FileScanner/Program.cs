using Autofac;
using FileScanner.Core;
using FileScanner.Interfaces;
using System;

namespace FileScanner
{
    class Program
    {
        static IContainer _container;

        static void Main(string[] args)
        {
            if (Initialize(args) && _container != null)
            {
                using (var scope = _container.BeginLifetimeScope())
                {
                    IScanner app = scope.Resolve<IScanner>();
                    var task = app.Run();
                    WaitUserInput(app);
                    task.Wait();
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
        }

        static bool Initialize(string[] args)
        {
            var result = false;
            var parameters = CommandLineParameters.MakeParameters(args);
            if (parameters?.IsValid() == true)
            {
                try
                {
                    InitializeContainer(parameters);
                    result = true;
                }
                catch (Exception ex)
                {
                    PrintMessage($"Container initialization error: {ex.Message}", ConsoleColor.Red);
                }
            }
            else
            {
                PrintMessage("Invalid command line arguments", ConsoleColor.Red);
            }

            return result;
        }

        static void InitializeContainer(CommandLineParameters parameters)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<StrPathCalculator>().As<IPathCalculator>().SingleInstance();

            builder.RegisterType<ConsolePrinter>().As<IPrinter>().SingleInstance();

            builder.Register(
                ctx => parameters.IsConsole() ? (IResultWriter)(new ConsoleWriter()) : (new FileWriter(parameters.ResultFile, ctx.Resolve<IPrinter>()))
            ).SingleInstance();

            builder.Register(
                ctx =>
                {
                    IFileHandler result = null;
                    IResultWriter writer = ctx.Resolve<IResultWriter>();
                    IPathCalculator calculator = ctx.Resolve<IPathCalculator>();
                    IPrinter printer = ctx.Resolve<IPrinter>();
                    switch (parameters.GetAction())
                    {
                        case Core.Action.All:
                            result = new SimpleFileHandler(printer);
                            break;
                        case Core.Action.Cs:
                            result = new CsFileHandler(printer);
                            break;
                        case Core.Action.ReversedOne:
                            result = new ReverseOneFileHandler(printer);
                            break;
                        case Core.Action.ReversedTwo:
                            result = new ReverseTwoFileHandler(printer);
                            break;
                        default:
                            throw new Exception($"Invalid action: [{parameters.ActionStr}]");
                    }

                    return result;
                }
            ).InstancePerLifetimeScope();

            builder.Register<IDirectoryHandler>(
                ctx => new DirectoryHandler(parameters.StartDirectory, 
                    ctx.Resolve<IFileHandler>(), 
                    ctx.Resolve<IResultWriter>(),
                    ctx.Resolve<IPathCalculator>(),
                    ctx.Resolve<IPrinter>())
            ).InstancePerLifetimeScope();

            builder.RegisterType<Scanner>().As<IScanner>().InstancePerLifetimeScope();

            _container = builder.Build();
        }

        static void PrintMessage(string message, ConsoleColor color = ConsoleColor.White)
        {
            try
            {
                lock (typeof(Console))
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine(message);
                    Console.ResetColor();
                }
            }
            catch
            {
                // ignore
            }
        }

        static void WaitUserInput(IScanner app)
        {
            PrintMessage("Processing (to cancel or exit press [Esc])...", ConsoleColor.Green);
            while (true)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    app?.Cancel();
                    break;
                }
            }
        }
    }
}