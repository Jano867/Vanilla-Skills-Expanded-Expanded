using System;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaSkillsExpandedExpanded
{
    public class GlobalXpRequirement
    {
        public static void Progression(ProgressionChoice choice)
        {
            switch (choice)
            {
                case choice.Flat:
                    Console.WriteLine("Not Implemented");
                    break;
                case ProgressionChoice.Linear:
                    break;
                case ProgressionChoice.Curved:
                    break;
                case ProgressionChoice.Exponential:
                    break;
                case ProgressionChoice.Custom:
                    break;
            }
        }


        private static float GetFlatProgression(int milestoneId)
        {
            return ExpandedSkillsMod.Settings.GlobalXpRequirement;
        }

        private static float GetLinearProgression(int milestoneId)
        {
            //Need amount increased by
        }
        private static float GetCurvedProgression(int milestoneId)
        {

        }
        private static float GetExponentialProgression(int milestoneId)
        {

        }
        private static float GetCustomProgression(int milestoneId)
        {

        }
        public static List<float> CustomProgression = new List<float> { 270000f, }

        //Need to make sure to defensively program in it so they can switch progression type mid game.
    }
}
