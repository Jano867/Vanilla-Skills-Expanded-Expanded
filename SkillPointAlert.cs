using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VanillaSkillsExpandedExpanded
{
    public class SkillPointAlert : Alert
    {
        private List<Pawn> pawnsWithSkillPoints = new List<Pawn>();
        private IEnumerable<Pawn> PawnsWithSkillPoints
        {
            get
            {
                pawnsWithSkillPoints.Clear(); //Done to make sure that I don't have to do an extra check to see if a pawn has 0 or less skill points
                foreach(Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
                {
                    SkillPointTracker tracker = SkillPointTrackers.GetTracker(pawn);
                    if (tracker != null && tracker.availableSkillPoints > 0)
                    {
                        pawnsWithSkillPoints.Add(pawn);
                    }
                }
                return pawnsWithSkillPoints;
            } 
        }

        public override string GetLabel()
        {
            int count = PawnsWithSkillPoints.Count();
            if (count == 1)
                return "Pawn Has Available Skill Point";//.Translate();
            else if (count >= 2)
                return "Pawns Have Available Skill Points";//.Translate();
            else
                return null;
        }
        public override AlertReport GetReport()
        {
            return ExpandedSkillsMod.Settings.EnableAlert ? AlertReport.CulpritsAre(PawnsWithSkillPoints.ToList()) : AlertReport.Inactive;
        }

        public override TaggedString GetExplanation()
        {
            int count = pawnsWithSkillPoints.Count();
            if (count == 1)            
                return "Pawn Has A Skill Point: " + PawnsWithSkillPoints.Select(p => p.NameFullColored.Resolve()).ToLineList("  ");
            
            else if (count >= 2)
                return "Pawns With Skill Points: " + PawnsWithSkillPoints.Select(p => p.NameFullColored.Resolve()).ToLineList("  ");
            
            else return null;
        }
    }
}
