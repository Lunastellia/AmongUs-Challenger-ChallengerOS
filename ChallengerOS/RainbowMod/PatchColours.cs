using HarmonyLib;
using UnhollowerBaseLib;
using static ChallengerMod.Set.Data;

namespace ChallengerOS.RainbowPlugin
{
    [HarmonyPatch]
    public static class CustomColorPatches
    {
        [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), new[] {
                typeof(StringNames),
                typeof(Il2CppReferenceArray<Il2CppSystem.Object>)
            })]
        private class ColorStringPatch
        {
            [HarmonyPriority(Priority.Last)]
            public static bool Prefix(ref string __result, [HarmonyArgument(0)] StringNames name)
            {
                if ((int)name >= 50000)
                {
                    if (ChallengerMod.Challenger.LangGameSet == 2f || (Playerlang == "French" && ChallengerMod.Challenger.LangGameSet == 0f))
                    {
                        
                        if ((int)name == 999976)
                        {
                            string text = "Sanguin";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999977)
                        {
                            string text = "Terre";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999978)
                        {
                            string text = "Cheddar";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999979)
                        {
                            string text = "Soleil";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999980)
                        {
                            string text = "Radian";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999981)
                        {
                            string text = "Feuille";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999982)
                        {
                            string text = "Marais";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999983)
                        {
                            string text = "Glace";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999984)
                        {
                            string text = "Lagon";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999985)
                        {
                            string text = "Océan";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999986)
                        {
                            string text = "Nuit";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999987)
                        {
                            string text = "Aube";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999988)
                        {
                            string text = "Bonbon";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999989)
                        {
                            string text = "Galaxie";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999990)
                        {
                            string text = "Neige";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999991)
                        {
                            string text = "Cendre";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999992)
                        {
                            string text = "Obscur";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999993)
                        {
                            string text = "Arc-En-Ciel";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999994)
                        {
                            string text = "Rubis";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999995)
                        {
                            string text = "Ambre";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999996)
                        {
                            string text = "Emmeraude";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999997)
                        {
                            string text = "Larimar";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999998)
                        {
                            string text = "Saphire";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999999)
                        {
                            string text = "Quartz";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        
                        if ((int)name == 999976)
                        {
                            string text = "Blooby";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999977)
                        {
                            string text = "Earth";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999978)
                        {
                            string text = "Cheddar";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999979)
                        {
                            string text = "Sun";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999980)
                        {
                            string text = "Radian";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999981)
                        {
                            string text = "Leef";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999982)
                        {
                            string text = "Swamp";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999983)
                        {
                            string text = "Ice";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999984)
                        {
                            string text = "Lagoon";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999985)
                        {
                            string text = "Ocean";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999986)
                        {
                            string text = "Night";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999987)
                        {
                            string text = "Dawn";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999988)
                        {
                            string text = "Candy";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999989)
                        {
                            string text = "Galaxy";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999990)
                        {
                            string text = "Snow";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999991)
                        {
                            string text = "Cender";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999992)
                        {
                            string text = "Dark";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999993)
                        {
                            string text = "Rainbow";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999994)
                        {
                            string text = "Ruby";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999995)
                        {
                            string text = "Amber";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999996)
                        {
                            string text = "Emerald";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999997)
                        {
                            string text = "Larimar";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999998)
                        {
                            string text = "Sapphir";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                        if ((int)name == 999999)
                        {
                            string text = "Quartz";
                            if (text != null)
                            {
                                __result = text;
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
        }
    }
}

