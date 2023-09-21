using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using Trainworks.Builders;
using Trainworks.Interfaces;
using Trainworks.Managers;
using UnityEngine;

namespace BigOlBagOfMutators
{
    [BepInPlugin(MODGUID, MODNAME, VERSION)]
    [BepInProcess("MonsterTrain.exe")]
    [BepInProcess("MtLinkHandler.exe")]
    [BepInDependency("tools.modding.trainworks")]
    public class Plugin : BaseUnityPlugin, IInitializable
    {
        public const string MODGUID = "bag.of.mutators";
        public const string MODNAME = "BigOlBagOfMutators";
        public const string VERSION = "1.1";

        private void Awake()
        {
            var harmony = new Harmony(MODGUID);
            harmony.PatchAll();
        }

        public void Initialize()
        {
            // Discover all Mutator classes and build them.
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                var method = type.GetMethod("Make", BindingFlags.Static | BindingFlags.Public);
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
            // Discover all Challenge classes and build them.
            foreach (var type in assembly.GetTypes())
            {
                var method = type.GetMethod("MakeChallenge", BindingFlags.Static | BindingFlags.Public);
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }
    }
}
