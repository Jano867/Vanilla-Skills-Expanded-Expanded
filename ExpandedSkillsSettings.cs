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
        public int GlobalXpRequirement = 100;
        public bool EnableProgressionUI = true;
        public bool EnableCSVExport = true;
        public int AmountOfTimesYouCanProgress = 100;

        public int InitialGlobalStart = 2000;

        public int LinearAddBy = 2000;
        public float CurvedMultiplyBy = 1.5;
        public int ProgressionChoice = 1;

        public float LinearAdd;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref GlobalXpRequirement, "GlobalXpRequirement");      
        }
    }
}
