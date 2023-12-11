namespace Server.Models.Items
{
	public class Publication
	{
		[Key]		public int ID { get; set; }
		[Required]	public string Name { get; set; } = string.Empty;
		[Required]	public string Description { get; set; } = string.Empty;

		public static Publication Empty = new Publication()
		{
			ID = -1,
			Name = string.Empty,
			Description = string.Empty
		};
	}

	[JsonSerializable(typeof(Publication[]))]
	public partial class ApiJsonSerializerPublication : JsonSerializerContext
	{

	}
}
