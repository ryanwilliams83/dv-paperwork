using DV.Logic.Job;
using System.Collections.Generic;

namespace DvMod.Paperwork.Cache
{
    public static class JobOverviewCache
    {
        public static Dictionary<Job, JobOverview> JobsToJobOverviews = new Dictionary<Job, JobOverview>();
    }
}
