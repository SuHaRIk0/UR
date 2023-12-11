global using Microsoft.AspNetCore.Mvc;

global using YouAre.Domain;
global using YouAre.Helpers;
global using YouAre.Persistent;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace YouAre.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext context;

        public UsersController(DataContext dataContext) => context = dataContext;

        [HttpGet("", Name = "GetAllUsers")]
        [Authorize]
        public async Task<List<User>> GetAll()
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get All Communities: Getting [navajowhite3]IEnumerable[/]<[darkcyan]User[/]> List from Database...");
            var list = await context.Profiles.OrderBy(cm => cm.Id).ToListAsync();
            if (list != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return list;
            }
            Logger.warning("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] List was [royalblue1]null[/] or does not exist...>");
            return new List<User>() {  };
        }

        [HttpGet("{id:int}", Name = "GetUserByID")]
        [Authorize]
        public async Task<User> GetBy([FromBody] int id)
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get User By ID: Getting [darkcyan]User[/]  from Database.Profiles...");
            var user = await context.Profiles.FirstOrDefaultAsync(cm => cm.Id == id);
            if (user != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return user;
            }
            Logger.warning($"[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] [darkcyan]Community[/] with ID {id} does not exist...>");
            return YouAre.Domain.User.Empty;
        }

        [HttpPost("", Name = "PostUser")]
        [Authorize]
        public async Task CreateCommunity([FromBody] User user)
        {
            if (user != null && user != YouAre.Domain.User.Empty)
            {
                context.Profiles.Add(user);
                await context.SaveChangesAsync();
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Community"));
            }
        }

        [HttpPut("", Name = "PutUser")]
        [Authorize]
        public async Task UpdateCommunity([FromBody] User New)
        {
            if (New != null && New != YouAre.Domain.User.Empty)
            {
                var Old = await context.Profiles.FirstOrDefaultAsync(com => com.Id == New.Id);
                if (Old != null)
                {
                    Old = New;
                    await context.SaveChangesAsync();
                    Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
                }
                else
                {
                    Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Community with ID {New.Id}"));
                }
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Community"));
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [Authorize]
        public async Task DeleteCommunity([FromBody] int id)
        {
            Logger.information($"[slateblue3]API-QUERY[/] Delete User: Attempt to delete [darkcyan]User[/] with ID {id}...");
            var community = await context.Profiles.FirstOrDefaultAsync(c => c.Id == id);
            if (community != null)
            {
                context.Profiles.Remove(community);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");

            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"User with ID {id}"));
            }
        }

    }
}
