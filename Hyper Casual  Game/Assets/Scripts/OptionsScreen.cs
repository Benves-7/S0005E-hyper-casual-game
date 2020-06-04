using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Toggle muteToggle, fullscreenToggle;
    public Slider volumeMasterSlider;
    public Slider volumeMusicSlider;
    public Slider volumeFXSlider;

    public AudioMixer audioMixer;
    public Options options;

    int currentResolutionIndex = 0;
    public bool setup;
    public bool mute;

    private void Start()
    {
        if (!setup)
        {
            GetResolutions();
            GetSound();
            setup = true;
        }
    }

    public void GetResolutions()
    {
        var resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (!options.Contains(option))
            {
                options.Add(option);
            }

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = options.Count-1;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void GetSound()
    {
        float masterVolume = 0;
        float musicVolume = 0;
        float sfxVolume = 0;

        mute = PlayerPrefs.GetInt("mute") == 1;
        masterVolume = PlayerPrefs.GetFloat("masterVolume");
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

        volumeMasterSlider.value = masterVolume;
        volumeMusicSlider.value = musicVolume;
        volumeFXSlider.value = sfxVolume;

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);

        if (mute)
        {
            muteToggle.isOn = true;
        }
        else
        {
            muteToggle.isOn = false;
        }

    }

    private void Awake()
    {
        options = FindObjectOfType<Options>();
        resolutionDropdown.onValueChanged.AddListener(delegate { ChangeResolution(); });
        fullscreenToggle.onValueChanged.AddListener(delegate { FullscreenChange(); });
    }

    void ChangeResolution()
    {
        var res = resolutionDropdown.options[resolutionDropdown.value].text.Split(' ');
        Screen.SetResolution(int.Parse(res[0]), int.Parse(res[2]), fullscreenToggle.isOn);
        resolutionDropdown.RefreshShownValue();
    }

    void FullscreenChange()
    {
        var res = resolutionDropdown.options[resolutionDropdown.value].text.Split(' ');
        Screen.SetResolution(int.Parse(res[0]), int.Parse(res[2]), fullscreenToggle.isOn);
        resolutionDropdown.RefreshShownValue();
    }

    public void MuteChange(bool _mute)
    {
        mute = _mute;
        float volume = 0;
        volume = volumeMasterSlider.value;
        SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
        audioMixer.SetFloat("musicVolume", volume);
    }
    public void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);
        audioMixer.SetFloat("sfxVolume", volume);
    }
    public void SetMasterVolume(float volume)
    {
        if (mute)
        {
            PlayerPrefs.SetFloat("masterVolume", volume);
            audioMixer.SetFloat("masterVolume", -80);
        }
        else
        {
            PlayerPrefs.SetFloat("masterVolume", volume);
            audioMixer.SetFloat("masterVolume", volume);
        }
    }
}
