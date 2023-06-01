using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using TouchDirWithFileDate.Commands.Settings;

namespace TouchDirWithFileDate.Commands
{
    public class TouchCommand : AsyncCommand<ConsoleSettings>
    {
        private ILogger Logger { get; }

        public override async Task<int> ExecuteAsync(CommandContext context, ConsoleSettings settings)
        {
            Console.WriteLine("Changes the modified date on a directory to the same date as the median of the containing files.");
            Console.WriteLine($"Processing directory: {settings.Directory} {(settings.Recursive?"RECURSIVE": "")}");

            new TouchDirectory(settings).Execute();

            return await Task.FromResult(0);
        }

        public TouchCommand(ILogger<TouchCommand> logger)
        {
            Logger = logger;
        }
    }
}