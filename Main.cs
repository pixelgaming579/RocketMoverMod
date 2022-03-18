using System;
// using System.Collections;
using UnityEngine;
using ModLoader;
// using HarmonyLib;
using SFS;
using SFS.WorldBase;

namespace RocketMoverMod
{
    public class Main : SFSMod
    {
        GameObject menu;
        bool menuVisible;
        public Main() : base("RocketMover", "RocketMoverMod", "pixel_gamer579", "v1.1.x", "v1.0", "Allows the player to move the current rocket to other planets/orbits/positions."){}
        private void WorldLoaded(object sender, EventArgs e)
        {
            menu = new GameObject("object");
            menu.AddComponent<RocketMoverMenu>();
            UnityEngine.Object.DontDestroyOnLoad(menu);
            menu.SetActive(true);
            menuVisible = menu.activeSelf;
        }
        private void HomeLoaded(object sender, EventArgs e)
        {
            if (menu != null)
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;
            }
        }
        private void BuildLoaded(object sender, EventArgs e)
        {
            if (menu != null)
            {
                UnityEngine.Object.Destroy(menu);
                menu = null;
            }
        }
        private void WorldUpdate(object sender, EventArgs e)
        {
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                menu.SetActive(!menu.activeSelf);
                menuVisible = menu.activeSelf;
            }
            if (VideoSettingsPC.main.uiOpacitySlider.value == 0)
            {
                menu.SetActive(false);
            }
            else
            {
                menu.SetActive(menuVisible);
            }
        }
        public override void load()
        {
            Helper.OnUpdateWorldScene += this.WorldUpdate;
            Helper.OnWorldSceneLoaded += this.WorldLoaded;
            Helper.OnHomeSceneLoaded += this.HomeLoaded;
            Helper.OnBuildSceneLoaded += this.BuildLoaded;
        }
        public override void unload()
        {
            throw new System.NotImplementedException();
        }
    }
}
