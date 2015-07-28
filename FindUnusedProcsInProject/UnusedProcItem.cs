using System;
using System.Collections.Generic;
using FindUnusedProcsInProject.Annotations;

namespace FindUnusedProcsInProject
{
    public class UnusedProcItem
    {
        private Guid ProcItemHitGuid { [UsedImplicitly] get; set; }
        public string ProcName { get; set; }
        public int CountOfHits { get; set; }
        public List<Tuple<string, int, string>> ContextList { get; set; }

        public UnusedProcItem()
        {
            ProcItemHitGuid = Guid.NewGuid();
            ContextList = new List<Tuple<string, int, string>>();
        }
    }
}
