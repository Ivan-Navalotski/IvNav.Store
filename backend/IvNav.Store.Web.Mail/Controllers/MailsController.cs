using System.Diagnostics;
using IvNav.Store.Setup.Controllers.Base;
using IvNav.Store.Web.Mail.Models;
using Microsoft.AspNetCore.Mvc;

namespace IvNav.Store.Web.Mail.Controllers
{
    public class MailsController : ApiControllerBase
    {
        private static readonly string[] Summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MailsController> _logger;

        public MailsController(ILogger<MailsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<MailResposneDto> Get()
        {
            Console.WriteLine(Activity.Current?.Id);

            return Enumerable.Range(1, 5).Select(index => new MailResposneDto
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
