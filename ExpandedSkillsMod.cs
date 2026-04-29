using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VanillaSkillsExpandedExpanded
{
    public class ExpandedSkillsMod : Mod
    {
        public static Harmony Harm;
        public static ExpandedSkillsSettings Settings;
        public static string ModFolderPath;
        
        public ExpandedSkillsMod(ModContentPack content) : base(content)
        {
            Harm = new Harmony("vanillaskillsexpandedexpanded");
            Settings = GetSettings<ExpandedSkillsSettings>();
            ModFolderPath = content.RootDir;
            try
            {
                SkillPointPatches.Do(Harm);

            }
            catch (Exception e) { Log.Error(e.ToString()); }


        }

        public override string SettingsCategory()
        {
            return "Vanilla Skills Expanded: Expanded";
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);

            var listing = new Listing_Standard();
            Rect rowRect = listing.GetRect(30f);
            Rect boxRight = rowRect.RightPart(0.3f);
            Rect textLeft = rowRect.LeftPart(0.7f);
            listing.Begin(inRect);
            listing.Label("Vanilla Skills Expanded: Expanded Config Page\n");
            listing.CheckboxLabeled("Enable Progression UI", ref Settings.EnableProgressionUI, "Turn the UI on or off in the player card");
            listing.CheckboxLabeled("Enable Mod CSV Export", ref Settings.EnableCSVExport, "Enable the CSV export that exports pawn data in the exports folder in the mod folder");
            listing.CheckboxLabeled("Enable Skill Point Available Widget", ref Settings.EnableAlert, "Turn the alert widget on or off");

            listing.Label("\nProgression Settings:\n");
            
            listing.TextFieldNumericLabeled(startingDescription.PadLeft(30), ref Settings.InitialGlobalStart, ref bufferStartingPoint);    
            if (listing.ButtonTextLabeled("Progression Choice", ExpandedSkillsMod.Settings.SelectedProgression.ToString()))
            {
                List<FloatMenuOption> progressionChoice = new List<FloatMenuOption>();
                foreach (ProgressionChoice choice in Enum.GetValues(typeof(ProgressionChoice)))
                {
                    progressionChoice.Add(new FloatMenuOption(choice.ToString(), () =>
                    {
                        ExpandedSkillsMod.Settings.SelectedProgression = choice;
                        ExpandedSkillsMod.Settings.Write();
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(progressionChoice.ToList()));
            }
            listing.TextFieldNumericLabeled<int>(linearDescription.PadLeft(30), ref Settings.LinearAddBy, ref bufferLinear);
            listing.TextFieldNumericLabeled<float>(multiplyDescription.PadLeft(30), ref Settings.CurvedMultiplyBy, ref bufferMultiply);
            listing.TextFieldNumericLabeled<float>(exponentialDescription.PadLeft(30), ref Settings.ExponentProgression, ref bufferExponential); ;
            listing.End();
        }
        
        private string bufferStartingPoint;
        private string bufferLinear;
        private string bufferMultiply;
        private string bufferExponential;
        private string startingDescription =    "Initial Global XP Starting Point:                      ";
        private string linearDescription =      "Linear Progression Add By:                           ";
        private string multiplyDescription =    "Curved Progression Multiply By:                    ";
        private string exponentialDescription = "Exponential Progression Power To Raise:     ";
        //Bugging the hell out of me, might create a custom method to shift text to left, box to right
        //Hard Coding is a temp solution


    }
}
