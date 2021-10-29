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
        [HttpGet]
        public async Task<IActionResult> Index(string message)
        {
            var received = string.Empty;

            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/MessagingHub", options => {
                    options.AccessTokenProvider = AccessTokenTask;
                })
                .Build();

            await connection.StartAsync();

            connection.On<string>("ReceiveMessageAsync", response =>
                received = response
            );

            await connection.SendAsync("EchoAsync", message);
            var invokeResult = await connection.InvokeCoreAsync<string>("EchoAsync", new object[]
            {
                message
            });

            await connection.StopAsync();
            return await Task.FromResult(Ok(received));
        }
    }
}