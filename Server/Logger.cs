namespace Server
{
	public static class Logger
	{
		public static void information(string message)
		{
			AnsiConsole.Markup(
				$"[lightcyan1]{DateTime.Now} - [skyblue3]information:\n\t[/]" +
				message +
				$"[/]\n"
			);
		}
		public static void debug(string message)
		{
			AnsiConsole.Markup(
				$"[lightcyan1]{DateTime.Now} - [lightsalmon3_1]debug information:\n\t[/]" +
				message +
				$"[/]\n"
			);
		}
		public static void warning(string message)
		{
			AnsiConsole.Markup(
				$"[lightcyan1]{DateTime.Now} - [sandybrown]warning:\n\t[/]" +
				message +
				$"[/]\n"
			);
		}
		public static void error(string description, Exception exception)
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
