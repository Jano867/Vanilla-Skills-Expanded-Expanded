using System;
using Verse;
using RimWorld;
using HarmonyLib;
using VSE;
using VSE.Expertise;
using UnityEngine;


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
            harm.Patch(AccessTools.Method(typeof(CharacterCardUtility), nameof(CharacterCardUtility.DrawCharacterCard)),
              postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(SkillPointDisplayUI)));
            //harm.Patch(AccessTools.Method(typeof(SaveGameFilesUtility)))
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
            int milestoneId = tracker.ClaimedMilestoneCount();
            float milestoneRequirement = ExpandedSkillsMod.Settings.GlobalXpRequirement + (200 * milestoneId);

            if (tracker.AwardPointForMilestone(milestoneRequirement, milestoneId))
            {
                Log.Message($"Awarded Skill Point to {__instance.Pawn.LabelShort}, Reached Milestone Requirement, Available Skill Points: {tracker.availableSkillPoints}");

            }
            if (__instance.GetLevel() >= 15)
            {
                string skillDefName = __instance.def.defName;

                if (!tracker.HasClaimedLevel15Skill(skillDefName))
                {
                    tracker.availableSkillPoints++;
                    tracker.ClaimLevel15Skill(skillDefName);
                    Log.Message($"Awarded Skill Point to {__instance.Pawn.LabelShort}, Reached Level 15, Available Skill Points: {tracker.availableSkillPoints}");
                }
            }

            //Debug 
            //Log.Message($"Pawn: {__instance.Pawn.LabelShort}, {xp}, Total Tracked XP: {tracker.trackedGlobalXp},Milestone XP Requirement: {milestoneRequirement}, Progress: {tracker.trackedGlobalXp}/{milestoneRequirement}");

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
            if (__instance == null || __instance.Pawn == null)
            {
                return;
            }

            SkillPointTracker tracker = SkillPointTrackers.GetTracker(__instance.Pawn);
            if (tracker == null)
            {
                return;
            }

            if (tracker.availableSkillPoints > 0)
            {
                tracker.availableSkillPoints--;
                Log.Message($"Pawn: {__instance.Pawn.LabelShort}, Skill Point Subtracted, New Total Skill Points: {tracker.availableSkillPoints}");
            }
        }

        public static void SkillPointDisplayUI(Rect rect, Pawn pawn)
        {
            if (pawn == null || !pawn.IsColonist)
            {
                return;
            }
            SkillPointTracker tracker = SkillPointTrackers.GetTracker(pawn);
            if (tracker == null)
            {
                return;
            }

            int milestoneId = tracker.ClaimedMilestoneCount();
            float milestoneRequirement = ExpandedSkillsMod.Settings.GlobalXpRequirement + (200 * milestoneId);
            int milestoneProgress = ((int)(100 * (tracker.trackedGlobalXp / milestoneRequirement)));

            TextAnchor anchor = Text.Anchor;
            GameFont font = Text.Font;
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;


            Rect skillPointNum = new Rect(rect.x + 320f, rect.y + 35f, 190f, 25f);
            Widgets.Label(skillPointNum, $"SP: {tracker.availableSkillPoints} | Next: {milestoneProgress}%");

            Text.Anchor = anchor;
            Text.Font = font;
        }

        //public static void CSVAfterSave()
        //{
        //    CsvExporter.CSVExport(SkillPointTrackers.AllTrackers());
        //}
    }
}
