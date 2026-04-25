using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VanillaSkillsExpandedExpanded
{
    public class UnlockRequirement
    {
        private static float InitialGlobalStart => ExpandedSkillsMod.Settings.InitialGlobalStart;
        public static float Progression(ProgressionChoice choice, SkillPointTracker tracker)
        {
            int milestoneId = tracker.ClaimedMilestoneCount();
            
            float entry;
            switch (choice)
            {
                case ProgressionChoice.Flat:
                    Log.Message("Flat Progression Chosen");
                    entry = GlobalXpRequirement.GetFlatProgression(milestoneId, tracker);
                    break;
                case ProgressionChoice.Linear:
                    Log.Message("Linear Progression Chosen");
                    entry = GlobalXpRequirement.GetLinearProgression(milestoneId, tracker);
                    break;
                case ProgressionChoice.Curved:
                    Log.Message("Curved Progression Chosen");
                    entry = GlobalXpRequirement.GetCurvedProgression(milestoneId, tracker);
                    break;
                case ProgressionChoice.Exponential:
                    Log.Message("Exponential Progression Chosen");
                    entry = GlobalXpRequirement.GetExponentialProgression(milestoneId, tracker);
                    break;
                case ProgressionChoice.Custom:
                    //Log.Message("Custom Progression Chosen");
                    entry = GlobalXpRequirement.GetCustomProgression(milestoneId, tracker);
                    break;
                case ProgressionChoice.Debug:
                    Log.Message($"Debug Progression Chosen");
                    entry = GlobalXpRequirement.GetDebugProgression();
                    break;
                default:
                    Log.Message($"DEFAULT CASE VALUE, ASSIGN A PROGRESSION CHOICE");
                    entry = ExpandedSkillsMod.Settings.InitialGlobalStart;
                    break;
            }
            //Log.Message($"Returning value in Progression with choice: {choice}, Returned Value: {entry}");
            return entry;
        }
    }
}
