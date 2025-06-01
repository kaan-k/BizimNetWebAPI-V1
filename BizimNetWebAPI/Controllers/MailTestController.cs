using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Entities.Concrete.Email;

namespace BizimNetWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailTestController : ControllerBase
    {
        private readonly IMailManager _mailManager;

        public MailTestController(IMailManager mailManager)
        {
            _mailManager = mailManager;
        }

        [HttpGet("Send")]
        public IActionResult SendTestMail()
        {
            var config = new EmailConfiguration
            {
                SmtpServer = "smtp.gmail.com",
                Port = 587,
                From = "kaannkale@gmail.com",
                Username = "kaannkale@gmail.com",
                Password = "pkho hrxk adwx oxkf ",
                To = new List<string> { "MauriceS@blueplanet-tv.de" }
            };

            var content = new EMailContent
            {
                Subject = "Test Maili",
                Body = "<h1>Hello bro</h1><p>Testing da code rn fr fr</p>",
                IsBodyHtml = true
            };

            _mailManager.SendMail(config, content);

            return Ok("Mail gönderildi.");
        }
    }
}
