using System.Collections.Generic;
using Verse;

namespace VanillaSkillsExpandedExpanded
{
    public class SkillPointTracker : IExposable
    {
        public float trackedGlobalXp;
        public int availableSkillPoints;
        public List<int> claimedMilestones=new List<int>();
        public List<string> claimedLevel15Skills = new List<string>();


        public void ExposeData()
        {
            Scribe_Values.Look(ref trackedGlobalXp, "trackedGlobalXp",0f);
            Scribe_Values.Look(ref availableSkillPoints, "availableSkillPoints",0);
            Scribe_Collections.Look(ref claimedMilestones, "claimedMilestones",LookMode.Value);
            Scribe_Collections.Look(ref claimedLevel15Skills, "claimedLevel15Skills", LookMode.Value);

            if (claimedLevel15Skills == null)
            {
                claimedLevel15Skills = new List<string>();
            }
            if (claimedMilestones == null)
            {
                claimedMilestones = new List<int>();
            }
        }

        public void AddGlobalXp(float xp)
        {
            if (xp <= 0f)
            {
                return;
            }
            trackedGlobalXp += xp;
        }
        
        public bool HasClaimedMilestone(int milestoneId)
        {
            return claimedMilestones.Contains(milestoneId);
        }

        public void ClaimMilestone(int milestoneId)
        {
            if (!claimedMilestones.Contains(milestoneId))
            {
                claimedMilestones.Add(milestoneId);
            }
        }
        
        public bool AwardPointForMilestone(float milestoneValue, int milestoneId)
        {
            if (HasClaimedMilestone(milestoneId))
            {
                return false;
            }
            if (trackedGlobalXp< milestoneValue)
            {
                return false;
            }

            availableSkillPoints++;
            ClaimMilestone(milestoneId);
            return true;
        }

        

        public bool HasClaimedLevel15Skill(string skillDefName)
        {
            return claimedLevel15Skills.Contains(skillDefName);
        }
        public void ClaimLevel15Skill(string skillDefName)
        {
            if (!claimedLevel15Skills.Contains(skillDefName))
            {
                claimedLevel15Skills.Add(skillDefName);
            }
        }
    }
}