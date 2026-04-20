using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VanillaSkillsExpandedExpanded
{
    public class CsvExporter
    {
        private static string GetExportPath()
        {
            string folder = Path.Combine(ExpandedSkillsMod.ModFolderPath, "Exports");
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, "PawnSkillPointData.txt");
        }
        private static string CreateCSV(Pawn pawn, SkillPointTracker tracker)
        {

            if (pawn==null || tracker == null)
            {
                return null;
            }
            int milestoneId = tracker.ClaimedMilestoneCount();
            float milestoneRequirement = ExpandedSkillsMod.Settings.GlobalXpRequirement + (200 * milestoneId);
            int milestoneProgress = ((int)(100 * (tracker.trackedGlobalXp / milestoneRequirement)));
            string claimedMilestones = string.Join("|", tracker.claimedMilestones);
            string claimedLevel15Skills = string.Join("|", tracker.claimedLevel15Skills);
            if (milestoneProgress > 100) //just in case 
            {
                milestoneProgress = 100;
            }
            string pawnName;
            if (pawn.Name is NameTriple nameTriple)
            {
                string first = nameTriple.First;
                string nickname = nameTriple.Nick;
                string last = nameTriple.Last;

                if (string.IsNullOrWhiteSpace(nickname))
                {
                    pawnName = $"{first} {last}";
                }
                else
                {
                    pawnName = $"{first} \"{nickname}\" {last}";
                }
                
            }
            else
            {
                pawnName = pawn.LabelShort;
            }

            return string.Join(",", $"\"{pawnName}\"", tracker.trackedGlobalXp, tracker.availableSkillPoints, $"\"{claimedMilestones}\"", $"\"{claimedLevel15Skills}\"", $"{milestoneProgress}%");
        }

        public static void CSVExport(Dictionary<Pawn, SkillPointTracker> cSVDictionary)
        {
            string path = GetExportPath();
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine("Pawn,TrackedGlobalXp,AvailableSkillPoints,ClaimedMilestones,ClaimedLevel15Skills,Progress");
                foreach (KeyValuePair<Pawn, SkillPointTracker> entry in cSVDictionary)
                {
                    Pawn pawn = entry.Key;
                    SkillPointTracker tracker = entry.Value;
                    if (pawn == null || tracker == null)
                    {
                        continue;
                    }
                    string line = CreateCSV(pawn, tracker);
                    writer.WriteLine(line);
                }

            }
        }
    }
}