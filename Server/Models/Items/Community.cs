using Spectre.Console;

namespace Server.Models.Items
{
	public class Community
	{
		[Key]		public int ID { get; set; }
		[Required]	public int AuthorID { get; set; }
		[Required]	public IEnumerable<Publication>? Publications { get; set; }
		[Required]	public IEnumerable<User>? Members { get; set; }

		public static Community Empty = new Community() { ID = -1 };

		public Publication FindPublication(int id)
		{
			if (this.Publications != null)
			{
				var result = this.Publications.FirstOrDefault(x => x.ID == id);
				if (result != null)
				{
					Logger.information($"Publication with id ({id}) was found in Community {this.ID}!");
					return result;
				}
			}
			Logger.error($"Community {this.ID} rise an exception:", new NotFoundException($"Publication with id: {id} in Community with id: {this.ID}"));
			return Publication.Empty;
		}

		public User FindUser(int id)
		{
			if(this.Members != null)
			{
				var result = this.Members.FirstOrDefault(x => x.ID == id);
				if(result != null)
				{
					Logger.information($"User with id ({id}) was found in Community {this.ID}!");
					return result;
				}
			}
			Logger.error($"Community {this.ID} rise an exception:", new NotFoundException($"User with id: {id} in Community with id: {this.ID}"));
			return User.Empty;
		}
	}

	[JsonSerializable(typeof(Community[]))]
	public partial class ApiJsonSerializerCommunity : JsonSerializerContext
	{

	}
}
