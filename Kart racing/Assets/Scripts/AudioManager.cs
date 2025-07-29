using MoreMountains.NiceVibrations;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;
    public AudioSource BGM, Popups, tictic;
    public AudioClip uiSound;
    // Start is called before the first frame update
    void Awake()
    {
        if (inst != null) Destroy(gameObject);
        else inst = this;
        BGM.volume = PlayerPrefs.GetFloat("Music", 1);
        Popups.volume = PlayerPrefs.GetFloat("Sound", 1);
        tictic.volume = PlayerPrefs.GetFloat("Sound", 1);
    }
    public void UITouched()
    {
        if (PlayerPrefs.GetInt("Viberation") == 1)
        {
            tictic.Play();
            MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
            print("dmhgd");
        }
    }
    public void PlayPopup(AudioClip clip, bool lo = false)
    {
        Popups.loop = lo;
        Popups.clip = clip;
        Popups.Play();
    }
    public void PausePopup()
    {
        Popups.Pause();
    }
    public void UnPausePopup()
    {
        Popups.UnPause();
    }
    public bool isLastSecondsSoundPlaying;
    public void LastSecondsAudio(bool val)
    {
        isLastSecondsSoundPlaying = val;
        if (isLastSecondsSoundPlaying)
        {
            tictic.Play();
            InvokeRepeating(nameof(RedTimerViberation),1,1);
        }
        else
        {
            tictic.Stop();
            tictic.loop = false;
            tictic.clip = uiSound;
            CancelInvoke();
        }
    }
    void RedTimerViberation()
    {
        if (PlayerPrefs.GetInt("Viberation") == 1)
            MMVibrationManager.Haptic(HapticTypes.SoftImpact, false);
    }
}
