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
        int GlobalXpRequirement = 270000;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref GlobalXpRequirement, "GlobalXpRequirement");      
        }
    }
}
