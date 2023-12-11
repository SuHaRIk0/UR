namespace Server.Helpers
{
	public class Title
	{
		public static void header()
		{
			AnsiConsole.Markup(
				"[aquamarine3]" +
				"YouAre Web API Application\tASP.NET Core & .NET 8.0\n" +
				"All Rights Reserved @ 2023\n\n" +
#if DEBUG
				"Version: 0.0.1.4\n\n" +
#else
				"Release Version: 0.0.1.4\n\n" +
#endif
				"[lightcyan1]NATIVE CODE PUBLICATION[/]\n\n" +
				"Documentation Route: [lightslateblue]/swagger[/]\n\n" +
				"Listening to ports:\n" +
				"https: [lightslateblue]https://localhost:7014[/]\n" +
				"http: [lightslateblue]http://localhost:5043[/]\n\n" +
				"Environment status: [darkkhaki]Development[/]\n\n" +
				"[/]"
#if DEBUG
				+ "[deeppink4_1]RUNNING DEBUG MODE...[/]\n\n"
#endif
			);
		}
	}
}
