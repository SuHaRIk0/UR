using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace YouAre.API.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly DataContext context;

        public ChatsController(DataContext dataContext) => context = dataContext;

        [HttpGet("", Name = "GetAllChats")]
        [Authorize]
        public async Task<IEnumerable<Chat>> GetAll()
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get All Chats: Getting [navajowhite3]IEnumerable[/]<[darkcyan]Chat[/]> List from Database...");
            var list = await context.Chats.OrderBy(cm => cm.Id).ToListAsync();
            if (list != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return list;
            }
            Logger.warning("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] List was [royalblue1]null[/] or does not exist...>");
            return new List<Chat>() { Chat.Empty };
        }

        [HttpGet("{id:int}", Name = "GetChatByID")]
        [Authorize]
        public async Task<Chat> GetBy([FromBody] int id)
        {
            Logger.information("[lightgoldenrod2]API-COMMAND[/] Get Chat By ID: Getting [darkcyan]Chat[/]  from Database.Chats...");
            var Chat = await context.Chats.FirstOrDefaultAsync(cm => cm.Id == id);
            if (Chat != null)
            {
                Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
                return Chat;
            }
            Logger.warning($"[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Execution failed.[/] [darkcyan]Chat[/] with ID {id} does not exist...>");
            return Chat.Empty;
        }

        [HttpPost("", Name = "PostChat")]
        [Authorize]
        public async Task CreateChat([FromBody] Chat Chat)
        {
            if (Chat != null && Chat != Chat.Empty)
            {
                Logger.information($"[slateblue3]API-QUERY[/] Create Chat: User {Chat.AuthorId} creats new [darkcyan]Chat[/] with ID {Chat.Id}...");
                context.Chats.Add(Chat);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Chat"));
            }
        }

        [HttpPut("", Name = "PutChat")]
        [Authorize]
        public async Task UpdateChat([FromBody] Chat New)
        {
            if (New != null && New != Chat.Empty)
            {
                Logger.information($"[slateblue3]API-QUERY[/] Update Chat: User {New.AuthorId} tries to update [darkcyan]Chat[/] with ID {New.Id}...");
                var Old = await context.Chats.FirstOrDefaultAsync(com => com.Id == New.Id);
                if (Old != null)
                {
                    Old = New;
                    await context.SaveChangesAsync();
                    Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
                }
                else
                {
                    Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Chat with ID {New.Id}"));
                }
            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new WasEmptyException("Received Chat"));
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteChat")]
        [Authorize]
        public async Task DeleteChat([FromBody] int id)
        {
            Logger.information($"[slateblue3]API-QUERY[/] Delete Chat: Attempt to delete [darkcyan]Chat[/] with ID {id}...");
            var Chat = await context.Chats.FirstOrDefaultAsync(c => c.Id == id);
            if (Chat != null)
            {
                context.Chats.Remove(Chat);
                await context.SaveChangesAsync();
                Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");

            }
            else
            {
                Logger.error("[slateblue3]API-QUERY ERROR[/]:", new NotFoundException($"Chat with ID {id}"));
            }
        }
    }
}
