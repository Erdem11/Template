using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace Template.Api.Controllers.V1._0
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class SignalrController : TemplateControllerBase
    {
        // GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var received = string.Empty;

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/MessagingHub")
                .Build();
            
            await connection.StartAsync();

            connection.On<string>("ReceiveMessageAsync", message => 
                received = message
            );
            
            await connection.SendAsync("EchoAsync", "aaa");
            var rr =  await connection.InvokeCoreAsync<string>("EchoAsync", new object[]{"aaa"});

            await connection.SendAsync("EchoAsync", "aaa");
            await Task.Delay(5000);
            await connection.StopAsync();
            return await Task.FromResult(Ok(received));
        }
    }
}