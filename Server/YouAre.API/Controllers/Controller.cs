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

    [HttpPost("send_message")]
    public IActionResult SendMessage([FromBody] Message model)
    {
        // Клієнт відправляє свій айді, айді того, з ким відкритий чат, повідомлення і час відправлення.
        // Сервер записує це в базу даних.

        var newMessage = new Message
        {
            AuthorId = model.AuthorId,
            RecipientId = model.RecipientId,
            Text = model.Text,
            SentAt = DateTime.Now
        };

        _context.Messages.Add(newMessage);
        _context.SaveChanges();

        return Ok("Повідомлення відправлено успішно");
    }

    [HttpGet("all_users")]
    public IActionResult GetAllUsers()
    {
        // Retrieve the list of all users with their names and surnames
        var users = _context.Profiles.Select(u => new { u.Id, u.Username }).ToList();

        return Ok(users);
    }

    [HttpGet("posts")]
    public IActionResult GetAllPublications()
    {
        // Пости - при запиті з впф сервер виводить всі наявні публікації в порядку від найновіших до старіших.

        var posts = _context.Publications.OrderByDescending(p => p.PostAt).ToList();

        return Ok(posts);
    }

    [HttpPost("create_post")]
    public IActionResult CreatePost([FromBody] Publication model)
    {
        // Створення поста - при запиті відправляємо посилання на зображення, текст та наш айді.
        // Сервер створює такий рядок в бд.

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
}
