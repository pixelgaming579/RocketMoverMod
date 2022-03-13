using System;
using System.Collections;
using UnityEngine;
using ModLoader;
using HarmonyLib;
using SFS.World;
using SFS.Input;
using SFS.UI;
using SFS.Parts.Modules;

namespace RocketMoverMod
{
    public class RocketMoverMenu : MonoBehaviour
    {
        private static int menuWidth = 200;
        private int borderThickness = 10;
        string currentSubmenu = "";
        float RocketMoverX = 1f;
        float RocketMoverY = 1f;
        float RocketMoverR = 90f;
        public Rect menuGUI = new Rect(10, 50, menuWidth, 130);
        public Rect submenuGUI = new Rect(10, 190, menuWidth, 260);
        void OnGUI()
        {
            this.menuGUI = GUI.Window
            (
                GUIUtility.GetControlID(FocusType.Passive),
                this.menuGUI,
                new GUI.WindowFunction(this.UpdateMainMenu),
                "RocketMover Menu"
            );
            if (this.currentSubmenu != "")
            {
                this.submenuGUI = GUI.Window
                (
                    GUIUtility.GetControlID(FocusType.Passive),
                    this.submenuGUI,
                    new GUI.WindowFunction(this.UpdateSubMenu),
                    this.currentSubmenu
                );
            }
        }
        public void UpdateMainMenu(int windowID)
        {
                GUI.Label(new Rect(borderThickness, 20, menuWidth - (borderThickness*2), 20), "Press \\ to hide this menu");
                if (GUI.Button(new Rect(borderThickness, 40, menuWidth - (borderThickness*2), 20), "Move Rocket"))
                {
                    this.currentSubmenu = "Move Rocket";
                    MsgDrawer.main.Log("Time frozen whilst build menu is open");
                }
                if (GUI.Button(new Rect(borderThickness, 70, menuWidth - (borderThickness*2), 20), "Change Planet"))
                {
                    this.currentSubmenu = "Change Planet";
                }
                if (GUI.Button(new Rect(borderThickness, 100, menuWidth - (borderThickness*2), 20), "Set Orbit"))
                {
                    this.currentSubmenu = "Set Orbit";
                }
                GUI.DragWindow();
        }
        public void UpdateSubMenu(int windowID)
        {
            if (this.currentSubmenu == "Move Rocket")
            {
                GUI.Label(new Rect(borderThickness, 20, menuWidth - (borderThickness*2), 80), "Use your RCS controls to move and rotate the rocket. Close this menu to resume the rocket's physics.");

                RocketMoverX = GUI.HorizontalSlider(new Rect(borderThickness, 100, menuWidth - (borderThickness*2), 20), RocketMoverX, 0.1f, 100f);
                GUI.Box(new Rect(borderThickness, 100, menuWidth - (borderThickness*2), 20), new GUIContent("", "Horziontal speed"), GUIStyle.none);
                Rect rectx = new Rect(borderThickness, 80, menuWidth - (borderThickness*2), 20);

                RocketMoverY = GUI.HorizontalSlider(new Rect(borderThickness, 130, menuWidth - (borderThickness*2), 20), RocketMoverY, 0.1f, 50f);
                GUI.Box(new Rect(borderThickness, 130, menuWidth - (borderThickness*2), 20), new GUIContent("", "Vertical speed"), GUIStyle.none);
                Rect recty = new Rect(borderThickness, 110, menuWidth - (borderThickness*2), 20);

                RocketMoverR = GUI.HorizontalSlider(new Rect(borderThickness, 160, menuWidth - (borderThickness*2), 20), RocketMoverR, 0.1f, 90f);
                GUI.Box(new Rect(borderThickness, 160, menuWidth - (borderThickness*2), 20), new GUIContent("", "Rotation speed"), GUIStyle.none);
                Rect rectr = new Rect(borderThickness, 140, menuWidth - (borderThickness*2), 20);

                if (GUI.tooltip == "Horziontal speed")
                {
                    GUI.Label(rectx, GUI.tooltip);
                }
                else if (GUI.tooltip == "Vertical speed")
                {
                    GUI.Label(recty, GUI.tooltip);
                }
                else if (GUI.tooltip == "Rotation speed")
                {
                    GUI.Label(rectr, GUI.tooltip);
                }

                GUI.Label(new Rect(borderThickness, 170, menuWidth - (borderThickness*2), 20), "Horizontal speed: "+RocketMoverX);
                GUI.Label(new Rect(borderThickness, 190, menuWidth - (borderThickness*2), 20), "Vertical speed: "+RocketMoverY);
                GUI.Label(new Rect(borderThickness, 210, menuWidth - (borderThickness*2), 20), "Change of rotation: "+RocketMoverR) ;
                if (GUI.Button(new Rect(borderThickness, 230, menuWidth - (borderThickness*2), 20), "Close Move Rocket Menu"))
                {
                    WorldTime.main.SetState(1, true, false);
                    MsgDrawer.main.Log("Physics resumed");
                    Rocket playerRocket = PlayerController.main.player.Value as Rocket;
                    playerRocket.rb2d.constraints = RigidbodyConstraints2D.None;
                    this.currentSubmenu = "";
                }

            }
            else if (this.currentSubmenu == "Change Planet")
            {

            }
            else if (this.currentSubmenu == "Set Orbit")
            {
                
            }
            GUI.DragWindow();
        }
        private void Update()
        {
            if (this.currentSubmenu == "Move Rocket")
            {
                MoveRocket(RocketMoverX, RocketMoverY, RocketMoverR);
            }
        }
        public void MoveRocket(float speedX, float speedY, float speedR)
        {
            WorldTime.main.SetState(0, false, false);
            Rocket playerRocket = PlayerController.main.player.Value as Rocket;
            if (playerRocket)
            {
                // RcsModule[] rcsParts = playerRocket.partHolder.GetModules<RcsModule>();
                // foreach (RcsModule rcs in rcsParts)
                // {
                //     rcs.RCS_On = fals
                // }
                Location rocketLocation = playerRocket.location.Value;
                Vector2 transform = playerRocket.rb2d.position;
                playerRocket.rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                if (Input.GetKeyDown(KeybindingsPC.keys.Move_Rocket_Using_RCS[0].key)) // Up direction.
                {
                    // print("w");
                    Quaternion preRot = playerRocket.rb2d.transform.localRotation;
                    playerRocket.rb2d.transform.localRotation = Quaternion.identity;
                    playerRocket.rb2d.transform.Rotate(0, speedY, 0);
                    playerRocket.rb2d.transform.localRotation = preRot;
                }
                else if (Input.GetKeyDown(KeybindingsPC.keys.Move_Rocket_Using_RCS[1].key)) // Left direction.
                {
                    // print("a");
                    Quaternion preRot = playerRocket.rb2d.transform.localRotation;
                    playerRocket.rb2d.transform.localRotation = Quaternion.identity;
                    playerRocket.rb2d.transform.Translate(-speedX, 0, 0);
                    playerRocket.rb2d.transform.localRotation = preRot;
                }
                else if (Input.GetKeyDown(KeybindingsPC.keys.Move_Rocket_Using_RCS[2].key)) // Down direction.
                {
                    // print("s");
                    Quaternion preRot = playerRocket.rb2d.transform.localRotation;
                    playerRocket.rb2d.transform.localRotation = Quaternion.identity;
                    playerRocket.rb2d.transform.Rotate(0, -speedY, 0);
                    playerRocket.rb2d.transform.localRotation = preRot;
                }
                else if (Input.GetKeyDown(KeybindingsPC.keys.Move_Rocket_Using_RCS[3].key)) // Right direction.
                {
                    // print("d");
                    Quaternion preRot = playerRocket.rb2d.transform.localRotation;
                    playerRocket.rb2d.transform.localRotation = Quaternion.identity;
                    playerRocket.rb2d.transform.Translate(speedX, 0, 0);
                    playerRocket.rb2d.transform.localRotation = preRot;
                }
                if (Input.GetKeyDown(KeybindingsPC.keys.Turn_Rocket[0].key))
                {
                    playerRocket.rb2d.transform.Rotate(new Vector3(0, 0, -speedR));
                }
                else if (Input.GetKeyDown(KeybindingsPC.keys.Turn_Rocket[1].key))
                {
                    playerRocket.rb2d.transform.Rotate(new Vector3(0, 0, speedR));
                } 
            }
        }
    }
}
