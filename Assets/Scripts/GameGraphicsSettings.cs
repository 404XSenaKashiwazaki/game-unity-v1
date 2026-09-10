using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class GameGraphicsSettings : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown shadowDropdown;
    public TMP_Dropdown aaDropdown;


    void Start()
    {
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", 0);
        shadowDropdown.value = PlayerPrefs.GetInt("Shadow", 1);
        aaDropdown.value = PlayerPrefs.GetInt("AA", 1);

        ApplyQuality();
        ApplyShadow();
        ApplyAA();
    }


    public void ApplyQuality()
    {
        #if UNITY_ANDROID
            int[] mobileLevels = { 0, 2, 3 }; // Mobile, Low, Medium
            QualitySettings.SetQualityLevel(mobileLevels[qualityDropdown.value], true);
        #else
            int[] pcLevels = { 2, 3, 4 }; // Low, Medium, High
            QualitySettings.SetQualityLevel(pcLevels[qualityDropdown.value], true);
        #endif

            PlayerPrefs.SetInt("Quality", qualityDropdown.value);
            PlayerPrefs.Save();
    }


    public void ApplyShadow()
    {
        if (shadowDropdown.value == 0)
            QualitySettings.shadows = ShadowQuality.Disable;
        else
            QualitySettings.shadows = ShadowQuality.All;

        PlayerPrefs.SetInt("Shadow", shadowDropdown.value);
        PlayerPrefs.Save();
    }



    public void ApplyAA()
    {
        switch (aaDropdown.value)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;

            case 1:
                QualitySettings.antiAliasing = 2;
                break;

            case 2:
                QualitySettings.antiAliasing = 4;
                break;
        }

        PlayerPrefs.SetInt("AA", aaDropdown.value);
        PlayerPrefs.Save();
    }

}