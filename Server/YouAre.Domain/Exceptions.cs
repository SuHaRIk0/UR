namespace YouAre.Domain
{
    public class QueryException
    {
        public string what { get; set; } = string.Empty;
    }


    public class NotFoundException : QueryException
    {
        public NotFoundException(string exception)
        {
            this.what = "[orchid2]" + exception + " was not found![/]";
        }
    }

    public class WasEmptyException : QueryException
    {
        public WasEmptyException(string exception)
        {
            this.what = "[orchid2]" + exception + " was empty![/]";
        }
    }
}