using System;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace VanillaSkillsExpandedExpanded
{

    public class GlobalXpRequirement
    {
        private static float InitialGlobalStart => ExpandedSkillsMod.Settings.InitialGlobalStart;
        private static float linearAddBy => ExpandedSkillsMod.Settings.LinearAddBy;
        private static float curvedMultiplyBy => ExpandedSkillsMod.Settings.CurvedMultiplyBy;
        private static float exponentProgression => ExpandedSkillsMod.Settings.ExponentProgression;

        //Custom Progression List
        private static float[] customList = { 200000, 250000, 300000, 350000, 400000, 500000 };

        
        

        //I want to deal with the possible change in progression by passing through the previous milestone, then change the requirements to meet the new threshold based on that
        //I need to figure out how to store the pawns progression data. I think I will make one list that only ever exists, not a bunch of seperate lists for different progression choices
        //This way, the program never awards a point if the player switches, or atleast it doesn't award too many points
        public static float GetFlatProgression(int milestoneId, SkillPointTracker tracker)
        {
            return InitialGlobalStart;
        }

        public static float GetLinearProgression(int milestoneId, SkillPointTracker tracker) //Checked WORKING
        {
            if (tracker.thresholdList == null || tracker.thresholdList.Count == 0)
            {
                return InitialGlobalStart;
            }
            float xPRequired = (tracker.thresholdList.Last() + linearAddBy);
            return xPRequired;
        }
        public static float GetCurvedProgression(int milestoneId, SkillPointTracker tracker) //Checked WORKING
        {
            if (tracker.thresholdList == null || tracker.thresholdList.Count == 0) 
            {
                return InitialGlobalStart;
            }
            float xPRequired = (tracker.thresholdList.Last()*curvedMultiplyBy);
            return xPRequired;
        }
        public static float GetExponentialProgression(int milestoneId, SkillPointTracker tracker) //Checked WORKING
        {
            if (tracker.thresholdList == null || tracker.thresholdList.Count == 0)
            {
                return InitialGlobalStart;
            }
            float xPRequired = ((float)(Math.Pow(tracker.thresholdList.Last(), exponentProgression)));
            return xPRequired;

        }
        public static float GetCustomProgression(int milestoneId, SkillPointTracker tracker)//Checked Kind of Working just don't use with logs turned on
        {//This is used as a stable fallback no matter how out of hand the other progression styles get
            
            if (tracker.thresholdList == null || tracker.thresholdList.Count == 0)
            {
                return customList[0];
            }
            else if (milestoneId >= 5)
            {
                return customList[5];
            }
            else
            {
                return customList[milestoneId];
            }
        }
        public static float GetDebugProgression() //Checked WORKING
        {
            return 200f;
        }
    }
}
