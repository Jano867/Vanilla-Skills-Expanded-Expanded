using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaSkillsExpandedExpanded
{

    public class GlobalXpRequirement
    {
        private static float InitialGlobalStart => ExpandedSkillsMod.Settings.InitialGlobalStart;
        private static float linearAddBy => ExpandedSkillsMod.Settings.LinearAddBy;
        private static float curvedMultiplyBy => ExpandedSkillsMod.Settings.CurvedMultiplyBy;
        private static float exponentProgression => ExpandedSkillsMod.Settings.ExponentProgression;

        //Custom Progression Array
        private static float[] customProgression = { 200000, 250000, 300000, 350000, 400000, 500000 };




        //I want to deal with the possible change in progression by passing through the previous milestone, then change the requirements to meet the new threshold based on that
        //I need to figure out how to store the pawns progression data. I think I will make one list that only ever exists, not a bunch of seperate lists for different progression choices
        public class GetFlatProgression : ProgressionRequirement
        {
            public override float GetRequirement(int milestoneId, SkillPointTracker tracker)
            {
                if (HasNoThresholds(tracker) == true)
                {
                    return InitialGlobalStart;
                }
                else
                {
                    return InitialGlobalStart;
                }
            }

        }

        public class GetLinearProgression : ProgressionRequirement //Checked WORKING
        {
            public override float GetRequirement(int milestoneId, SkillPointTracker tracker)
            {
                if (HasNoThresholds(tracker) == true)
                {
                    return InitialGlobalStart;
                }
                else
                {
                    return tracker.thresholdList.Last() + linearAddBy;
                }
            }
        }

        public class GetCurvedProgression : ProgressionRequirement //Checked WORKING
        {
            public override float GetRequirement(int milestoneId, SkillPointTracker tracker)
            {
                if (HasNoThresholds(tracker) == true)
                {
                    return InitialGlobalStart;
                }
                else
                {
                    return tracker.thresholdList.Last() * curvedMultiplyBy;
                }
            }
            public class GetExponentialProgression : ProgressionRequirement //Checked WORKING
            {

                public override float GetRequirement(int milestoneId, SkillPointTracker tracker)
                {
                    if (HasNoThresholds(tracker) == true)
                    {
                        return InitialGlobalStart;
                    }
                    else
                    {
                        return ((float)(Math.Pow(tracker.thresholdList.Last(), exponentProgression)));
                    }
                }
            }
            public class GetCustomProgression : ProgressionRequirement
            {//This is used as a stable fallback no matter how out of hand the other progression styles get

                public override float GetRequirement(int milestoneId, SkillPointTracker tracker)
                {
                    if (HasNoThresholds(tracker) == true)
                    {
                        return customProgression[0];
                    }
                    else if (milestoneId >= 5)
                    {
                        return customProgression[5];
                    }
                    else
                    {
                        return customProgression[milestoneId];
                    }
                }
            }
            public class GetDebugProgression : ProgressionRequirement //Checked WORKING
            {
                public override float GetRequirement(int milestoneId, SkillPointTracker tracker)
                {
                    return 200f;
                }
            }
        }
    }
}
