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
using System.Collections.Generic;

namespace Leds_run_azure_functions
{
    public static class Function1
    {
        // Function to Add a new user to the DB
        [FunctionName("AddUserV1")]
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


        // Function to GET all default Workouts out of the DB
        [FunctionName("GetDefaultWorkoutsV1")]
        public static async Task<IActionResult> GetDefaultWorkoutsV1(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/workouts")] HttpRequest req,
            ILogger log)
        {

            try
            {
                List<Workout> workouts = new List<Workout>();

                string connectionString = Environment.GetEnvironmentVariable("SQLServer");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        // Make query to select all default workouts (tbldefaultworkouts) with an Inner join to tblworkouts
                        command.CommandText = "SELECT tbldefaultworkouts.default_id, tblworkouts.type, tblworkouts.name, tblworkouts.distance, tblworkouts.speed, tblworkouts.time FROM tbldefaultworkouts INNER JOIN tblworkouts ON tbldefaultworkouts.workout_id_fk = tblworkouts.workout_id; ";

                        SqlDataReader reader = await command.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            Workout workout = new Workout();
                            workout.Default_Id = int.Parse(reader["default_id"].ToString());
                            workout.Name = reader["name"].ToString();
                            workout.Type = reader["type"].ToString();
                            workout.Distance = double.Parse(reader["distance"].ToString());
                            workout.Speed = double.Parse(reader["speed"].ToString());
                            workout.Time = TimeSpan.Parse(reader["time"].ToString());
                            workouts.Add(workout);
                        }

                    }
                }


                return new OkObjectResult(workouts);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }

        }


        // Function to GET all default Workouts out of the DB
        [FunctionName("GetChallengesV1")]
        public static async Task<IActionResult> GetChallengesV1(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/challenges")] HttpRequest req,
            ILogger log)
        {

            try
            {
                List<Workout> workouts = new List<Workout>();

                string connectionString = Environment.GetEnvironmentVariable("SQLServer");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        // Make query to select all CHALLENGES (tbldefaultworkouts) with an Inner join to tblworkouts (search for type 'runagainst')
                        command.CommandText = "SELECT tbldefaultworkouts.default_id, tblworkouts.type, tblworkouts.name, tblworkouts.distance, tblworkouts.speed, tblworkouts.time FROM tbldefaultworkouts INNER JOIN tblworkouts ON tbldefaultworkouts.workout_id_fk = tblworkouts.workout_id WHERE type='runagainst'";

                        SqlDataReader reader = await command.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            Workout workout = new Workout();
                            workout.Default_Id = int.Parse(reader["default_id"].ToString());
                            workout.Name = reader["name"].ToString();
                            workout.Type = reader["type"].ToString();
                            workout.Distance = double.Parse(reader["distance"].ToString());
                            workout.Speed = double.Parse(reader["speed"].ToString());
                            workout.Time = TimeSpan.Parse(reader["time"].ToString());
                            workouts.Add(workout);
                        }

                    }
                }


                return new OkObjectResult(workouts);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }

        }
    }
}
