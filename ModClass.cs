using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;

namespace NoBlur
{
    public class GlobalSettingsClass
    {
        public bool noBlur = true;
        public bool noFog = false;
    }

    public class NoBlur : Mod, IMenuMod, IGlobalSettings<GlobalSettingsClass>
    {
        public bool ToggleButtonInsideMenu => false;

        public static GlobalSettingsClass GS { get; set; } = new GlobalSettingsClass();
        public void OnLoadGlobal(GlobalSettingsClass s)
        {
            GS = s;
        }
        public GlobalSettingsClass OnSaveGlobal()
        {
            return GS;
        }

        new public string GetName() => "No Blur";
        public override string GetVersion() => "1.1";

        public override void Initialize()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            UpdateNoBlur();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) { UpdateNoBlur(); }

        public void UpdateNoBlur()
        {
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                GameObject gameObject = gameObjects[i];
                if (GS.noBlur) //If NoBlur is on
                {
                    if (gameObject.transform.parent == null)
                    {
                        //If the lowercas-ified name of gameObject contains 'blur'
                        if (gameObject.name.ToLower().Contains("blur"))
                        {
                            //Turn it off
                            gameObject.SetActive(false);
                        }
                    }
                }
                else //If NoBlur is off
                {
                    if (gameObject.transform.parent == null)
                    {
                        //If the lowercas-ified name of gameObject contains 'blur'
                        if (gameObject.name.ToLower().Contains("blur"))
                        {
                            //Turn it on
                            gameObject.SetActive(true);
                        }
                    }
                }

                if (GS.noFog) //If NoFog is on
                {
                    if (gameObject.transform.parent == null)
                    {
                        //If the lowercas-ified name of gameObject contains 'fog'
                        if (gameObject.name.ToLower().Contains("fog"))
                        {
                            //Turn it off
                            gameObject.SetActive(false);
                        }
                    }
                }
                else //If NoFog is off
                {
                    if (gameObject.transform.parent == null)
                    {
                        //If the lowercas-ified name of gameObject contains 'fog'
                        if (gameObject.name.ToLower().Contains("fog"))
                        {
                            //Turn it on
                            gameObject.SetActive(true);
                        }
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
                        UpdateNoBlur();
                    },
                    Loader = () => GS.noFog switch
                    {
                        false => 0,
                        true => 1,
                    }
                }
            };
        }
    }
}
