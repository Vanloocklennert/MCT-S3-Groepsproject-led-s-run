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
                    string url = endpoint + "/leaderboard";
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
    }
}
