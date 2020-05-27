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

    private void Start()
    {
        if (!setup)
        {
            GetResolutions();
            GetSound();
            setup = true;
        }
    }
    private void OnDisable()
    {
        float masterVolume = 0;
        float musicVolume = 0;
        float sfxVolume = 0;

        audioMixer.GetFloat("masterVolume", out masterVolume);
        audioMixer.GetFloat("musicVolume", out musicVolume);
        audioMixer.GetFloat("sfxVolume", out sfxVolume);

        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);

    }

    private void GetResolutions()
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
    private void GetSound()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume");
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);

        volumeMasterSlider.value = masterVolume;
        volumeMusicSlider.value = musicVolume;
        volumeFXSlider.value = sfxVolume;

    }
    private void Awake()
    {
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
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }
    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

}
