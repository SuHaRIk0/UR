using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
	[ApiController]
    [Route("api/")]
    public class CommunitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public CommunitiesController(DataContext context) => _context = context;

        [HttpGet("[controller]/all", Name = "GetAllCommunities")]
        [Authorize]
        public async Task<IEnumerable<Community>> GetAll()
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get All Communities: Getting [navajowhite3]IEnumerable[/]<[darkcyan]Community[/]> List from Database...");
            var list = await _context.Communities.OrderBy(cm => cm.ID).ToListAsync();
            if (list != null)
            {
				Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
				return list;
            }
			Logger.warning("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] List was [royalblue1]null[/] or does not exist...>");
			return new List<Community>() { Community.Empty };
        }

        [HttpGet("[controller]/{id:int}", Name = "GetCommunityByID")]
        [Authorize]
        public async Task<Community> GetBy([FromBody] int id)
        {
			Logger.information("[lightgoldenrod2]API-COMMAND[/] Get Community By ID: Getting [darkcyan]Community[/]  from Database.Communities...");
			var community = await _context.Communities.FirstOrDefaultAsync(cm => cm.ID == id);
            if (community != null)
            {
				Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
				return community;
            }
			Logger.warning($"[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] [darkcyan]Community[/] with ID {id} does not exist...>");
			return Community.Empty;
        }

        [HttpPost("[controller]", Name = "PostCommunity")]
        [Authorize]
        public async Task CreateCommunity([FromBody] Community community)
        {
			if (community != null && community != Community.Empty)
            {
				Logger.information($"[slateblue3]API-QUERY[/] Create Community: User {community.AuthorID} creats new [darkcyan]Community[/] with ID {community.ID}...");
				_context.Communities.Add(community);
                await _context.SaveChangesAsync();
				Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
			}
            else
            {
				Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Community"));
			}
        }

        [HttpPut("[controller]", Name = "PutCommunity")]
        [Authorize]
        public async Task UpdateCommunity([FromBody] Community New)
        {
            if (New != null && New != Community.Empty)
			{
				Logger.information($"[slateblue3]API-QUERY[/] Update Community: User {New.AuthorID} tries to update [darkcyan]Community[/] with ID {New.ID}...");
				var Old = await _context.Communities.FirstOrDefaultAsync(com => com.ID == New.ID);
				if (Old != null)
                {
                    Old = New;
                    await _context.SaveChangesAsync();
					Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
				} else
                {
					Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Community with ID {New.ID}"));
				}
            }
            else
            {
				Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Community"));
			}
		}

        [HttpDelete("[controller]/{id:int}", Name = "DeleteCommunity")]
        [Authorize]
        public async Task DeleteCommunity([FromBody] int id)
        {
			Logger.information($"[slateblue3]API-QUERY[/] Delete Community: Attempt to delete [darkcyan]Community[/] with ID {id}...");
			var community = await _context.Communities.FirstOrDefaultAsync(c => c.ID == id);
            if (community != null)
            {
                _context.Communities.Remove(community);
                await _context.SaveChangesAsync();
				Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");

			} else
            {
				Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Community with ID {id}"));
			}
        }
    }
}
