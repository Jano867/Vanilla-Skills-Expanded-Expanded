using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static VanillaSkillsExpandedExpanded.GlobalXpRequirement;
using static VanillaSkillsExpandedExpanded.GlobalXpRequirement.GetCurvedProgression;

namespace VanillaSkillsExpandedExpanded
{
    public class UnlockRequirement
    {
        public static float Progression(ProgressionChoice choice, SkillPointTracker tracker)
        {
            int milestoneId = tracker.ClaimedMilestoneCount();

            ProgressionRequirement requirement;
            switch (choice)
            {
                case ProgressionChoice.Flat:
                    Log.Message("Flat Progression Chosen");
                    requirement = new GetFlatProgression();
                    break;
                case ProgressionChoice.Linear:
                    Log.Message("Linear Progression Chosen");
                    requirement = new GetLinearProgression();
                    break;
                case ProgressionChoice.Curved:
                    Log.Message("Curved Progression Chosen");
                    requirement = new GetCurvedProgression();
                    break;
                case ProgressionChoice.Exponential:
                    Log.Message("Exponential Progression Chosen");
                    requirement = new GetExponentialProgression();
                    break;
                case ProgressionChoice.Custom:
                    requirement = new GetCustomProgression();
                    break;
                case ProgressionChoice.Debug:
                    Log.Message($"Debug Progression Chosen");
                    requirement = new GetDebugProgression();
                    break;
                default:
                    Log.Message($"DEFAULT CASE VALUE, ASSIGN A PROGRESSION CHOICE");
                    requirement =new GetCustomProgression();
                    break;
            }
            return requirement.GetRequirement(milestoneId,tracker);
        }
    }
}
