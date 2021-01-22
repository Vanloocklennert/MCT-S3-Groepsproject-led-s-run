using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Leds_Run.models;
using Newtonsoft.Json;
using System.Diagnostics;

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

        public async static Task<List<Workout>> GetCostumWorkouts()
        {
            //Costumworkout opvragen
            List<Workout> workouts = new List<Workout>();
            return workouts;
        }


        public async static Task<Leaderboard> GetLeaderboard()
        {
            //Leaderboard opvragen
            Leaderboard Leaderboards = new Leaderboard();
            return Leaderboards;
        }

    }
}
