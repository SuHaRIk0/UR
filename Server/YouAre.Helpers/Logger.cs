using Spectre.Console;

using YouAre.Domain;

namespace YouAre.Helpers
{
    public static class Logger
    {
        public static void Information(string message)
        {
            AnsiConsole.Markup(
                $"[lightcyan1]{DateTime.Now} - [skyblue3]information:\n\t[/]" +
                message +
                $"[/]\n"
            );
        }

        public static void Debug(string message)
        {
            AnsiConsole.Markup(
                $"[lightcyan1]{DateTime.Now} - [lightsalmon3_1]debug information:\n\t[/]" +
                message +
                $"[/]\n"
            );
        }

        public static void Warning(string message)
        {
            AnsiConsole.Markup(
                $"[lightcyan1]{DateTime.Now} - [sandybrown]warning:\n\t[/]" +
                message +
                $"[/]\n"
            );
        }

        public static void Error(string description, QueryException exception)
        {
            AnsiConsole.Markup(
                $"[lightcyan1]{DateTime.Now} - [orchid2]exception:\n\t[/]" +
                description +
                "\n\t" + exception.what +
                $"[/]\n"
            );
        }
    }
}