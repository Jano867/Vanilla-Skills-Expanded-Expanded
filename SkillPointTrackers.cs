using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VanillaSkillsExpandedExpanded
{
    public static class SkillPointTrackers
    {
        public static readonly Dictionary<Pawn_SkillTracker, SkillPointTracker> trackers = new Dictionary<Pawn_SkillTracker, SkillPointTracker>();
        //private static readonly Dictionary<Pawn, SkillPointTracker> allTrackers = new Dictionary<Pawn, SkillPointTracker>();

        public static SkillPointTracker GetTracker(Pawn pawn)
        {
            if (pawn == null || pawn.skills == null)
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

            if (trackers.TryGetValue(skills, out SkillPointTracker tracker))
            {
                return tracker;
                //return pawnTrackers;
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
            //pawnTrackers[skills] = skillPoints;
            return skillPoints;

        }

        public static void SaveTracker(Pawn_SkillTracker skills)
        {
            if (skills == null)
            {
                return;
            }
            SkillPointTracker tracker = GetTracker(skills);
            //SkillPointTracker pawnTracker = GetTracker(skills);
            Scribe_Deep.Look(ref tracker, "skillPointTracker");
            if (tracker == null)
            {
                tracker = new SkillPointTracker();

            }

            trackers[skills] = tracker;
            //pawnTrackers[skills] = tracker;
        }


        public static Dictionary<Pawn, SkillPointTracker> AllTrackers() //Loops through and extracts the pawn from Pawn_SkillTracker
        { //Found out that LINQ is a good idea to compare and convert my other tracker to a Pawn tracker for CSV as I need the name and the other dictionary didn't have it
            var getTracker = from getPawn in trackers
                             let skills = getPawn.Key //Get the skills to compare
                             let tracker = getPawn.Value //Get the tracker for later
                             where skills != null && tracker != null //Make sure nothing is null
                             from pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead //Get all pawns
                             
                             where pawn != null && pawn.skills == skills //If pawn is not null and the pawns skills match the skills 
                             && pawn.IsColonist//Bunch of conditionals because IsColonist isn't enough to get rid of all the clutter
                             && pawn.Faction == Faction.OfPlayer
                             && !pawn.Dead
                             && !pawn.IsPrisoner //Don't care about prisoners but slaves are fine
                             && (pawn.Map !=null || pawn.IsCaravanMember())
                             select new //Create a new object containing the matching pawn and tracker
                             {
                                 Pawn = pawn, //Get pawn 
                                 Tracker = tracker //Get the tracker
                             };
            Dictionary<Pawn, SkillPointTracker> results = new Dictionary<Pawn, SkillPointTracker>();
            foreach(var entry in getTracker)
            {
                Pawn pawn = entry.Pawn;
                SkillPointTracker tracker = entry.Tracker;

                if (results.ContainsKey(pawn))
                {
                    continue;
                }
                else
                {
                    results[pawn] = tracker;
                }
                
            }
            return results;
        }
    }
}
