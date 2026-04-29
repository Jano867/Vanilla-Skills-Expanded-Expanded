using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaSkillsExpandedExpanded
{
    public abstract class ProgressionRequirement
    {
        public abstract float GetRequirement(int milestoneId, SkillPointTracker tracker);

        protected float InitialGlobalStart
        {
            get
            {
                return ExpandedSkillsMod.Settings.InitialGlobalStart;
            }
        }
        protected bool HasNoThresholds(SkillPointTracker tracker)
        {
            if (tracker.thresholdList.Count == null || tracker.thresholdList.Count==0)
            {
                return false;
            }
            return true;
        }


    }
}
