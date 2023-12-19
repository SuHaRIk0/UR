using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using YouAre.Domain;
using YouAre.Persistent; 
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext _context;

    public UserController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User model)
    {
        var user = _context.Profiles.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);

        if (user == null)
        {
            return BadRequest("Неправильне ім'я користувача або пароль");
        }
        var token = GenerateToken(user);

        return Ok(new { Token = token, UserId = user.Id });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User model)
    {
        var existingUser = _context.Profiles.SingleOrDefault(u => u.Username == model.Username);

        if (existingUser != null)
        {
            return BadRequest("Користувач з таким ім'ям вже існує");
        }

        var newUser = new User
        {
            Username = model.Username,
            Email = model.Email,
            Password = model.Password,
        };

        _context.Profiles.Add(newUser);
        _context.SaveChanges();

        var token = GenerateToken(newUser);

        return Ok(new { Token = token, UserId = newUser.Id });
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("zelenko_youare_1234567890123456_1234567890123456");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            }),
            Expires = DateTime.UtcNow.AddDays(7), 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [Authorize]
    [HttpPost("send_message")]
    public IActionResult SendMessage([FromBody] Message model)
    {
        try
        {
            // Клієнт відправляє свій айді, айді того, з ким відкритий чат, повідомлення і час відправлення.
            // Сервер записує це в базу даних.

            var newMessage = new Message
            {
                AuthorId = model.AuthorId,
                RecipientId = model.RecipientId,
                Text = model.Text,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(newMessage);
            _context.SaveChanges();

            return Ok("Повідомлення відправлено успішно");
        }
        catch (Exception ex)
        {
            // Handle exceptions as needed
            return BadRequest("Помилка при відправленні повідомлення");
        }
    }

    [Authorize]
    [HttpGet("chats")]
    public IActionResult GetChat([FromQuery] int user1Id, [FromQuery] int user2Id)
    {
        try
        {
            var chat = _context.Messages
                .Where(m => (m.AuthorId == user1Id && m.RecipientId == user2Id) ||
                            (m.AuthorId == user2Id && m.RecipientId == user1Id))
                .ToList();

            return Ok(chat);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }


    [Authorize]
    [HttpGet("all_users")]
    public IActionResult GetAllUsers()
    {
        // Retrieve the list of all users with their names and surnames
        var users = _context.Profiles.Select(u => new { u.Id, u.Username }).ToList();

        return Ok(users);
    }

    [Authorize]
    [HttpGet("posts")]
    public IActionResult GetAllPublications()
    {
        var posts = _context.Publications
            .OrderByDescending(p => p.PostAt)
            .ToList();

        var publications = posts.Select(q => new Publication
        {
            Id = q.Id,
            AuthorId = q.AuthorId,
            PostAt = q.PostAt,
            Text = q.Text,
            Picture = q.Picture,
        }).ToList();

        return Ok(publications);
    }



    [Authorize]
    [HttpPost("create_post")]
    public IActionResult CreatePost([FromBody] Publication model)
    {
        if (model == null)
        {
            return BadRequest("Invalid request body");
        }

        var newPublication = new Publication
        {
            Picture = model.Picture,
            Text = model.Text,
            AuthorId = model.AuthorId,
            PostAt = DateTime.UtcNow
        };

        _context.Publications.Add(newPublication);
        _context.SaveChanges();

        return Ok("Пост створено успішно");
    }

    [Authorize]
    [HttpPost("update_avatar")]
    public IActionResult UpdateAvatar([FromBody] User model)
    {
        var user = _context.Profiles.FirstOrDefault(u => u.Id == model.Id);

        if (user == null)
        {
            return NotFound("Користувач не знайдений");
        }

        user.ProfilePhoto = model.ProfilePhoto;

        _context.SaveChanges();

        return Ok("Аватар змінено успішно");
    }

    [Authorize]
    [HttpPost("update_description")]
    public IActionResult UpdateDescription([FromBody] User model)
    {
        var user = _context.Profiles.FirstOrDefault(u => u.Id == model.Id);

        if (user == null)
        {
            return NotFound("Користувач не знайдений");
        }

        user.Description = model.Description;

        _context.SaveChanges();

        return Ok("Опис змінено успішно");
    }

    [Authorize]
    [HttpGet("my_statistics")]
    public IActionResult GetMyStatistics(int userId)
    {
        // Статистика моя - при запиті відправляємо айді, сервер повертає кількість часу проведеного користувачем в застосунку.
        // (Можливо, що вам доведеться додати логіку для вибору часу за останні 7 днів)

        var user = _context.Profiles.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("Користувач не знайдений");
        }

        return Ok(user.TimeSpent);
    }

    [Authorize]
    [HttpGet("daily_statistics")]
    public IActionResult GetDailyStatistics()
    {
        // Статистика за день - показує трьох учасників, які найбільшу кількість проведеного часу за день мають,
        // і поточну позицію юзера який зараз залогований.
        // (Можливо, що вам доведеться додати логіку для вибору часу за сьогодні)

        var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


        var usersByTime = _context.Profiles.OrderByDescending(u => u.TimeSpent).Take(3).Select(u => new { u.Id, u.TimeSpent }).ToList();
        var currentUser = _context.Profiles.FirstOrDefault(u => u.Id.ToString() == loggedInUserId);

        if (currentUser == null)
        {
            return NotFound("Користувач не знайдений");
        }

        var currentUserPosition = usersByTime.FindIndex(u => u.Id == currentUser.Id) + 1;

        return Ok(new { TopUsers = usersByTime, CurrentUserPosition = currentUserPosition });
    }

    [Authorize]
    [HttpGet("weekly_statistics")]
    public IActionResult GetWeeklyStatistics()
    {
        // Статистика за тиждень - показує трьох учасників, які найбільшу кількість проведеного часу за тиждень мають,
        // і поточну позицію юзера який зараз залогований.
        // (Можливо, що вам доведеться додати логіку для вибору часу за останні 7 днів)

        var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


        var usersByTime = _context.Profiles.OrderByDescending(u => u.TimeSpent).Take(3).Select(u => new { u.Id, u.TimeSpent }).ToList();
        var currentUser = _context.Profiles.FirstOrDefault(u => u.Id.ToString() == loggedInUserId);


        if (currentUser == null)
        {
            return NotFound("Користувач не знайдений");
        }

        var currentUserPosition = usersByTime.FindIndex(u => u.Id == currentUser.Id) + 1;

        return Ok(new { TopUsers = usersByTime, CurrentUserPosition = currentUserPosition });
    }

    [Authorize]
    [HttpGet("profile")]
    [HttpPost("profile")]
    public IActionResult GetUserProfile(int userId)
    {
        var userProfile = _context.Profiles
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                u.Username,
                u.ProfilePhoto,
                u.Description,
                u.Email
            })
            .FirstOrDefault();

        if (userProfile == null)
        {
            return NotFound("Користувач не знайдений");
        }

        return Ok(userProfile);
    }


}
