using System;
using Verse;
using RimWorld;
using HarmonyLib;
using VSE;
using VSE.Expertise;
using UnityEngine;
using System.Linq;
using System.Diagnostics;


namespace VanillaSkillsExpandedExpanded
{
    public static class SkillPointPatches
    {
        public static void Do(Harmony harm)
        { //Patch all the methods into the game here:
            harm.Patch(AccessTools.Constructor(typeof(Pawn_SkillTracker), new Type[] { typeof(Pawn) }), //Creates the pawn tracker
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(CreateTracker)));
            harm.Patch(AccessTools.Method(typeof(Pawn_SkillTracker), nameof(Pawn_SkillTracker.ExposeData)), //Saves the pawn tracker
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(SaveTracker)));
            harm.Patch(AccessTools.Method(typeof(SkillRecord), nameof(SkillRecord.Learn)), // Tracks the Global XP a pawn earns
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(TrackGlobalXp)));
            harm.Patch(AccessTools.Method(typeof(ExpertiseDef), nameof(ExpertiseDef.CanApplyOn)), //Checks if a pawn has enough skill points to purchase an expertise
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(CheckSkillPoints)));
            harm.Patch(AccessTools.Method(typeof(ExpertiseTracker), nameof(ExpertiseTracker.AddExpertise)), //Subtracts a skill point upon selecting an expertise
                postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(SubtractSkillPoint)));
            if (ExpandedSkillsMod.Settings.EnableProgressionUI == true)
            {
                harm.Patch(AccessTools.Method(typeof(CharacterCardUtility), nameof(CharacterCardUtility.DrawCharacterCard)), //Adds the Skill Point Count and Progress to the pawn UI card
                  postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(SkillPointDisplayUI)));
            }
            if (ExpandedSkillsMod.Settings.EnableCSVExport == true)
            {
                harm.Patch(AccessTools.Method(typeof(GameDataSaveLoader), nameof(GameDataSaveLoader.SaveGame)), //Saves pawn info to a CSV file after save
                    postfix: new HarmonyMethod(typeof(SkillPointPatches), nameof(CSVAfterSave)));
            }
        }
        
        public static void CreateTracker(Pawn_SkillTracker __instance) //Creates the pawn tracker 
        {
            SkillPointTrackers.CreateTracker(__instance);
        }

        public static void SaveTracker(Pawn_SkillTracker __instance) //Saves the pawn tracker
        {
            SkillPointTrackers.SaveTracker(__instance);
        }

        public static void TrackGlobalXp(SkillRecord __instance, float xp) //Tracks how much XP a pawn earns on a global scale
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


            
            float milestoneRequirement;
            if (tracker.thresholdList == null || tracker.thresholdList.Count == 0)
            {
                milestoneRequirement = UnlockRequirement.Progression(ExpandedSkillsMod.Settings.SelectedProgression, tracker);
                tracker.thresholdList.Add(milestoneRequirement);
            }
            else if (ExpandedSkillsMod.Settings.SelectedProgression == ProgressionChoice.Custom)
            {
                milestoneRequirement = UnlockRequirement.Progression(ExpandedSkillsMod.Settings.SelectedProgression, tracker);    
            }
            else
            {
                //Log.Message($"SkillPointPatches tracker is NOT null, setting to LAST");
                milestoneRequirement = tracker.thresholdList.Last();
            }

            //Log.Message($"Milestone Requirement: {milestoneRequirement}, MilestoneID{milestoneId}");

            if (tracker.AwardPointForMilestone(milestoneRequirement, milestoneId))
            {
                Log.Message($"Awarded Skill Point to {__instance.Pawn.LabelShort}, Reached Milestone Requirement, Available Skill Points: {tracker.availableSkillPoints}");
                //Log.Message($"Awarded Point Milestone Requirement: {milestoneRequirement}, MilestoneID{milestoneId}");
                Messages.Message($"Pawns have available Skill Points", MessageTypeDefOf.PositiveEvent);
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
            //Log.Message($"Pawn: {__instance.Pawn.LabelShort}, {xp}, Total Tracked XP: {tracker.trackedGlobalXp},Milestone XP Requirement: {milestoneRequirement}, Progress: {tracker.trackedGlobalXp}/{milestoneRequirement}, Progression: {ExpandedSkillsMod.Settings.SelectedProgression}");

        }

        public static void CheckSkillPoints(Pawn pawn, ref string reason, ref bool __result) //Checks if a pawn has enough Skill Points to purchase an expertise
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
                reason = "Need a S.P";
                __result = false;
            }

        }

        public static void SubtractSkillPoint(ExpertiseTracker __instance) //Subtracts a skill point when purchasing an expertise
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

        public static void SkillPointDisplayUI(Rect rect, Pawn pawn) //Code for the display of Skill Points and Progress
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
         

            float milestoneRequirement;
            if (tracker.thresholdList == null || tracker.thresholdList.Count == 0)
            {

                milestoneRequirement = ExpandedSkillsMod.Settings.InitialGlobalStart;
            }
            else if (ExpandedSkillsMod.Settings.SelectedProgression == ProgressionChoice.Custom)
            {
                milestoneRequirement = UnlockRequirement.Progression(ExpandedSkillsMod.Settings.SelectedProgression, tracker);
            }
            else
            {
                milestoneRequirement = tracker.thresholdList.Last();
            }

            int milestoneProgress = ((int)(100 * ((tracker.trackedGlobalXp / milestoneRequirement))));
            if (milestoneProgress > 100)
            {
                milestoneProgress = 100;
            }

            TextAnchor anchor = Text.Anchor;
            GameFont font = Text.Font;
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;


            Rect skillPointNum = new Rect(rect.x + 320f, rect.y + 35f, 190f, 25f);
            Widgets.Label(skillPointNum, $"S.P: {tracker.availableSkillPoints} | Next: {milestoneProgress}%");

            Text.Anchor = anchor;
            Text.Font = font;
        }

        public static void CSVAfterSave()
        {
            CsvExporter.CSVExport(SkillPointTrackers.AllTrackers());
            Log.Message("Save detected, writing CSV file");
        }

        
    }
}
