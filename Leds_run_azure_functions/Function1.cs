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
using Newtonsoft.Json.Linq;

namespace Leds_run_azure_functions
{
    public static class Function1
    {
        //------------------------------------------------------ USER ------------------------------------------------------
        // Function to Add a new user to the DB
        [FunctionName("AddUserV1")]
        public static async Task<IActionResult> AddUserV1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/user")] HttpRequest req,
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
                    
                    if(newuser.Username == null || newuser.EMail == null || newuser.PasswordHash == null)
                    {
                        Response response = new Response();
                        response.Succes = false;
                        response.Info = "Error: Not enough parameters given! (username, email, passwordhash)";
                        return new BadRequestObjectResult(response); // 400
                    }

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO tblusers (username, email, passwordhash) VALUES(@username, @email, @passwordhash)";
                        command.Parameters.AddWithValue("@username", newuser.Username);
                        command.Parameters.AddWithValue("@email", newuser.EMail);
                        command.Parameters.AddWithValue("@passwordhash", newuser.PasswordHash);
                        //log.LogInformation("ok");
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


        // Function to Know if a User may be logged in or not POST
        [FunctionName("GetUserLoginPermissionV1")]
        public static async Task<IActionResult> GetUserLoginPermissionV1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/userlogin")] HttpRequest req,
            ILogger log)
        {

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                User userToLogin = JsonConvert.DeserializeObject<User>(requestBody); // user given in the JSON

                User userFromDB = new User(); //User you recieve as a result by searching userToLogin in DB

                Response response = new Response();
                response.Succes = false;

                string connectionString = Environment.GetEnvironmentVariable("SQLServer");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        bool requiredParams = false;
                        // Make query depending on given credentials to search for the user
                        if((userToLogin.Username != null) && (userToLogin.PasswordHash != null))
                        {
                            command.CommandText = "SELECT user_id, username, email, passwordhash FROM tblusers WHERE username=@username";
                            command.Parameters.AddWithValue("@username", userToLogin.Username);
                            requiredParams = true;
                        }
                        else if ((userToLogin.EMail != null) && (userToLogin.PasswordHash != null))
                        {
                            command.CommandText = "SELECT user_id, username, email, passwordhash FROM tblusers WHERE email=@email";
                            command.Parameters.AddWithValue("@email", userToLogin.EMail);
                            requiredParams = true;
                        }

                        //are all required parameters given in the Json
                        if (requiredParams == true)
                        {
                            SqlDataReader reader = await command.ExecuteReaderAsync();

                            while (await reader.ReadAsync())
                            {
                                userFromDB.User_Id = int.Parse(reader["user_id"].ToString());
                                userFromDB.Username = reader["username"].ToString();
                                userFromDB.EMail = reader["email"].ToString();
                                userFromDB.PasswordHash = reader["passwordhash"].ToString();
                            }
                            reader.Close();

                            //see if user exists
                            if(userFromDB.User_Id != 0)
                            {
                                //Is it a correct Login?
                                if(userFromDB.PasswordHash == userToLogin.PasswordHash)
                                {
                                    response.Succes = true;
                                    response.Info = "User may log in!";
                                }
                                else
                                {
                                    response.Info = "User found but wrong credentials!";
                                    return new BadRequestObjectResult(response); // 400
                                }
                            }
                            else
                            {
                                response.Info = "User not found!";
                                return new BadRequestObjectResult(response); // 400
                            }
                        }
                        else
                        {
                            response.Info = "You did not return all necessary parameters!";
                            return new BadRequestObjectResult(response); // 400
                        }
                        
                    }
                }

                return new OkObjectResult(response); // 200

            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }

        }

        //-------------------------------------------------- Default Workouts --------------------------------------------------
        // Function to GET all default Workouts out of the DB
        [FunctionName("GetDefaultWorkoutsV1")]
        public static async Task<IActionResult> GetDefaultWorkoutsV1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/workouts")] HttpRequest req,
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
                            string time = reader["time"].ToString();
                            if (time != "")
                            {
                                workout.Time = TimeSpan.Parse(time);
                            }
                            workouts.Add(workout);
                        }

                    }
                }

                // NOT NECESSARY but done for Frontend ease
                List<List<Workout>> groupedWorkouts = new List<List<Workout>>(); // List of workouts with there INTERVALS in it as a list.

                for (int i = 0; i < workouts.Count; i++)
                {
                    List<Workout> tempWorkoutList = new List<Workout>();
                    tempWorkoutList.Add(workouts[i]);
                    groupedWorkouts.Add(tempWorkoutList);
                }
                
                return new OkObjectResult(groupedWorkouts);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }

        }


        // Function to GET all default challenges out of the DB
        [FunctionName("GetChallengesV1")]
        public static async Task<IActionResult> GetChallengesV1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/challenges")] HttpRequest req,
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
                            string time = reader["time"].ToString();
                            if (time != "")
                            {
                                workout.Time = TimeSpan.Parse(time);
                            }
                            workouts.Add(workout);
                        }

                    }
                }

                // NOT NECESSARY but done for Frontend ease
                List<List<Workout>> groupedWorkouts = new List<List<Workout>>(); // List of workouts with there INTERVALS in it as a list.

                for (int i = 0; i < workouts.Count; i++)
                {
                    List<Workout> tempWorkoutList = new List<Workout>();
                    tempWorkoutList.Add(workouts[i]);
                    groupedWorkouts.Add(tempWorkoutList);
                }

                return new OkObjectResult(groupedWorkouts);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }

        }


        //--------------------------------------------------- USER WORKOUTS ---------------------------------------------------
        // Function to GET all default Workouts out of the DB
        [FunctionName("GetUserWorkoutsV1")]
        public static async Task<IActionResult> GetUserWorkoutsV1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/{username}/workouts")] HttpRequest req,
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
                        

                        command.CommandText = "SELECT user_id, username, email FROM tblusers WHERE username=@username";
                        command.Parameters.AddWithValue("@username", username);

                        SqlDataReader reader = await command.ExecuteReaderAsync();

                        int userId = 0;
                        while (await reader.ReadAsync())
                        {
                            userId = int.Parse(reader["user_id"].ToString());
                        }
                        reader.Close();

                        if (userId != 0)
                        {
                            // Make query to select all Custom workouts for a specific USER
                            command.CommandText = "SELECT tblcustomworkouts.custom_id, tblworkouts.type, tblworkouts.name, tblworkouts.distance, tblworkouts.speed, tblworkouts.time FROM tblcustomworkouts INNER JOIN tblcustomworkouts_has_tblworkouts ON tblcustomworkouts.custom_id = tblcustomworkouts_has_tblworkouts.tblcustomworkouts_custom_id INNER JOIN tblworkouts ON tblcustomworkouts_has_tblworkouts.tblworkouts_workout_id = tblworkouts.workout_id WHERE tblcustomworkouts.user_id_fk = @userId";
                            command.Parameters.AddWithValue("@userId", userId); 

                            reader = await command.ExecuteReaderAsync();

                            while (await reader.ReadAsync())
                            {
                                Workout workout = new Workout();
                                workout.Default_Id = int.Parse(reader["custom_id"].ToString());
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
                        else
                        {
                            Response response = new Response();
                            response.Succes = false;
                            response.Info = "The requested user was not found!";
                            return new BadRequestObjectResult(response); // 400
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


        // Function to Add a new workout for a given user to the DB
        [FunctionName("AddUserWorkoutV1")]
        public static async Task<IActionResult> AddUserWorkoutV1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/{username}/workouts")] HttpRequest req,
            string username,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                List<Workout> userWorkouts = JsonConvert.DeserializeObject<List<Workout>>(requestBody);

                string connectionString = Environment.GetEnvironmentVariable("SQLServer");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();


                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;

                        //Get UserId by searching for username in tblusers
                        command.CommandText = "SELECT user_id, username FROM tblusers WHERE username=@username";
                        command.Parameters.AddWithValue("@username", username);

                        SqlDataReader reader = await command.ExecuteReaderAsync();

                        int userId = 0;
                        while (await reader.ReadAsync())
                        {
                            userId = int.Parse(reader["user_id"].ToString());
                        }
                        reader.Close();
                        //------
                        

                        if (userId != 0)
                        {
                            // 1. -- -- -- --
                            //Create new Custom workout with UserId in tblcustomworkouts
                            command.CommandText = "INSERT INTO tblcustomworkouts (user_id_fk) OUTPUT INSERTED.custom_id VALUES(@userId)";
                            command.Parameters.AddWithValue("@userId", userId);


                            SqlDataReader readerForId = await command.ExecuteReaderAsync();

                            int customWorkoutId = 0;
                            while (await readerForId.ReadAsync())
                            {
                                customWorkoutId = int.Parse(readerForId["custom_id"].ToString());
                            }
                            readerForId.Close();
                            //------

                            // 2. -- -- -- --
                            //Create the workout its intervals as different rows in tblworkouts
                            List<int> workoutIds = new List<int>();

                            string workoutQuery = "INSERT INTO tblworkouts (type, name, distance, speed, time) OUTPUT INSERTED.workout_id VALUES";
                            for (int i = 0; i < userWorkouts.Count; i++)
                            {
                                if (i != (userWorkouts.Count - 1))
                                {
                                    workoutQuery = workoutQuery + "('" + userWorkouts[i].Type + "', '" + userWorkouts[i].Name + "', " + userWorkouts[i].Distance.ToString().Replace(',', '.') + ", " + userWorkouts[i].Speed.ToString().Replace(',', '.') + ", '" + userWorkouts[i].Time + "'), ";
                                }
                                else
                                {
                                    workoutQuery = workoutQuery + "('" + userWorkouts[i].Type + "', '" + userWorkouts[i].Name + "', " + userWorkouts[i].Distance.ToString().Replace(',', '.') + ", " + userWorkouts[i].Speed.ToString().Replace(',', '.') + ", '" + userWorkouts[i].Time + "')";
                                }
                            }
                            command.CommandText = workoutQuery;
                            readerForId = await command.ExecuteReaderAsync();

                            int workoutId = 0;
                            while (await readerForId.ReadAsync())
                            {
                                workoutId = int.Parse(readerForId["workout_id"].ToString());
                                workoutIds.Add(workoutId);
                            }
                            readerForId.Close();



                            log.LogInformation("2");

                            //3. -- -- -- --
                            //Insert an interval into the database
                            string query = "INSERT INTO tblcustomworkouts_has_tblworkouts (tblcustomworkouts_custom_id, tblworkouts_workout_id) VALUES";
                            for (int x = 0; x < workoutIds.Count; x++)
                            {
                                if (x != (workoutIds.Count - 1))
                                {
                                    query = query + "(" + customWorkoutId + ", " + workoutIds[x] + "), ";
                                }
                                else
                                {
                                    query = query + "(" + customWorkoutId + ", " + workoutIds[x] + ")";
                                }
                            }

                            command.CommandText = query;
                            await command.ExecuteNonQueryAsync();
                            //------
                            log.LogInformation("3");

                            return new OkObjectResult(userWorkouts);
                        }
                        else
                        {
                            Response response = new Response();
                            response.Succes = false;
                            response.Info = "The given username was not found!";
                            return new BadRequestObjectResult(response); // 400
                        }
                    }
                }


                
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }


        //--------------------------------------------------- LEADERBOARD ---------------------------------------------------
        // Function to Add a new Leaderboard Entry to the DB
        [FunctionName("AddLeaderboardEntryV1")]
        public static async Task<IActionResult> AddLeaderboardEntryV1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/leaderboard")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Leaderboard newLeaderboardEntry = JsonConvert.DeserializeObject<Leaderboard>(requestBody);
                
                if(newLeaderboardEntry.Username == null || newLeaderboardEntry.Time == null || newLeaderboardEntry.Distance == 0 || newLeaderboardEntry.Speed == 0)
                {
                    Response response = new Response();
                    response.Succes = false;
                    response.Info = "Error: Not enough parameters given! (username, time, distance, speed)";
                    return new BadRequestObjectResult(response); // 400
                }

                string connectionString = Environment.GetEnvironmentVariable("SQLServer");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO tblleaderboard (username, time, distance, speed) VALUES(@username, @time, @distance, @speed)";
                        command.Parameters.AddWithValue("@username", newLeaderboardEntry.Username);
                        command.Parameters.AddWithValue("@time", newLeaderboardEntry.Time);
                        command.Parameters.AddWithValue("@distance", newLeaderboardEntry.Distance);
                        command.Parameters.AddWithValue("@speed", newLeaderboardEntry.Speed);
                        log.LogInformation("ok");
                        await command.ExecuteNonQueryAsync();

                    }
                }


                return new OkObjectResult(newLeaderboardEntry);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }

        // Function to GET all Leaderboard Entries out of the DB
        [FunctionName("GetLeaderboardEntriesV1")]
        public static async Task<IActionResult> GetLeaderboardEntriesV1(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/leaderboard")] HttpRequest req,
            ILogger log)
        {

            try
            {
                List<Leaderboard> leaderboardEntries = new List<Leaderboard>();


                string connectionString = Environment.GetEnvironmentVariable("SQLServer");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        // Make query to select all default workouts (tbldefaultworkouts) with an Inner join to tblworkouts
                        command.CommandText = "SELECT leaderboard_id, username, time, distance, speed, datetime FROM tblleaderboard ORDER BY time ASC";

                        SqlDataReader reader = await command.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            Leaderboard leaderboardEntry = new Leaderboard();
                            leaderboardEntry.LeaderboardId = int.Parse(reader["leaderboard_id"].ToString());
                            leaderboardEntry.Username = reader["username"].ToString();
                            leaderboardEntry.Distance = double.Parse(reader["distance"].ToString());
                            leaderboardEntry.Speed = double.Parse(reader["speed"].ToString());
                            leaderboardEntry.DateTime = DateTime.Parse(reader["datetime"].ToString());
                            string time = reader["time"].ToString();
                            if (time != "")
                            {
                                leaderboardEntry.Time = TimeSpan.Parse(time);
                            }
                            leaderboardEntries.Add(leaderboardEntry);
                        }

                    }
                }

                return new OkObjectResult(leaderboardEntries);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }

        }
    }
}
