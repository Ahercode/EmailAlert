
using AlertEmailAPI.Dto;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace AlertEmailAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailAlertController : ControllerBase
{
    private readonly IConfiguration _config;

    public EmailAlertController(IConfiguration config)
    {
        _config = config;
    }


    [HttpPost]
    public IActionResult SendMail([FromBody] EmailRequest request)
    {
        
        var from = _config["EmailConfiguration:From"];
        var password = _config["EmailConfiguration:Password"];
        var username = _config["EmailConfiguration:Username"];
        var port = _config["EmailConfiguration:Port"];
        var smtpServer = _config["EmailConfiguration:SmtpServer"];
        
        
        try
        {
            foreach (var item in request.Recipients)
            {
                // Create the email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add( MailboxAddress.Parse(item.Email));
                email.Subject = request.Subject;

                email.Body = new TextPart(TextFormat.Plain)
                {
                    Text = request.Message
                };

                // Configure the SMTP client
                using var smtp = new SmtpClient();
                smtp.Connect(smtpServer, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(username, password);

                // Send the email
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        
    }
}