using System.Collections.ObjectModel;
using R2API;
using RoR2;
using UnityEngine;
using TILER2;
using static R2API.RecalculateStatsAPI;

namespace Itemchest.Items
{
    public class HybridEngine : Item<HybridEngine>
    {
        [AutoConfigUpdateActions(AutoConfigUpdateActionTypes.InvalidateLanguage)]
        [AutoConfig("In percentage, amount of maximum HP granted per Utility item you possess, for first stack of the item. Default: .01 = 1%", AutoConfigFlags.PreventNetMismatch, 0f, float.MaxValue)]
        public float baseHPPercent { get; private set; } = .1f;

        [AutoConfigUpdateActions(AutoConfigUpdateActionTypes.InvalidateLanguage)]
        [AutoConfig("In percentage, amount of maximum HP granted per Utility item you possess, for additional stacks of item. Default: .01 = 1%", AutoConfigFlags.PreventNetMismatch, 0f, float.MaxValue)]
        public float stackHPPercent { get; private set; } = .05f;

        [AutoConfigUpdateActions(AutoConfigUpdateActionTypes.InvalidateLanguage)]
        [AutoConfig("In percentage, amount of attack speed granted per Utility item you possess, for first stack of the item. Default: .01 = 1%", AutoConfigFlags.PreventNetMismatch, 0f, float.MaxValue)]
        public float baseAttackSpeedPercent { get; private set; } = .1f;

        [AutoConfigUpdateActions(AutoConfigUpdateActionTypes.InvalidateLanguage)]
        [AutoConfig("In percentage, amount of attack speed granted per Utility item you possess, for additional stacks of item. Default: .01 = 1%", AutoConfigFlags.PreventNetMismatch, 0f, float.MaxValue)]
        public float stackAttackSpeedPercent { get; private set; } = .05f;

        public override string displayName => "Hybrid Engine";
        public override ItemTier itemTier => ItemTier.Tier1;
        public override ReadOnlyCollection<ItemTag> itemTags => new ReadOnlyCollection<ItemTag>(new[] { ItemTag.Utility });
        protected override string GetNameString(string langid = null) => displayName;
        protected override string GetPickupString(string langID = null) => "Gain base HP and attack speed";
        protected override string GetDescString(string langID = null) => $"Gives you <style=cIsUtility>{baseHPPercent * 100}%</style> <style=cStack>(+{stackHPPercent * 100}% per stack)</style> base health and <style=cIsUtility>{baseAttackSpeedPercent * 100}%</style> <style=cStack>(+{stackAttackSpeedPercent}% per stack)</style> attack speed";
        protected override string GetLoreString(string landID = null) => "An engine ripped from a vehicle in the near future...";

        public HybridEngine()
        {
            modelResource = Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery");
            iconResource = Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon");
        }
        public override void SetupAttributes()
        {
            displayRules = GenerateItemDisplayRules();
            base.SetupAttributes();
        }
        private static ItemDisplayRuleDict GenerateItemDisplayRules()
        {
            return new ItemDisplayRuleDict(null);
        }
        public override void Install()
        {
            base.Install();
            GetStatCoefficients += GainBonusHP;
        }
        public override void Uninstall()
        {
            base.Uninstall();
            GetStatCoefficients -= GainBonusHP;
        }
        private void GainBonusHP(CharacterBody sender, StatHookEventArgs args)
        {
            var inventoryCount = GetCount(sender);
            if (GetCount(sender) > 0)
            {
                args.healthMultAdd += baseHPPercent + ((inventoryCount - 1) * stackHPPercent);
                args.attackSpeedMultAdd += baseAttackSpeedPercent + ((inventoryCount - 1) * stackAttackSpeedPercent);
            }
        }
    }
}