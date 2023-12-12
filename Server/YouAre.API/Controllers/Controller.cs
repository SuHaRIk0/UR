using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using YouAre.Domain;
using YouAre.Persistent; // Add this line
using System;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class Controller : ControllerBase
{
    private readonly DataContext _context; // Assuming DataContext is your DatabaseContext

    public Controller(DataContext context)
    {
        _context = context;
    }


    [HttpPost("SendMessage")]
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

    [HttpGet("Posts")]
    public IActionResult GetAllPublications()
    {
        // Пости - при запиті з впф сервер виводить всі наявні публікації в порядку від найновіших до старіших.

        var posts = _context.Publications.OrderByDescending(p => p.PostAt).ToList();

        return Ok(posts);
    }

    [HttpPost("CreatePost")]
    public IActionResult CreatePost([FromBody] Publication model)
    {
        // Створення поста - при запиті відправляємо посилання на зображення, текст та наш айді.
        // Сервер створює такий рядок в бд.

        var newPublication = new Publication
        {
            Picture = model.Picture,
            Text = model.Text,
            AuthorId = model.AuthorId,
            PostAt = DateTime.Now
            // Додайте інші поля за потребою
        };

        _context.Publications.Add(newPublication);
        _context.SaveChanges();

        return Ok("Пост створено успішно");
    }

    [HttpGet("MyStatistics")]
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


    [HttpGet("DailyStatistics")]
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

    [HttpGet("WeeklyStatistics")]
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
