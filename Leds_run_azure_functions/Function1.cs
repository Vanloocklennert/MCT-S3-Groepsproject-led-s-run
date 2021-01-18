using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Leds_run_azure_functions.Models;
using System.Data.SqlClient;

namespace Leds_run_azure_functions
{
    public static class Function1
    {
        [FunctionName("AddUser")]
        public static async Task<IActionResult> AddUserV1(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/user")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                User newuser = JsonConvert.DeserializeObject<User>(requestBody);

                string connectionString = Environment.GetEnvironmentVariable("SQLServer");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO tblusers (username, e-mail, passwordhash) VALUES(@username, @email, @passwordhash)";
                        command.Parameters.AddWithValue("@username", newuser.Username);
                        command.Parameters.AddWithValue("@email", newuser.EMail);
                        command.Parameters.AddWithValue("@passwordhash", newuser.PasswordHash);

                        await command.ExecuteNonQueryAsync();

                    }
                }


                return new OkObjectResult(newuser);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }
    }
}
