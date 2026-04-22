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
        public int GlobalXpRequirement = 100;
        public bool EnableProgressionUI = true;
        public bool EnableCSVExport = true;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref GlobalXpRequirement, "GlobalXpRequirement");      
        }
    }
}
