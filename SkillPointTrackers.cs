using RimWorld;
using System;
using System.Collections.Generic;
using Verse; 

namespace VanillaSkillsExpandedExpanded
{
    public static class SkillPointTrackers
    {
        private static readonly Dictionary<Pawn_SkillTracker, SkillPointTracker> trackers = new Dictionary<Pawn_SkillTracker, SkillPointTracker>();
        

        public static SkillPointTracker GetTracker(Pawn pawn)
        {
            if (pawn == null || pawn.skills== null)
            {
                return null;
            }
            return GetTracker(pawn.skills);

        }

        public static SkillPointTracker GetTracker(Pawn_SkillTracker skills)
        {
            if (skills == null)
            {
                return null;
            }

            if (trackers.TryGetValue(skills,out SkillPointTracker tracker))  
            {
                return tracker;
            }
            return CreateTracker(skills);
        }

        public static SkillPointTracker CreateTracker(Pawn_SkillTracker skills)
        {
            if (skills == null)
            {
                return null;
            }
            var skillPoints = new SkillPointTracker();
            trackers[skills] = skillPoints;
            return skillPoints;
            
        }

        public static void SaveTracker(Pawn_SkillTracker skills)
        {
            if (skills == null)
            {
                return;
            }
            SkillPointTracker tracker = GetTracker(skills);
            Scribe_Deep.Look(ref tracker, "skillPointTracker");
            if (tracker == null)
            {
                tracker = new SkillPointTracker();
            }
            
            trackers[skills] = tracker;
        }
    }
}