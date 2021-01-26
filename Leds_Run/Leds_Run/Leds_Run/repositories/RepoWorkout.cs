using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Leds_Run.models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Net;

namespace Leds_Run.repositories
{
    class RepoWorkout
    {
        private static readonly string endpoint = "https://leds-run.azurewebsites.net/api/v1/";

        private static async Task<HttpClient> GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            return client;
        }

        // GETS
        public async static Task<List<Workout.Interval>> GetDefaultWorkouts()
        {
            //default workouts opvragen
            using (HttpClient client = await GetClient())
            {
                try
                {
                    string url = endpoint + "workouts";
                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        List<List<Workout.Interval>> workouts = JsonConvert.DeserializeObject<List<List<Workout.Interval>>>(json);
                        List<Workout.Interval> intervalList = new List<Workout.Interval>();
                        foreach (List<Workout.Interval> intervals in workouts)
                        {
                            intervalList.Add(intervals[0]);
                        }
                        return intervalList;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }
        public async static Task<List<Workout.Interval>> GetChallenges()
        {
            //default challenges opvragen
            using (HttpClient client = await GetClient())
            {
                try
                {
                    string url = endpoint + "challenges";
                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        List<List<Workout.Interval>> challenges = JsonConvert.DeserializeObject<List<List<Workout.Interval>>>(json);
                        List<Workout.Interval> challengeList = new List<Workout.Interval>();
                        foreach (List<Workout.Interval> challenge in challenges)
                        {
                            challengeList.Add(challenge[0]);
                        }
                        return challengeList;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }
        public async static Task<List<Workout>> GetCostumWorkouts(string id)
        {
            //Costumworkout opvragen
            using (HttpClient client = await GetClient())
            {
                try
                {
                    string url = endpoint + $"{id}/workouts";
                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {

                        // some list magic POGGERS. Anders deseriliazed em ni sadge
                        List<List<Workout.Interval>> intervals = JsonConvert.DeserializeObject<List<List<Workout.Interval>>>(json);
                        List<Workout> workouts = new List<Workout>();

                        foreach(List<Workout.Interval> workout in intervals)
                        {
                            Workout curWorkout = new Workout();
                            curWorkout.Intervals = workout;
                            workouts.Add(curWorkout);
                        }

                        return workouts;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }
        public async static Task<Leaderboard> GetLeaderboard()
        {
            //Leaderboard opvragen
            using (HttpClient client = await GetClient())
            {
                try
                {
                    string url = endpoint + "leaderboard";
                    string json = await client.GetStringAsync(url);

                    if (json != null)
                    {
                        List<Leaderboard.Entry> entries = JsonConvert.DeserializeObject<List<Leaderboard.Entry>>(json);
                        Leaderboard leaderboard = new Leaderboard
                        {
                            entries = entries
                        };
                        return leaderboard;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }

        // POSTS
        public async static Task<bool> GetUserLogin(string email, string password)
        {
            //checken of user kan inloggen
            using (HttpClient client = await GetClient())
            {
                try
                {
                    string url = endpoint + "userlogin";

                    string passwordHash = Hash(email + password);
                    StringContent content = new StringContent($"{{'email': '{email}','passwordhash':'{passwordHash}'}}", Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    string json = await response.Content.ReadAsStringAsync();
                    Dictionary<string, string> resultObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    if (resultObject["succes"] == "true")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
        }
        public async static Task<bool> CreateUser(string username, string email, string password)
        {
            bool succes = false;

            //checken of user kan inloggen
            using (HttpClient client = await GetClient())
            {
                try
                {
                    string url = endpoint + "user";
                    string passwordHash = Hash(email + password);
                    StringContent content = new StringContent($"{{'username': '{username}','passwordhash':'{passwordHash}', 'email':'{email}'}}", Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        succes = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    succes = false;
                }

                return succes;
            }
        }        
        public async static Task<bool> CreateWorkout(string id, Workout workout)
        {
            //checken of user kan inloggen
            using (HttpClient client = await GetClient())
            {
                try
                {
                    string url = endpoint + $"user{id}/workouts";

                    string json = JsonConvert.SerializeObject(workout);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
        }
        public async static Task<bool> CreateLeaderBoardEntry(string username, string time, string distance, string speed)
        {
            //checken of user kan inloggen
            using (HttpClient client = await GetClient())
            {
                try
                {
                    string url = endpoint + "/leaderboard";

                    string json = $@"{{
                                    'username': '{username}',
                                    'time': '{time}',
                                    'distance': {distance},
                                    'speed': {speed}
                                    }}";

                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
        }
        public static string Hash(string data)
        {

            byte[] byteData = Encoding.UTF8.GetBytes(data);
            byte[] hash;

            using (SHA256 shaM = new SHA256Managed())
            {
                hash = shaM.ComputeHash(byteData);
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
