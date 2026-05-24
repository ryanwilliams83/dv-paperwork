using DV.Logic.Job;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DvMod.Paperwork.Cache
{
    public static class TrainsetCache
    {
        public static Dictionary<Trainset, List<Job>> TrainsetToJobs = new Dictionary<Trainset, List<Job>>();

        public static void Rebuild(JobsManager jobsManager)
        {
            Debug.Log($"{nameof(TrainsetCache)}.{nameof(Rebuild)}()");

            TrainsetToJobs = Trainset.allSets
                .ToDictionary(ts => ts, ts => jobsManager
                        .jobToJobCars
                        .Where(j => j.Value.Overlaps(ts.cars.Select(x => x.logicCar)))
                        .Select(x => x.Key)
                        .Distinct()
                        .ToList());
        }
    }
}
