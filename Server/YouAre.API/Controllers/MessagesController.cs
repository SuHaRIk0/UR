using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace YouAre.API.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly DataContext context;

        public MessagesController(DataContext dataContext) => context = dataContext;

        [HttpGet("", Name = "GetAllMessages")]
        [Authorize]
        public async Task<IEnumerable<Message>> GetAll()
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get All Messages: Getting [navajowhite3]IEnumerable[/]<[darkcyan]Message[/]> List from Database...");
            var list = await context.Messages.OrderBy(cm => cm.Id).ToListAsync();
            if (list != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return list;
            }
            Logger.warning("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] List was [royalblue1]null[/] or does not exist...>");
            return new List<Message>() { Message.Empty };
        }

        [HttpGet("{id:int}", Name = "GetMessageByID")]
        [Authorize]
        public async Task<Message> GetBy([FromBody] int id)
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get Message By ID: Getting [darkcyan]Message[/]  from Database.Messages...");
            var Message = await context.Messages.FirstOrDefaultAsync(cm => cm.Id == id);
            if (Message != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return Message;
            }
            Logger.warning($"[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] [darkcyan]Message[/] with ID {id} does not exist...>");
            return Message.Empty;
        }

        [HttpPost("", Name = "PostMessage")]
        [Authorize]
        public async Task CreateMessage([FromBody] Message Message)
        {
            if (Message != null && Message != Message.Empty)
            {
                Logger.information($"[slateblue3]API-QUERY[/] Create Message: User {Message.AuthorId} creats new [darkcyan]Message[/] with ID {Message.Id}...");
                context.Messages.Add(Message);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Message"));
            }
        }

        [HttpPut("", Name = "PutMessage")]
        [Authorize]
        public async Task UpdateMessage([FromBody] Message New)
        {
            if (New != null && New != Message.Empty)
            {
                Logger.information($"[slateblue3]API-QUERY[/] Update Message: User {New.AuthorId} tries to update [darkcyan]Message[/] with ID {New.Id}...");
                var Old = await context.Messages.FirstOrDefaultAsync(com => com.Id == New.Id);
                if (Old != null)
                {
                    Old = New;
                    await context.SaveChangesAsync();
                    Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
                }
                else
                {
                    Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Message with ID {New.Id}"));
                }
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Message"));
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteMessage")]
        [Authorize]
        public async Task DeleteMessage([FromBody] int id)
        {
            Logger.information($"[slateblue3]API-QUERY[/] Delete Message: Attempt to delete [darkcyan]Message[/] with ID {id}...");
            var Message = await context.Messages.FirstOrDefaultAsync(c => c.Id == id);
            if (Message != null)
            {
                context.Messages.Remove(Message);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");

            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Message with ID {id}"));
            }
        }
    }
}
