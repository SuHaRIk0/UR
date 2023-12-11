using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
	[ApiController]
	[Route("api/")]
    public class PublicationsController : ControllerBase
    {
        private readonly DataContext _context;

        public PublicationsController(DataContext context) => _context = context;


        [HttpGet("Communities/{com_id:int}/[controller]/all", Name = "GetAllPubs")]
        [Authorize]
        public async Task<IEnumerable<Publication>> GetAllPubs([FromRoute] int com_id)
        {
			Logger.information($"[lightgoldenrod2]API-COMMAND[/] Get All Publications from Community with ID {com_id}: Getting [navajowhite3]IEnumerable[/]<[darkcyan]Publication[/]> List from Database...");
			var community = await _context.Communities.FirstOrDefaultAsync(cm => cm.ID == com_id);

            if (community != null)
            {
                if (community.Publications != null)
                {
                    var list = community.Publications.ToList();
                    if (list != null)
                    {
						Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
						return list;
                    }
                    Logger.error("[lightgoldenrod2]API-COMMAND EXCEPTION[/]", new WasEmptyException($"Publications List in Community with ID {com_id}"));
                }
            } else
            {
				Logger.error("[lightgoldenrod2]API-COMMAND EXCEPTION[/]:", new NotFoundException($"Community with ID {com_id}"));
			}
			return new List<Publication>() { Publication.Empty };
        }

        [HttpGet("Communities/{com_id:int}/[controller]/{id:int}", Name = "GetPubByID")]
        [Authorize]
        public async Task<Publication> GetPubByID([FromRoute] int com_id, [FromBody] int id)
        {
			Logger.information($"[lightgoldenrod2]API-COMMAND[/] Get All Publication with ID {id} from Community with ID {com_id}: Getting [darkcyan]Publication[/] from Database...");
			var community = await _context.Communities.FirstOrDefaultAsync(cm => cm.ID == com_id);

            if (community != null)
            {
                if (community.Publications != null)
                {
                    var list = community.Publications.ToList();
                    if (list != null)
                    {
                        var pub = list.FirstOrDefault(p => p.ID == id);
                        if (pub != null)
                        {
							Logger.information("[lightgoldenrod2]API-COMMAND RESULT[/]: [darkcyan]Success[/].");
							return pub;
                        }
						Logger.error("[lightgoldenrod2]API-COMMAND EXCEPTION[/]:", new NotFoundException($"Community with ID {com_id}"));
					} else
                    {
						Logger.error("[lightgoldenrod2]API-COMMAND EXCEPTION[/]", new WasEmptyException($"Publications List in Community with ID {com_id}"));
					}
				}
            } else
            {
				Logger.error("[lightgoldenrod2]API-COMMAND EXCEPTION[/]:", new NotFoundException($"Community with ID {com_id}"));
			}
            return Publication.Empty;
        }

        [HttpPost("Communities/{com_id:int}/[controller]", Name = "CratePub")]
        [Authorize]
        public async Task CreatePub([FromRoute] int com_id, [FromBody] Publication publication)
        {
			Logger.information($"[slateblue3]API-QUERY[/] Create new Publication inside Community with ID {com_id}...");
			var community = await _context.Communities.FirstOrDefaultAsync(cm => cm.ID == com_id);

            if (community != null)
            {
                if (community.Publications != null)
                {
                    community.Publications.Append(publication);
                }
                else
                {
                    community.Publications = new List<Publication>() { publication };
                }
				Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
			} else
            {
				Logger.error("[slateblue3]API-QUERY EXCEPTION[/]:", new NotFoundException($"Community with ID {com_id}"));
			}
        }

        [HttpPut("Communities/{com_id:int}/[controller]", Name = "UpdatePub")]
        [Authorize]
        public async Task UpdatePub([FromRoute] int com_id, [FromBody] Publication publication)
        {
			Logger.information($"[slateblue3]API-QUERY[/] Update Publication with ID {publication.ID} inside Community with ID {com_id}...");
			var community = await _context.Communities.FirstOrDefaultAsync(cm => cm.ID == com_id);

            if (community != null)
            {
                if (community.Publications != null)
                {
                    var pub = community.Publications.FirstOrDefault(p => p.ID == publication.ID);
                    if (pub != null)
                    {
                        pub = publication;
                        await _context.SaveChangesAsync();
						Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
					}
					Logger.error("[slateblue3]API-QUERY EXCEPTION[/]:", new NotFoundException($"Publication with ID {publication.ID}"));
				} else
                {
					Logger.error("[slateblue3]API-QUERY EXCEPTION[/]:", new WasEmptyException("Community.Publications"));
				}
            } else
            {
				Logger.error("[slateblue3]API-QUERY EXCEPTION[/]:", new NotFoundException($"Community with ID {com_id}"));
			}
        }

        [HttpDelete("api/Communities/{com_id:int}/[controller]/{id:int}", Name = "DeletePub")]
        [Authorize]
        public async Task DeletePub([FromRoute] int com_id, [FromBody] int id)
        {
            var community = await _context.Communities.FirstOrDefaultAsync(cm => cm.ID == com_id);

            if (community != null)
            {
                if (community.Publications != null)
                {
                    var publication = community.Publications.FirstOrDefault(p => p.ID == id);
                    if (publication != null)
                    {
                        community.Publications.ToList().Remove(publication);
                        await _context.SaveChangesAsync();
						Logger.information($"[slateblue3]API-QUERY RESULT[/]: [darkcyan]Success[/].");
					}
                    Logger.error("[slateblue3]API-QUERY EXCEPTION[/]:", new NotFoundException($"Publication with ID {id}"));
				} else
                {
					Logger.error("[slateblue3]API-QUERY EXCEPTION[/]:", new WasEmptyException("Community.Publications"));
				}
            } else
            {
				Logger.error("[slateblue3]API-QUERY EXCEPTION[/]:", new NotFoundException($"Community with ID {com_id}"));
			}
        }
    }
}
