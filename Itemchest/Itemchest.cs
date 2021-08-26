using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using TILER2;
using static TILER2.MiscUtil;
using Itemchest.Items;
using BepInEx.Configuration;

namespace Itemchest
{
    [BepInDependency(R2API.R2API.PluginGUID, R2API.R2API.PluginVersion)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(TILER2Plugin.ModGuid, TILER2Plugin.ModVer)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(LanguageAPI), nameof(ResourcesAPI), nameof(RecalculateStatsAPI))]

    public class Itemchest : BaseUnityPlugin
	{
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Itemchest";
        public const string PluginName = "Itemchest";
        public const string PluginVersion = "0.0.1";

        internal static FilingDictionary<CatalogBoilerplate> masterItemList = new FilingDictionary<CatalogBoilerplate>();

        internal static BepInEx.Logging.ManualLogSource _logger;

        // public static AssetBundle MainAssets;
        private static ConfigFile ConfigFile;

        public void Awake()
        {
            _logger = Logger;

            ConfigFile = new ConfigFile(System.IO.Path.Combine(Paths.ConfigPath, PluginGUID + ".cfg"), true);

            masterItemList = T2Module.InitAll<CatalogBoilerplate>(new T2Module.ModInfo
            {
                displayName = "Itemchest",
                longIdentifier = "ITEMCHEST",
                shortIdentifier = "ITMCHST",
                mainConfigFile = ConfigFile
            });

            T2Module.SetupAll_PluginAwake(masterItemList);
            T2Module.SetupAll_PluginStart(masterItemList);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;
                PickupDropletController.CreatePickupDroplet(masterItemList.Get<HybridEngine>().pickupIndex, transform.position, transform.forward * 20f);
                _logger.LogMessage("Should have dropped item");
            }
        }

        private void Start()
        {
            CatalogBoilerplate.ConsoleDump(Logger, masterItemList);
        }
    }
}
