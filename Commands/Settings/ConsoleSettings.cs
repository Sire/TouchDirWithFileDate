using System.ComponentModel;
using Spectre.Console.Cli;

namespace TouchDirWithFileDate.Commands.Settings
{
    public class ConsoleSettings : CommandSettings
    {

        [CommandArgument(0, "<directory>")]
        [Description("Resets the modified date on this directory to the same date as the containing files")]
        public string Directory { get; set; } = string.Empty;

        [CommandOption("--recursive|-r")]
        [Description("Process all subdirectories as well")]
        public bool Recursive { get; set; } = false;

    }
}