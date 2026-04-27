using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Grammar;
using HarmonyLib;



namespace VanillaSkillsExpandedExpanded
{
    public class ExpandedSkillsSettings : ModSettings
    {
        //Most of these are temporary until I get a config page going
        public int GlobalXpRequirement = 200000; //Fallback value that I used before I implimented the different settings
        public bool EnableProgressionUI = true;
        public bool EnableCSVExport = true;
        public int AmountOfTimesYouCanProgress = 100;
        public bool EnableAlert = true;

        public float InitialGlobalStart = 200000;

        public int LinearAddBy = 50000;
        public float CurvedMultiplyBy = 1.5f;
        public ProgressionChoice SelectedProgression = ProgressionChoice.Custom;
        public float ExponentProgression = 1.01f; //BE EXTREMELY CAREFUL, WILL BALOON VERY QUICKLY keep around 1.01

        

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref GlobalXpRequirement, "GlobalXpRequirement");      
        }
    }
}
