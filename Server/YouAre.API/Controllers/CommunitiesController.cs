using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace YouAre.API.Controllers
{
    [Route("api/communities")]
    [ApiController]
    public class CommunitiesController : ControllerBase
    {
        private readonly DataContext context;

        public CommunitiesController(DataContext dataContext) => context = dataContext;

        [HttpGet("", Name = "GetAllCommunities")]
        [Authorize]
        public async Task<IEnumerable<Community>> GetAll()
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get All Communities: Getting [navajowhite3]IEnumerable[/]<[darkcyan]Community[/]> List from Database...");
            var list = await context.Communities.OrderBy(cm => cm.Id).ToListAsync();
            if (list != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return list;
            }
            Logger.warning("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] List was [royalblue1]null[/] or does not exist...>");
            return new List<Community>() { Community.Empty };
        }

        [HttpGet("{id:int}", Name = "GetCommunityByID")]
        [Authorize]
        public async Task<Community> GetBy([FromBody] int id)
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get Community By ID: Getting [darkcyan]Community[/]  from Database.Communities...");
            var community = await context.Communities.FirstOrDefaultAsync(cm => cm.Id == id);
            if (community != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return community;
            }
            Logger.warning($"[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] [darkcyan]Community[/] with ID {id} does not exist...>");
            return Community.Empty;
        }

        [HttpPost("", Name = "PostCommunity")]
        [Authorize]
        public async Task CreateCommunity([FromBody] Community community)
        {
            if (community != null && community != Community.Empty)
            {
                Logger.information($"[slateblue3]API-QUERY[/] Create Community: User {community.AuthorId} creats new [darkcyan]Community[/] with ID {community.Id}...");
                context.Communities.Add(community);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Community"));
            }
        }

        [HttpPut("", Name = "PutCommunity")]
        [Authorize]
        public async Task UpdateCommunity([FromBody] Community New)
        {
            if (New != null && New != Community.Empty)
            {
                Logger.information($"[slateblue3]API-QUERY[/] Update Community: User {New.AuthorId} tries to update [darkcyan]Community[/] with ID {New.Id}...");
                var Old = await context.Communities.FirstOrDefaultAsync(com => com.Id == New.Id);
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

        [HttpDelete("{id:int}", Name = "DeleteCommunity")]
        [Authorize]
        public async Task DeleteCommunity([FromBody] int id)
        {
            Logger.information($"[slateblue3]API-QUERY[/] Delete Community: Attempt to delete [darkcyan]Community[/] with ID {id}...");
            var community = await context.Communities.FirstOrDefaultAsync(c => c.Id == id);
            if (community != null)
            {
                context.Communities.Remove(community);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");

            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Community with ID {id}"));
            }
        }
    }
}
