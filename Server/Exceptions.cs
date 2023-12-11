namespace Server
{
    public class Exception
    {
		public string what { get; set; } = string.Empty;
	}


    public class NotFoundException : Exception
	{
        public NotFoundException(string exception)
        {
            this.what = "[orchid2]" + exception + " was not found![/]";
        }
    }

	public class WasEmptyException : Exception
	{
		public WasEmptyException(string exception)
		{
			this.what = "[orchid2]" + exception + " was empty![/]";
		}
	}
}
