using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using retonet.Services;
using retonet.Models;

public class LoginFunction
{
    private readonly IUserService _userService;

    public LoginFunction(IUserService userService)
    {
        _userService = userService;
    }

    [Function("Login")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req,
        FunctionContext executionContext)
    {
        ILogger log = executionContext.GetLogger("LoginFunction");
        log.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<LoginRequest>(requestBody);

        if (data == null || string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Password))
        {
            log.LogWarning("Request body is null or missing required fields.");
            return req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
        }

        try
        {
            var user = await _userService.GetUserByEmailAsync(data.Email);
            if (user == null || string.IsNullOrEmpty(user.PasswordHash) || string.IsNullOrEmpty(user.Salt) || !_userService.VerifyPassword(data.Password, user.PasswordHash, user.Salt))
            {
                return req.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }

            var token = _userService.GenerateJwtToken(user);
            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { token });

            return response;
        }
        catch (Exception ex)
        {
            log.LogError($"An error occurred: {ex.Message}");
            return req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}