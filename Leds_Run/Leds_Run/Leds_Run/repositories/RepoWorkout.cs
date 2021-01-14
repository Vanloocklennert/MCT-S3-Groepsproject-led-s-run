using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Leds_Run.models;

namespace Leds_Run.repositories
{
    class RepoWorkout
    {


        public async static Task<List<Workout>> GetDefaultWorkouts()
        {
            //default workouts opvragen
            List<Workout> workouts = new List<Workout>();
            return workouts;
        }

        public async static Task<List<Workout>> GetCostumWorkouts()
        {
            //Costumworkout opvragen
            List<Workout> workouts = new List<Workout>();
            return workouts;
        }


        //public async static Task<List<Leaderboard>> GetLeaderboard()
        //{
        //    //Leaderboard opvragen
        //    List<Leaderboard> Leaderboards = new List<Leaderboard>();
        //    return List<Leaderboard>
        //}

    }
}
