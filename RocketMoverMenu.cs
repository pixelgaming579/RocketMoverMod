using System.Collections.Generic;
using UnityEngine;
using SFS.World;
using SFS.WorldBase;
using SFS.Input;
using SFS.UI;
using static SFS.Base;

namespace RocketMoverMod
{
    public class RocketMoverMenu : MonoBehaviour
    {
        Dictionary<string, Planet> planets = planetLoader.planets;
        List<string> planetNames = new List<string>();
        Vector2 scrollVec = Vector2.zero;
        private void Start()
        {
            foreach (string planetName in planets.Keys)
            {
                this.planetNames.Add(planetName);
            };
        }
        private static int menuWidth = 200;
        private int borderThickness = 10;
        int selectedPlanetIndex;
        string currentSubmenu = "";
        float RocketMoverX = 5f;
        float RocketMoverY = 5f;
        float RocketMoverR = 1f;
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
                GUI.Label(new Rect(borderThickness, 20, menuWidth - (borderThickness*2), 20), "Press \\ to hide this menu.");
                if (GUI.Button(new Rect(borderThickness, 40, menuWidth - (borderThickness*2), 20), "Move Rocket"))
                {
                    this.currentSubmenu = "Move Rocket";
                    MsgDrawer.main.Log("Time frozen whilst Rocket Mover menu is open");
                }
                if (GUI.Button(new Rect(borderThickness, 70, menuWidth - (borderThickness*2), 20), "Change Planet"))
                {
                    if (this.currentSubmenu == "Move Rocket")
                    {
                        WorldTime.main.SetState(1, true, false);
                        MsgDrawer.main.Log("Physics resumed");
                        Rocket playerRocket = PlayerController.main.player.Value as Rocket;
                        playerRocket.rb2d.constraints = RigidbodyConstraints2D.None;
                    }
                    this.currentSubmenu = "Change Planet";
                }
                if (GUI.Button(new Rect(borderThickness, 100, menuWidth - (borderThickness*2), 20), "Set Orbit"))
                {
                    if (this.currentSubmenu == "Move Rocket")
                    {
                        WorldTime.main.SetState(1, true, false);
                        MsgDrawer.main.Log("Physics resumed");
                        Rocket playerRocket = PlayerController.main.player.Value as Rocket;
                        playerRocket.rb2d.constraints = RigidbodyConstraints2D.None;
                    }
                    this.currentSubmenu = "Set Orbit";
                }
                GUI.DragWindow();
        }
        public void UpdateSubMenu(int windowID)
        {
            if (this.currentSubmenu == "Move Rocket")
            {
                GUI.Label(new Rect(borderThickness, 20, menuWidth - (borderThickness*2), 80), "Use your RCS controls to move and rotate the rocket. Close this menu to resume the rocket's physics.");

                RocketMoverX = GUI.HorizontalSlider(new Rect(borderThickness, 100, menuWidth - (borderThickness*2), 20), RocketMoverX, 0.1f, 25f);
                GUI.Box(new Rect(borderThickness, 100, menuWidth - (borderThickness*2), 20), new GUIContent("", "Horziontal speed"), GUIStyle.none);
                Rect rectx = new Rect(borderThickness, 80, menuWidth - (borderThickness*2), 20);

                RocketMoverY = GUI.HorizontalSlider(new Rect(borderThickness, 130, menuWidth - (borderThickness*2), 20), RocketMoverY, 0.1f, 15f);
                GUI.Box(new Rect(borderThickness, 130, menuWidth - (borderThickness*2), 20), new GUIContent("", "Vertical speed"), GUIStyle.none);
                Rect recty = new Rect(borderThickness, 110, menuWidth - (borderThickness*2), 20);

                RocketMoverR = GUI.HorizontalSlider(new Rect(borderThickness, 160, menuWidth - (borderThickness*2), 20), RocketMoverR, 0.1f, 5f);
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
                GUI.Label(new Rect(borderThickness, 20, menuWidth - (borderThickness*2), 50), "Select a planet from the list below, then press the \"Teleport to\" button to teleport.");
                if (GUI.Button(new Rect(borderThickness, 70, menuWidth - (borderThickness*2), 20), "Teleport to "+this.planetNames[selectedPlanetIndex]))
                {
                    ChangePlanet();
                }
                this.scrollVec = GUI.BeginScrollView(new Rect(borderThickness, 95, menuWidth - (borderThickness*2), 130), this.scrollVec, new Rect(0, 75, menuWidth - (borderThickness*2), planets.Count*15), false, true, GUIStyle.none, GUI.skin.verticalScrollbar);
                selectedPlanetIndex = GUI.SelectionGrid(new Rect(0, 75, menuWidth - (borderThickness*2) - 20, planets.Count*15), selectedPlanetIndex, this.planetNames.ToArray(), 2);
                GUILayout.EndScrollView();
                if (GUI.Button(new Rect(borderThickness, 230, menuWidth - (borderThickness*2), 20), "Close Change Planet Menu"))
                {
                    this.currentSubmenu = "";
                }
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
            WorldTime.main.SetState(0, true, false);
            Rocket playerRocket = PlayerController.main.player.Value as Rocket;
            if (playerRocket)
            {
                Location rocketLocation = playerRocket.location.Value;
                Vector2 transform = playerRocket.rb2d.position;
                playerRocket.rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                if (Input.GetKey(KeybindingsPC.keys.Move_Rocket_Using_RCS[0].key)) // Up direction.
                {
                    Quaternion preRot = playerRocket.rb2d.transform.localRotation;
                    playerRocket.rb2d.transform.localRotation = Quaternion.identity;
                    playerRocket.rb2d.transform.Translate(0, speedY, 0);
                    playerRocket.rb2d.transform.localRotation = preRot;
                }
                else if (Input.GetKey(KeybindingsPC.keys.Move_Rocket_Using_RCS[2].key)) // Down direction.
                {
                    Quaternion preRot = playerRocket.rb2d.transform.localRotation;
                    playerRocket.rb2d.transform.localRotation = Quaternion.identity;
                    playerRocket.rb2d.transform.Translate(0, -speedY, 0);
                    playerRocket.rb2d.transform.localRotation = preRot;
                }
                if (Input.GetKey(KeybindingsPC.keys.Move_Rocket_Using_RCS[1].key)) // Left direction.
                {
                    Quaternion preRot = playerRocket.rb2d.transform.localRotation;
                    playerRocket.rb2d.transform.localRotation = Quaternion.identity;
                    playerRocket.rb2d.transform.Translate(-speedX, 0, 0);
                    playerRocket.rb2d.transform.localRotation = preRot;
                }
                else if (Input.GetKey(KeybindingsPC.keys.Move_Rocket_Using_RCS[3].key)) // Right direction.
                {
                    Quaternion preRot = playerRocket.rb2d.transform.localRotation;
                    playerRocket.rb2d.transform.localRotation = Quaternion.identity;
                    playerRocket.rb2d.transform.Translate(speedX, 0, 0);
                    playerRocket.rb2d.transform.localRotation = preRot;
                }
                if (Input.GetKey(KeybindingsPC.keys.Turn_Rocket[0].key))
                {
                    playerRocket.rb2d.transform.Rotate(new Vector3(0, 0, speedR));
                }
                else if (Input.GetKey(KeybindingsPC.keys.Turn_Rocket[1].key))
                {
                    playerRocket.rb2d.transform.Rotate(new Vector3(0, 0, -speedR));
                } 
            }
        }
        public void ChangePlanet()
        {
            string planetName = planetNames[selectedPlanetIndex];
            Planet planet = planets[planetName];
            if (PlayerController.main.player.Value as Rocket)
            {
                SFS.Variables.Planet_Local planet_local = new SFS.Variables.Planet_Local();
                planet_local.Value = planet;
                WorldLocation newLocation = new WorldLocation();
                newLocation.planet = planet_local;
                SFS.Variables.Double2_Local pos = new SFS.Variables.Double2_Local();
                // if (!planet.data.hasTerrain)
                // {
                //     pos.Value = new Double2(0, planet.GetTerrainHeightAtAngle(0));
                // }
                // else
                // {
                //     pos.Value = new Double2(0, planet.GetTerrainHeightAtAngle(0));
                // }
                pos.Value = new Double2(0, planet.OrbitRadius);
                newLocation.position = pos;
                (PlayerController.main.player.Value as Rocket).physics.SetLocationAndState(newLocation.Value, (PlayerController.main.player.Value as Rocket).physics.PhysicsMode);
            }
        }
    }
}
