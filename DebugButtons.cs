using LudeonTK;
using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Verse;

namespace VanillaSkillsExpandedExpanded
{
    public static class DebugButtons
    {
        //Add Skill Point
        [DebugAction("VSE:E", "Add Skill Point", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void AddSkillPoint(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
                tracker.availableSkillPoints++;
                Messages.Message($"{p.LabelShort} Earned a Skill Point", MessageTypeDefOf.PositiveEvent);
                Log.Message($"Added Skill Point to {p.LabelShort}, current Skill Points: {tracker.availableSkillPoints}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }


        //Subtract Skill Point
        [DebugAction("VSE:E", "Subtract Skill Point", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void SubtractSkillPoint(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);

                
                tracker.availableSkillPoints--;
                if (tracker.availableSkillPoints <= 0)
                {
                    tracker.availableSkillPoints = 0; 
                    return;
                }
                Log.Message($"Subtracted Skill Point from {p.LabelShort}, current Skill Points: {tracker.availableSkillPoints}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }


        //Add 1,000 Global XP
        [DebugAction("VSE:E", "Add 1,000 Global XP", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void Add1000GlobalXp(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
                UnlockRequirement.Progression(ExpandedSkillsMod.Settings.SelectedProgression, tracker);
                if (tracker.thresholdList == null)
                {
                    tracker.thresholdList = new List<float>();
                }
                float milestoneRequirement;
                if (tracker.thresholdList.Count == 0 || tracker.thresholdList == null)
                {
                    milestoneRequirement = ExpandedSkillsMod.Settings.InitialGlobalStart;
                    tracker.thresholdList.Add(milestoneRequirement);
                }
                else if (ExpandedSkillsMod.Settings.SelectedProgression == ProgressionChoice.Custom)
                {
                    milestoneRequirement = UnlockRequirement.Progression(ExpandedSkillsMod.Settings.SelectedProgression, tracker);
                }
                else
                {
                    milestoneRequirement = tracker.thresholdList.Last();
                }

                tracker.trackedGlobalXp += 1000;
                if (tracker.AwardPointForMilestone(milestoneRequirement, tracker.ClaimedMilestoneCount())) //Awarding a point logic
                {
                    Log.Message($"Awarded Skill Point to {p.LabelShort}, Reached Milestone Requirement, Available Skill Points: {tracker.availableSkillPoints}");
                    Messages.Message($"{p.LabelShort} Earned a Skill Point", MessageTypeDefOf.PositiveEvent);
                }

                Log.Message($"Added 1000 global XP to {p.LabelShort}, current global tracked XP: {tracker.trackedGlobalXp}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }


        //Add 10,000 Global XP
        [DebugAction("VSE:E", "Add 10,000 Global XP", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void Add10000GlobalXp(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
                if (tracker.thresholdList == null)
                {
                    tracker.thresholdList = new List<float>();
                }
                float milestoneRequirement;
                if (tracker.thresholdList.Count == 0 || tracker.thresholdList == null)
                {
                    milestoneRequirement = ExpandedSkillsMod.Settings.InitialGlobalStart;
                    tracker.thresholdList.Add(milestoneRequirement);
                }
                else if (ExpandedSkillsMod.Settings.SelectedProgression == ProgressionChoice.Custom)
                {
                    milestoneRequirement = UnlockRequirement.Progression(ExpandedSkillsMod.Settings.SelectedProgression, tracker);
                }
                else
                {
                    milestoneRequirement = tracker.thresholdList.Last();
                }

                tracker.trackedGlobalXp += 10000;
                if (tracker.AwardPointForMilestone(milestoneRequirement, tracker.ClaimedMilestoneCount())) //Awarding a point logic
                {
                    Log.Message($"Awarded Skill Point to {p.LabelShort}, Reached Milestone Requirement, Available Skill Points: {tracker.availableSkillPoints}");
                    Messages.Message($"{p.LabelShort} Earned a Skill Point", MessageTypeDefOf.PositiveEvent);
                }
                Log.Message($"Added 1000 global XP to {p.LabelShort}, current global tracked XP: {tracker.trackedGlobalXp}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }

        //Add 1000 Historical XP
        [DebugAction("VSE:E", "Add 1,000 Historical XP", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void Add1000HistoricalXp(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
                
                tracker.historicalGlobalXp += 1000;
                Log.Message($"Added 1000 historical XP to {p.LabelShort}, current historical tracked XP: {tracker.historicalGlobalXp}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }


        //Add 10,000 Historical XP
        [DebugAction("VSE:E", "Add 10,000 Historical XP", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void Add10000HistoricalXp(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);

                tracker.historicalGlobalXp += 10000;
                Log.Message($"Added 10,000 historical XP to {p.LabelShort}, current historical tracked XP: {tracker.historicalGlobalXp}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }


        //Subtract 1,000 Global XP
        [DebugAction("VSE:E", "Subtract 1,000 Global XP", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void Subtract1000GlobalXp(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
                
                tracker.trackedGlobalXp -= 1000;
                if (tracker.trackedGlobalXp <= 0)
                {
                    tracker.trackedGlobalXp = 0;
                    return;
                }
                
                Log.Message($"Subtracted 1,000 global and historical XP to {p.LabelShort}, current global tracked XP: {tracker.trackedGlobalXp}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }


        //Subtract 10,000 Global XP
        [DebugAction("VSE:E", "Subtract 10,000 Global XP", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void Subtract10000GlobalXp(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);

                tracker.trackedGlobalXp -= 10000;
                if (tracker.trackedGlobalXp <= 0)
                {
                    tracker.trackedGlobalXp = 0;
                    return;
                }

                Log.Message($"Subtracted 10,000 global and historical XP to {p.LabelShort}, current global tracked XP: {tracker.trackedGlobalXp}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }


        //Subtract 1,000 Historical XP
        [DebugAction("VSE:E", "Subtract 1,000 Historical XP", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void Subtract1000HistoricalXp(Pawn p)
        {
            SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
            tracker.historicalGlobalXp -= 1000;
            if (tracker.historicalGlobalXp <= 0)
            {
                tracker.historicalGlobalXp = 0;
                return;
            }
            Log.Message($"Subtracted 1,000 historical XP from {p.LabelShort}, current historical tracked XP: {tracker.historicalGlobalXp} ");
            DebugActionsUtility.DustPuffFrom(p);
        }


        //Subtract 10,000 Historical XP
        [DebugAction("VSE:E", "Subtract 10,000 Historical XP", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void Subtract10000HistoricalXp(Pawn p)
        {
            SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
            tracker.historicalGlobalXp -= 10000;
            if (tracker.historicalGlobalXp <= 0)
            {
                tracker.historicalGlobalXp = 0;
                return;
            }
            Log.Message($"Subtracted 10,000 historical XP from {p.LabelShort}, current historical tracked XP: {tracker.historicalGlobalXp} ");
            DebugActionsUtility.DustPuffFrom(p);
        }

        //Pawn Claimed Milestone
        [DebugAction("VSE:E", "View Pawn Claimed Milestones", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ViewClaimedMilestones(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
                Log.Message($"{p.LabelShort}'s Claimed Milestones: {string.Join(" | ", tracker.claimedMilestones)}, Claimed Level 15 Skills: {string.Join(" | ", tracker.claimedLevel15Skills)}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }


        //Pawn Threshold List
        [DebugAction("VSE:E", "View Pawn Threshold List", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ViewPawnThresholdList(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
                Log.Message($"{p.LabelShort}'s Threshold List: {string.Join(" | ", tracker.thresholdList)}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }
        

        //Pawn Progression Progress
        [DebugAction("VSE:E", "View Pawn Progression", actionType = DebugActionType.ToolMapForPawns,
            allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ViewPawnProgressionProgress(Pawn p)
        {
            if (p != null)
            {
                SkillPointTracker tracker = SkillPointTrackers.GetTracker(p);
                if (tracker.thresholdList == null || tracker.thresholdList.Count == 0)
                    Log.Message($"{p.LabelShort}'s current progress: {tracker.trackedGlobalXp}/{ExpandedSkillsMod.Settings.InitialGlobalStart}");

                else
                    Log.Message($"{p.LabelShort}'s current progress: {tracker.trackedGlobalXp}/{tracker.thresholdList.Last()}");
                DebugActionsUtility.DustPuffFrom(p);
            }
        }
    }
}
