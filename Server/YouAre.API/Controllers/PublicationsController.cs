using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YouAre.API.Controllers
{
    [Route("api/publications")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly DataContext context;

        public PublicationsController(DataContext dataContext) => context = dataContext;

        [HttpGet("", Name = "GetAllPublications")]
        [Authorize]
        public async Task<IEnumerable<Publication>> GetAll()
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get All Publications: Getting [navajowhite3]IEnumerable[/]<[darkcyan]Publication[/]> List from Database...");
            var list = await context.Publications.OrderBy(cm => cm.Id).ToListAsync();
            if (list != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return list;
            }
            Logger.warning("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] List was [royalblue1]null[/] or does not exist...>");
            return new List<Publication>() { Publication.Empty };
        }

        [HttpGet("{id:int}", Name = "GetPublicationByID")]
        [Authorize]
        public async Task<Publication> GetBy([FromBody] int id)
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get Publication By ID: Getting [darkcyan]Publication[/]  from Database.Publications...");
            var Publication = await context.Publications.FirstOrDefaultAsync(cm => cm.Id == id);
            if (Publication != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return Publication;
            }
            Logger.warning($"[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] [darkcyan]Publication[/] with ID {id} does not exist...>");
            return Publication.Empty;
        }

        [HttpPost("", Name = "PostPublication")]
        [Authorize]
        public async Task CreatePublication([FromBody] Publication Publication)
        {
            if (Publication != null && Publication != Publication.Empty)
            {
                Logger.information($"[slateblue3]API-QUERY[/] Create Publication: User {Publication.AuthorId} creats new [darkcyan]Publication[/] with ID {Publication.Id}...");
                context.Publications.Add(Publication);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Publication"));
            }
        }

        [HttpPut("", Name = "PutPublication")]
        [Authorize]
        public async Task UpdatePublication([FromBody] Publication New)
        {
            if (New != null && New != Publication.Empty)
            {
                Logger.information($"[slateblue3]API-QUERY[/] Update Publication: User {New.AuthorId} tries to update [darkcyan]Publication[/] with ID {New.Id}...");
                var Old = await context.Publications.FirstOrDefaultAsync(com => com.Id == New.Id);
                if (Old != null)
                {
                    Old = New;
                    await context.SaveChangesAsync();
                    Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
                }
                else
                {
                    Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Publication with ID {New.Id}"));
                }
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Publication"));
            }
        }

        [HttpDelete("{id:int}", Name = "DeletePublication")]
        [Authorize]
        public async Task DeletePublication([FromBody] int id)
        {
            Logger.information($"[slateblue3]API-QUERY[/] Delete Publication: Attempt to delete [darkcyan]Publication[/] with ID {id}...");
            var Publication = await context.Publications.FirstOrDefaultAsync(c => c.Id == id);
            if (Publication != null)
            {
                context.Publications.Remove(Publication);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");

            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Publication with ID {id}"));
            }
        }
    }
}
