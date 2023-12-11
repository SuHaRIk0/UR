using Server.Models.Items;

namespace Server.Models
{
	public class User
	{
		[Key] public int ID { get; set; }
		[Required] public string Username { get; set; } = string.Empty;
		[Required] public string UserTag { get; set; } = string.Empty;
		[Required] public string AboutMe { get; set; } = string.Empty;

		public static User Empty = new User()
		{
			ID = -1,
		};
	}

	[JsonSerializable(typeof(User[]))]
	public partial class ApiJsonSerializerUser : JsonSerializerContext
	{

	}
}
