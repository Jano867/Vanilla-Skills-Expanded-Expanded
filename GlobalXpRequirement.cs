using System;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Collections.Generic;

namespace VanillaSkillsExpandedExpanded
{

    public class GlobalXpRequirement
    {
        private static int InitialGlobalStart => ExpandedSkillsMod.Settings.InitialGlobalStart;
        private static float linearAddBy => ExpandedSkillsMod.Settings.LinearAddBy;
        private static float curvedMultiplyBy => ExpandedSkillsMod.Settings.CurvedMultiplyBy;

        private static List SkillPointThreshold => 

        //I want to deal with the possible change in progression by passing through the previous milestone, then change the requirements to meet the new threshold based on that
        //I need to figure out how to store the pawns progression data. I think I will make one list that only ever exists, not a bunch of seperate lists for different progression choices
        //This way, the program never awards a point if the player switches, or atleast it doesn't award too many points
        private static float GetFlatProgression(int milestoneId, )
        {
            return InitialGlobalStart;
        }

        private static float GetLinearProgression(int milestoneId)
        {
            float xPRequired = ((linearAddBy * milestoneId) + linearAddBy);
            return xPRequired;
        }
        private static float GetCurvedProgression(int milestoneId)
        {
            float xPRequired = ();//Get List at place [milestoneId] and multiply by curvedMultiplyBy)
            return xPRequired;
        }
        private static float GetExponentialProgression(int milestoneId)
        {

        }
        private static float GetCustomProgression(int milestoneId)
        {

        }
        //public static List<float> CustomProgression = new List<float> { 270000f, }

        //Need to make sure to defensively program in it so they can switch progression type mid game.

    }
}
