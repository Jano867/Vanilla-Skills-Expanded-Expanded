using HarmonyLib;
using System;
using Verse;

namespace VanillaSkillsExpandedExpanded
{
    public class ExpandedSkillsMod : Mod
    {
        public static Harmony Harm;
        public static ExpandedSkillsSettings Settings;
        

        public ExpandedSkillsMod(ModContentPack content) : base(content)
        {
            Harm = new Harmony("vanillaskillsexpandedexpanded");
            Settings = GetSettings<ExpandedSkillsSettings>();
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
    }
}
