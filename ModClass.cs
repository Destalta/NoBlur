﻿using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

namespace NoBlur
{
    public class GlobalSettingsClass
    {
        public bool noBlur = true;
        public bool noFog = false;
        public bool noHaze = false;
    }

    public class NoBlur : Mod, IMenuMod, IGlobalSettings<GlobalSettingsClass>
    {
        new public string GetName() => "No Blur";
        public override string GetVersion() => "3";

        public bool ToggleButtonInsideMenu => false;

        private List<GameObject> disabledBlurs = new List<GameObject>();
        private List<GameObject> disabledFogs = new List<GameObject>();
        private List<GameObject> disabledHazes = new List<GameObject>();

        public static GlobalSettingsClass GS { get; set; } = new GlobalSettingsClass();
        public void OnLoadGlobal(GlobalSettingsClass s)
        {
            GS = s;
        }
        public GlobalSettingsClass OnSaveGlobal()
        {
            return GS;
        }

        public override void Initialize()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            disabledBlurs.Clear();
            disabledFogs.Clear();
            disabledHazes.Clear();

            UpdateNoBlur();
            UpdateNoFog();
            UpdateNoHaze();
        }

        public void UpdateNoBlur()
        {
            //If NoBlur is on
            if (GS.noBlur) 
            {
                //Disable all blur planes in scene
                GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
                for (int i = 0; i < allGameObjects.Length; i++)
                {
                    GameObject gameObject = allGameObjects[i];
                    if (gameObject.transform.parent == null)
                    {
                        //If the lowercas-ified name of gameObject contains 'blurplane' and isn't "no blur"
                        if (gameObject.name.ToLower().Contains("blurplane") && gameObject.name.ToLower() != "no blur")
                        {
                            //Disable
                            disabledBlurs.Add(gameObject);
                            gameObject.SetActive(false);
                        }
                    }
                }
            }
            //If NoBlur is off
            else
            {
                //If blur planes were disabled this scene
                if (disabledBlurs.Count > 0)
                {
                    //Enable all blur planes in disabledBlurs
                    for (int b = 0; b < disabledBlurs.Count; b++) 
                    {
                        disabledBlurs[b].SetActive(true);
                    }
                    if (disabledBlurs.Count == 0)
                    {
                        //This shouldnt happen
                        Debug.LogError("Disabled Blurs is empty");
                    }
                }
            }
        }

        public void UpdateNoFog()
        {
            //If NoFog is on
            if (GS.noFog) 
            {
                //Disable all fog objects in scene
                GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
                for (int i = 0; i < allGameObjects.Length; i++)
                {
                    GameObject gameObject = allGameObjects[i];
                    //If the lowercas-ified name of gameObject contains... and isn't "no fog"
                    if (((gameObject.name.ToLower().Contains("fog") && !gameObject.name.ToLower().Contains("fog_canyon")) || gameObject.name.ToLower().Contains("fog_canyon_fog")) && gameObject.name.ToLower() != "no fog")
                    {
                        disabledFogs.Add(gameObject);
                        gameObject.SetActive(false);
                    }
                }
            }
            //If NoFog is off
            else
            {
                //If fog objects were disabled this scene
                if (disabledFogs.Count > 0)
                {
                    //Enable all fog objects in disabledFogs
                    for (int f = 0; f < disabledFogs.Count; f++) 
                    {
                        disabledFogs[f].SetActive(true);
                    }
                    if (disabledFogs.Count == 0)
                    {
                        //This shouldnt happen
                        Debug.LogError("Disabled Fogs is empty");
                    }
                }
                
            }
        }

        public void UpdateNoHaze()
        {
            //If NoHaze is on
            if (GS.noHaze)
            {
                //Disable all haze objects in scene
                GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
                for (int i = 0; i < allGameObjects.Length; i++)
                {
                    GameObject gameObject = allGameObjects[i];
                    //If the lowercas-ified name of gameObject contains 'haze' and isn't "no haze"
                    if (gameObject.name.ToLower().Contains("haze") && gameObject.name.ToLower() != "no haze")
                    {
                        disabledHazes.Add(gameObject);
                        gameObject.SetActive(false);
                    }
                }
            }
            //If NoHaze is off
            else
            {
                //If haze objects were disabled this scene
                if (disabledHazes.Count > 0)
                {
                    //Enable all haze objects in disabledHazes
                    for (int h = 0; h < disabledHazes.Count; h++)
                    {
                        disabledHazes[h].SetActive(true);
                    }
                    if (disabledHazes.Count == 0)
                    {
                        //This shouldnt happen
                        Debug.LogError("Disabled Hazes is empty");
                    }
                }
            }

        }

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return new List<IMenuMod.MenuEntry>
            {
                new IMenuMod.MenuEntry
                {
                    Name = "No Blur",
                    Description = "Removes all blur planes",
                    Values = new string[]
                    {
                        "Off",
                        "On"
                    },
                    Saver = opt =>
                    {
                        GS.noBlur = opt switch
                        {
                            0 => false,
                            1 => true,
                            _ => throw new System.NotImplementedException(),
                        };
                        UpdateNoBlur();
                    },
                    Loader = () => GS.noBlur switch
                    {
                        false => 0,
                        true => 1,
                    }
                },
                new IMenuMod.MenuEntry
                {
                    Name = "No Fog",
                    Description = "Removes all fog objects",
                    Values = new string[]
                    {
                        "Off",
                        "On"
                    },
                    Saver = opt =>
                    {
                        GS.noFog = opt switch
                        {
                            0 => false,
                            1 => true,
                            _ => throw new System.NotImplementedException(),
                        };
                        UpdateNoFog();
                    },
                    Loader = () => GS.noFog switch
                    {
                        false => 0,
                        true => 1,
                    }
                },
                new IMenuMod.MenuEntry
                {
                    Name = "No Haze",
                    Description = "Removes all haze objects",
                    Values = new string[]
                    {
                        "Off",
                        "On"
                    },
                    Saver = opt =>
                    {
                        GS.noHaze = opt switch
                        {
                            0 => false,
                            1 => true,
                            _ => throw new System.NotImplementedException(),
                        };
                        UpdateNoHaze();
                    },
                    Loader = () => GS.noHaze switch
                    {
                        false => 0,
                        true => 1,
                    }
                }
            };
        }
    }
}
