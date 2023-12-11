using Microsoft.AspNetCore.Identity;

namespace YouAre.Domain
{
	public class Account : IdentityUser
	{
		[Required]	public int? UserID { get; set; }
	}
}
