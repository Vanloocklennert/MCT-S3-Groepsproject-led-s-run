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
                        command.CommandText = "SELECT tbldefaultworkouts.default_id, tblworkouts.type, tblworkouts.name, tblworkouts.distance, tblworkouts.speed, tblworkouts.time FROM tbldefaultworkouts INNER JOIN tblworkouts ON tbldefaultworkouts.workout_id_fk = tblworkouts.workout_id WHERE NOT type='runagainst'";

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


        // Function to GET all default Workouts out of the DB
        [FunctionName("GetUserWorkoutsV1")]
        public static async Task<IActionResult> GetUserWorkoutsV1(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/{username}/workouts")] HttpRequest req,
            string username,
            ILogger log)
        {

            try
            {
                List<Workout> workouts = new List<Workout>(); // List of all workouts
                List<List<Workout>> groupedWorkouts = new List<List<Workout>>(); // List of workouts with there INTERVALS in it as a list.

                string connectionString = Environment.GetEnvironmentVariable("SQLServer");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        

                        command.CommandText = "SELECT user_id, username, e-mail FROM tblusers WHERE username=@username";
                        command.Parameters.AddWithValue("@username", username);

                        SqlDataReader reader = await command.ExecuteReaderAsync();

                        int userId = 0;
                        while (await reader.ReadAsync())
                        {
                            userId = int.Parse(reader["user_id"].ToString());
                        }

                        if (userId != 0)
                        {
                            // Make query to select all Custom workouts for a specific USER
                            command.CommandText = "SELECT tblcustomworkouts.custom_id, tblworkouts.type, tblworkouts.name, tblworkouts.distance, tblworkouts.speed, tblworkouts.time FROM tblcustomworkouts INNER JOIN tblcustomworkouts_has_tblworkouts ON tblcustomworkouts.custom_id = tblcustomworkouts_has_tblworkouts.tblcustomworkouts_custom_id INNER JOIN tblworkouts ON tblcustomworkouts_has_tblworkouts.tblworkouts_workout_id = tblworkouts.workout_id WHERE tblcustomworkouts.user_id_fk = @userId";
                            command.Parameters.AddWithValue("@userId", userId); 

                            reader = await command.ExecuteReaderAsync();

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



                            //Use workouts list and see if there are entries with the same name! If so put them together under a list 
                            //--> all of the workouts alone or merched put into a LIST 
                            //--> send list as response
                            
                            List<string> workoutNames = new List<string>(); //List to know which workouts are processend.
                            for (int i = 0; i < workouts.Count; i++)
                            {
                                string workoutName = workouts[i].Name;
                                if (!workoutNames.Contains(workoutName)) //check if the workout isn't already processed.
                                {
                                    List<Workout> tempWorkoutList = new List<Workout>(); //make a temp list which will include 1 workout with its intervals!

                                    for(int x = 0; x < workouts.Count; x++) // go through all workouts again and check if it has the Name of the currently being processed workout as its name.
                                    {
                                        if(workoutName == workouts[x].Name)
                                        {
                                            tempWorkoutList.Add(workouts[x]);   // add the workout as INTERVAL to the tempWorkoutList
                                            workoutNames.Add(workoutName);      // add the workoutName to the list with processed workoutNames
                                        }
                                    }
                                    groupedWorkouts.Add(tempWorkoutList);   //Add the just Processed workout with intervals to the Big List

                                }
                            }
                        }

                    }
                }


                return new OkObjectResult(groupedWorkouts);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }

        }
    }
}
