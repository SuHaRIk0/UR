using Microsoft.AspNetCore.Identity;

using Server.Models.Items;

namespace Server.Models
{
	public class Account : IdentityUser
	{
		[Required]	public User? User { get; set; }
	}
}
