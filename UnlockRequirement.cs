using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaSkillsExpandedExpanded
{
    public class UnlockRequirement
    {
        
        public static float Progression(ProgressionChoice choice, SkillPointTracker tracker)
        {
            int milestoneId = tracker.ClaimedMilestoneCount();
            float linearAdd = ExpandedSkillsMod.Settings.LinearProgressionAddBy;
            float entry;
            switch (choice)
            {
                case ProgressionChoice.Flat:
                    entry = GetFlatProgression(milestoneId, linearAdd);

                    break;
                case ProgressionChoice.Linear:
                    break;
                case ProgressionChoice.Curved:
                    break;
                case ProgressionChoice.Exponential:
                    break;
                case ProgressionChoice.Custom:
                    break;
            }

            return entry;
        }
    }
}
