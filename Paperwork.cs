using DV.Booklets;
using DV.InventorySystem;
using DV.Logic.Job;
using DV.ThingTypes;
using DV.Utils;
using DvMod.Paperwork.Behaviours;
using DvMod.Paperwork.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using VLB;

namespace DvMod.Paperwork
{
    public static class Paperwork
    {
        private static int bookletsRunning;

        public static void CheckJobBooklet(Job job)
        {
            var booklet = JobBooklet.allExistingJobBooklets.FirstOrDefault(x => x.job.ID == job.ID)
                ?? BookletCreator.CreateJobBooklet(job, PlayerManager.PlayerTransform.position, PlayerManager.PlayerTransform.rotation);

            if (booklet.GetComponent<JobBookletCheckmark>() != null)
                return;

            booklet.gameObject.GetOrAddComponent<JobBookletCheckmark>();
            SfxHost.Instance.StartCoroutine(SfxHost.PlayOneShotDelayed(EmbeddedResources.Assets.CheckWav.Value, 2f));
            ForceInventoryRedraw(booklet.gameObject);
        }

        private static void ForceInventoryRedraw(GameObject booklet)
        {
            var inventory = SingletonBehaviour<Inventory>.Instance;
            if (inventory == null)
                return;

            int slot = inventory.IndexOf(booklet);
            if (!InventoryUtils.IsValidInventoryIndex(slot))
                return;

            bool isLocked = inventory.GetSlotLockState(slot);
            bool isReserved = inventory.GetSlotReservedState(slot);
            bool isDropped = inventory.GetSlotDroppedState(slot);

            var itemState = isDropped
                ? InventoryItemState.Dropped
                : InventoryItemState.Disabled;

            var slotState = new InventorySlotState(slot, booklet, itemState, isLocked, isReserved, -1);

            var field = typeof(Inventory).GetField(nameof(Inventory.InventoryStatusChanged), BindingFlags.Instance | BindingFlags.NonPublic);
            var del = field?.GetValue(inventory) as Inventory.InventoryStatusChangedDelegate;
            del?.Invoke(slotState, InventoryActionType.Add, InventorySlotState.Empty, InventoryActionType.None);
        }

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
                        JobState.Available => JobOverviewCache.JobsToJobOverviews.TryGetValue(job, out var overview)
                            ? overview.gameObject
                            : BookletCreator.CreateJobOverview(job, PlayerManager.PlayerTransform.position, PlayerManager.PlayerTransform.rotation).gameObject,
                        JobState.InProgress => JobBooklet.allExistingJobBooklets.FirstOrDefault(x => x.job.ID == job.ID)?.gameObject
                            ?? BookletCreator.CreateJobBooklet(job, PlayerManager.PlayerTransform.position, PlayerManager.PlayerTransform.rotation).gameObject,
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
