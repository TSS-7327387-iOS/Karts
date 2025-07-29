using System.Threading;
using MoreMountains.NiceVibrations;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AudioHandler : MonoBehaviour
{
    
    AudioSource _audioSFX;
    public AudioSource playerSFX, playerSFXloop, BGM;
    public AudioClip batteryPickup, coinPickup, levelPickup, buttonClick, doorOpen;
    public AudioClip danger, victory, achivement, lose, gameplayBGM, cutsceneBGM;
    public static AudioHandler Instance;
    public Toggle soundT, musicT, hapticsT;
    public AudioSource[] musicSource, soundSource;
    //public Slider qualitySlider;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _audioSFX = GetComponent<AudioSource>();

        // Load stored preferences
        soundT.isOn = PlayerPrefs.GetInt("Sound", 1) == 1;
        musicT.isOn = PlayerPrefs.GetInt("Music", 1) == 1;
        hapticsT.isOn = PlayerPrefs.GetInt("Haptics", 1) == 1;

        // Apply settings
        ApplySound();
        ApplyMusic();
        ApplyHaptics();
    }

    public void SetMusic()
    {
        PlayerPrefs.SetInt("Music", musicT.isOn ? 1 : 0);
        PlayerPrefs.Save();
        AudioHandler.Instance.ButtonClicked();
        ApplyMusic();
    }

    private void ApplyMusic()
    {
        foreach (AudioSource music in musicSource)
            music.volume = musicT.isOn ? 1 : 0;

         if(MainMenu.inst!=null) MainMenu.inst.SoundOff(musicT.isOn);
        if(GameController.instance!=null) GameController.instance.BGMOff(musicT.isOn);
       //  if (GameplayManager02.instance != null) GameplayManager02.instance.BGMOff(musicT.isOn);
    }

    public void SetSound()
    {
        PlayerPrefs.SetInt("Sound", soundT.isOn ? 1 : 0);
        PlayerPrefs.Save();
        AudioHandler.Instance.ButtonClicked();
        ApplySound();
    }

    private void ApplySound()
    {
        foreach (AudioSource sound in soundSource)
            sound.volume = soundT.isOn ? 1 : 0;
    }

    public void SetHeptics()
    {
        PlayerPrefs.SetInt("Haptics", hapticsT.isOn ? 1 : 0);
        PlayerPrefs.Save();
        AudioHandler.Instance.ButtonClicked();
        ApplyHaptics();
    }

    private void ApplyHaptics()
    {
        MMVibrationManager.SetHapticsActive(hapticsT.isOn);
        if (hapticsT.isOn)
        {
            MMVibrationManager.Haptic(HapticTypes.Selection);
        }
    }
    

    public void PlayCutsceneBGM()
    {
        BGM.clip = cutsceneBGM;
        BGM.Play();
    }

    public void PlayGameplayBGM()
    {
        BGM.clip = gameplayBGM;
        BGM.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        _audioSFX.clip = clip;
        _audioSFX.Play();
    }

    public void ButtonClicked()
    {
        PlaySFX(buttonClick);
    }
    
    public void dooorButtonClicked()
    {
        PlaySFX(doorOpen);
    }

    public void BatterySFX()
    {
        PlaySFX(batteryPickup);
    }

    public void CoinSFX()
    {
        PlaySFX(coinPickup);
    }

    public void LevelSFX()
    {
        PlaySFX(levelPickup);
    }

    public void PlayVicSFX()
    {
        playerSFX.clip = victory;
        playerSFX.Play();
    }

    public void PlayAchivementSFX()
    {
        playerSFX.clip = achivement;
        playerSFX.Play();
    }

    public void PlayLoseSFX()
    {
        playerSFX.clip = lose;
        playerSFX.Play();
    }

    public void PlayDangerSFX()
    {
        playerSFXloop.clip = danger;
        playerSFXloop.Play();
    }

    public void StopDangerSFX()
    {
        playerSFXloop.Stop();
    }

    // public void SetQuality()
    // {
    //     QualitySettings.SetQualityLevel((int)qualitySlider.value, true);
    // }
}
