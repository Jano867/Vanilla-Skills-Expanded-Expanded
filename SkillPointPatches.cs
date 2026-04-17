using System;
using Verse;
using RimWorld;
using HarmonyLib;


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
        }
    }
}
