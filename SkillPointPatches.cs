using System;
using Verse;
using RimWorld;
using HarmonyLib;
using VSE;
using VSE.Expertise;


namespace VanillaSkillsExpandedExpanded
{
    public static class SkillPointPatches
    {

        public static void Do(Harmony harm)
        {
            harm.Patch(AccessTools.Constructor(typeof(Pawn_SkillTracker), new Type[] { typeof(Pawn) }),
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(CreateTracker)));
            harm.Patch(AccessTools.Method(typeof(Pawn_SkillTracker), nameof(Pawn_SkillTracker.ExposeData)),
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(SaveTracker)));
            harm.Patch(AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn)),
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(TrackGlobalXp)));
            harm.Patch(AccessTools.Method(typeof(ExpertiseDef), nameof(ExpertiseDef.CanApplyOn)),
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(CheckSkillPoints)));
            harm.Patch(AccessTools.Method(typeof(ExpertiseTracker), nameof(ExpertiseTracker.AddExpertise)),
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(SubtractSkillPoint)));
          
        }

        public static void CreateTracker(Pawn_SkillTracker __instance)
        {
            SkillPointTrackers.CreateTracker(__instance);
        }

        public static void SaveTracker(Pawn_SkillTracker __instance)
        {
            SkillPointTrackers.SaveTracker(__instance);
        }

        public static void TrackGlobalXp(SkillRecord __instance, float xp)
        {
            if (xp <= 0f)
            {
                return;
            }
            if (__instance == null || __instance.Pawn == null)
            {
                return;
            }
            SkillPointTracker tracker = SkillPointTrackers.GetTracker(__instance.Pawn);
            if (tracker == null)
            {
                return;
            }
            tracker.AddGlobalXp(xp);

            if (tracker.AwardPointForMilestone(ExpandedSkillsMod.Settings.GlobalXpRequirement, 0))
            {
                Log.Message($"Awarded Skill Point to {__instance.Pawn.LabelShort} "+
                    $"Available Skill Points: {tracker.availableSkillPoints}");
                ExpandedSkillsMod.Settings.GlobalXpRequirement *= 2;
            }


            Log.Message($"Pawn: {__instance.Pawn.LabelShort}, {xp}, Total Tracked XP: {tracker.trackedGlobalXp},Global XP Requirement: {ExpandedSkillsMod.Settings.GlobalXpRequirement}, Progress: {tracker.trackedGlobalXp}/{ExpandedSkillsMod.Settings.GlobalXpRequirement}");
            
        }

        public static void CheckSkillPoints(Pawn pawn, ref string reason, ref bool __result)
        {
            if (!__result)
            {
                return;
            }
            SkillPointTracker tracker = SkillPointTrackers.GetTracker(pawn);
            if (tracker == null)
            {
                reason = "No tracker found";
                __result = false;
                return;
            }
            if (tracker.availableSkillPoints <= 0)
            {
                reason = "Needs a specialization point";
                __result = false;
            }
           
        }

        public static void SubtractSkillPoint(ExpertiseTracker __instance)
        {
            if (__instance == null||__instance.Pawn==null)
            {
                return;
            }
            
            SkillPointTracker tracker = SkillPointTrackers.GetTracker(__instance.Pawn);
            if (tracker.availableSkillPoints > 0)
            {
                tracker.availableSkillPoints--;
                Log.Message($"Pawn: {__instance.Pawn}, Skill Point Subtracted, New Total Skill Points: {tracker.availableSkillPoints}");
            }
        }
        
    }
}
