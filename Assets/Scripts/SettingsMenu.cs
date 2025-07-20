using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resDropdown;
    public TMP_Dropdown grphDropdown;
    public Toggle fullscreenToggle;
    public Resolution[] resolutions;

    private void Start()
    {
        int currentResolutionIndex = 0;

        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < resolutions.Length; i++)
        {
#if UNITY_ANDROID
            dropOptions.Add(new TMP_Dropdown.OptionData(resolutions[i].height + " x " + resolutions[i].width + " - " + resolutions[i].refreshRateRatio.value.ToString()));//.ToString()[..(resolutions[i].ToString().IndexOf('.') >= 0 ? resolutions[i].ToString().IndexOf('.') : 0]));
#else
            dropOptions.Add(new TMP_Dropdown.OptionData(resolutions[i].width + " x " + resolutions[i].height + " - " + resolutions[i].refreshRateRatio.value.ToString()[..(resolutions[i].refreshRateRatio.value.ToString().IndexOf('.') >= 0 ? resolutions[i].refreshRateRatio.value.ToString().IndexOf('.') : 0)]));//.ToString()[..(resolutions[i].ToString().IndexOf('.') >= 0 ? resolutions[i].ToString().IndexOf('.') : 0]));
#endif
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resDropdown.options = dropOptions;
        resDropdown.value = PlayerPrefs.HasKey("PrefRes") ? PlayerPrefs.GetInt("PrefRes") : currentResolutionIndex;
        Screen.fullScreen = PlayerPrefs.HasKey("PrefScreenMode") ? PlayerPrefs.GetInt("PrefScreenMode")==1 : Screen.fullScreen;
        //resDropdown.itemText.text = Screen.currentResolution.ToString();
        //resDropdown.GetComponent<Selectable>().interactable = false;
#if UNITY_ANDROID
            fullscreenToggle.interactable = false;
#endif
        grphDropdown.value = PlayerPrefs.HasKey("PrefQual") ? PlayerPrefs.GetInt("PrefQual") : QualitySettings.GetQualityLevel();
        audioMixer.SetFloat("Volume", PlayerPrefs.HasKey("PrefVolume") ? PlayerPrefs.GetFloat("PrefVolume") : 1);
        transform.Find("Background").Find("Volume Slider").GetComponent<Slider>().value = PlayerPrefs.HasKey("PrefVolume") ? PlayerPrefs.GetFloat("PrefVolume") : -10;
        audioMixer.SetFloat("MusicVolume", PlayerPrefs.HasKey("PrefMusic") ? PlayerPrefs.GetFloat("PrefMusic") : 1);
        transform.Find("Background").Find("Music Slider").GetComponent<Slider>().value = PlayerPrefs.HasKey("PrefMusic") ? PlayerPrefs.GetFloat("PrefMusic") : -45;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
        {
            bool isPaused = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>().isPaused;
            if (!isPaused && !GameObject.FindGameObjectWithTag("Player").GetComponent<Movements>().isWonOrLost) OnExit();
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume",volume);
        PlayerPrefs.SetFloat("PrefVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume",volume);
        PlayerPrefs.SetFloat("PrefMusic", volume);
    }
    
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("PrefQual", index);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("PrefScreenMode", isFullscreen ? 1 : 0);
    }

    public void SetResolution(int index)
    {
        //int width = Int16.Parse(data.ToString()[..data.ToString().IndexOf('x')]);
        //int height = Int16.Parse(data.ToString()[(data.ToString().IndexOf('x') + 1)..]);

        Screen.SetResolution(resolutions[index].width, resolutions[index].height,Screen.fullScreenMode, resolutions[index].refreshRateRatio);
        //PlayerPrefs.SetInt("PrefWidth", resolutions[index].width);
        //PlayerPrefs.SetInt("PrefHeight", resolutions[index].height);
        //PlayerPrefs.SetString("PrefRR", resolutions[index].refreshRateRatio.ToString());
        PlayerPrefs.SetInt("PrefRes", index);
    }
    public void OnExit()
    {
        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }
}
