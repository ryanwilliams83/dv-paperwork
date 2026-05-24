using DV.InventorySystem;
using DV.ThingTypes;
using DvMod.Paperwork.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace DvMod.Paperwork
{
    public static class Paperwork
    {
        private static int bookletsRunning;

        public static void GiveBookletsForTrainset(Trainset trainset)
        {
            try
            {
                if (Interlocked.Exchange(ref bookletsRunning, 1) == 1)
                    return;

                if (!TrainsetCache.TrainsetToJobs.TryGetValue(trainset, out var jobs))
                    return;

                IEnumerable<GameObject?> papers = jobs
                    .Select(job => job.State switch
                    {
                        JobState.Available => JobOverviewCache.JobsToJobOverviews.TryGetValue(job, out var overview) ? overview.gameObject : null,
                        JobState.InProgress => JobBooklet.allExistingJobBooklets.FirstOrDefault(x => x.job.ID == job.ID)?.gameObject,
                    })
                    .Where(x => x != null);

                var inv = Inventory.Instance;
                foreach (var paper in papers)
                {
                    if (paper == null
                        || inv.Contains(paper)
                        || !inv.CanAddItem(paper))
                        continue;

                    inv.AddItemToInventory(paper);
                }
            }
            finally
            {
                Interlocked.Exchange(ref bookletsRunning, 0);
            }
        }
    }
}
