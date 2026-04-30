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
        public int GlobalXpRequirement = 200000; //Fallback value that I used before I implimented the different settings
        public bool EnableProgressionUI = true;
        public bool EnableCSVExport = true;
        public bool EnableAlert = true;
        public float InitialGlobalStart = 200000;
        public int LinearAddBy = 50000;
        public float CurvedMultiplyBy = 1.5f;
        public ProgressionChoice SelectedProgression = ProgressionChoice.Custom;
        public float ExponentProgression = 1.01f; //BE EXTREMELY CAREFUL, WILL BALOON VERY QUICKLY keep around 1.01
        public int MaxExpertiseVSEE = 3;

        

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref GlobalXpRequirement, "GlobalXpRequirement");
            Scribe_Values.Look(ref EnableProgressionUI, "EnableProgressionUI");
            Scribe_Values.Look(ref EnableCSVExport, "EnableCSVExport");
            Scribe_Values.Look(ref EnableAlert, "EnableAlert");
            Scribe_Values.Look(ref InitialGlobalStart, "InitialGlobalStart");
            Scribe_Values.Look(ref LinearAddBy, "LinearAddBy");
            Scribe_Values.Look(ref CurvedMultiplyBy, "CurvedMultiplyBy");
            Scribe_Values.Look(ref SelectedProgression, "SelectedProgression");
            Scribe_Values.Look(ref ExponentProgression, "ExponentProgression");
            Scribe_Values.Look(ref MaxExpertiseVSEE, "MaxExpertiseVSEE");
        }
    }
}
