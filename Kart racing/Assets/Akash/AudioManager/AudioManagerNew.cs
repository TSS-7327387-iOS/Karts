using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip clip;
    public AudioMixerGroup mixer;
	[Range(0f,1f)]
	public float volume = 0.7f;
	[Range(0f,1f)]
	public float pitch  = 1f;

	public bool loop = false;

	public AudioSource source;

    public void SetSource (AudioSource _source)
	{
		source = _source;
		source.clip = clip;
        source.outputAudioMixerGroup = mixer;
		source.loop = loop;
	}
	public void Play()
	{
		source.volume = volume;
		source.pitch = pitch;
		source.Play();
	}
	public void Stop()
	{
		source.Stop();
	}
}

public class AudioManagerNew : MonoBehaviour {
	public static AudioManagerNew instance;
    
	[SerializeField]
	Sound[] sounds;
	[SerializeField]
	Sound[] _randomSoundsOnhit;
	[SerializeField]
	Sound[] _relodingRandomSound;
	[SerializeField]
	Sound[] _onWinSounds;
	[SerializeField]
	Sound[] _onGameStartSounds;
	[SerializeField]
	Sound[] _randomBulletSound;
	[SerializeField]
	public AudioMixer mixer;
	void Awake()
	{
		if (instance !=null)
		{
			if (instance != this)
			{
				Destroy (this.gameObject);
			} 

		}
		  else
		    {
				instance = this;	
				DontDestroyOnLoad (this);
			}

	}

	void Start ()
	{
		mixer.SetFloat("Sfx", (PlayerPrefs.GetInt("Sfx", 3) * 25) - 80);
		mixer.SetFloat("Music", (PlayerPrefs.GetInt("Music", 3) * 25) - 80);
		for (int i = 0; i < sounds.Length; i++) 
		{
			GameObject _go = new GameObject ("Sound_" + i + "_" + sounds [i].name);
			_go.transform.SetParent (this.transform);
			sounds [i].SetSource (_go.AddComponent<AudioSource> ());
            

        }
        for (int i = 0; i < _randomSoundsOnhit.Length; i++)
        {
			GameObject _go = new GameObject("Sound1_" + i + "_" + _randomSoundsOnhit[i].name);
			_go.transform.SetParent(this.transform);
			_randomSoundsOnhit[i].SetSource(_go.AddComponent<AudioSource>());
		}
        for (int i = 0; i < _relodingRandomSound.Length; i++)
        {
			GameObject _go = new GameObject("Sound2_" + i + "_" + _relodingRandomSound[i].name);
			_go.transform.SetParent(this.transform);
			_relodingRandomSound[i].SetSource(_go.AddComponent<AudioSource>());
		}
		for (int i = 0; i < _onWinSounds.Length; i++)
		{
			GameObject _go = new GameObject("Sound2_" + i + "_" + _onWinSounds[i].name);
			_go.transform.SetParent(this.transform);
			_onWinSounds[i].SetSource(_go.AddComponent<AudioSource>());
		}
		for (int i = 0; i < _onGameStartSounds.Length; i++)
		{
			GameObject _go = new GameObject("Sound2_" + i + "_" + _onGameStartSounds[i].name);
			_go.transform.SetParent(this.transform);
			_onGameStartSounds[i].SetSource(_go.AddComponent<AudioSource>());
		}
		for (int i = 0; i < _randomBulletSound.Length; i++)
		{
			GameObject _go = new GameObject("Sound2_" + i + "_" + _randomBulletSound[i].name);
			_go.transform.SetParent(this.transform);
			_randomBulletSound[i].SetSource(_go.AddComponent<AudioSource>());
		}
	}
	public void PlaySound (string _name)
	{
		for (int i = 0; i < sounds.Length; i++) 
		{
			if (sounds [i].name == _name) 
			{
				sounds [i].Play();
				return;
			}
		}
	}
	public void PlayRandomSoundForKill()
    {
		int i = Random.Range(0, _randomSoundsOnhit.Length);
		_randomSoundsOnhit[i].Play();
    }
	public void PlayRandomWinSound()
    {
        int i = Random.Range(0, _onWinSounds.Length);
        _onWinSounds[i].Play();
    }
    public void PlayRandomGameStartSound()
    {
        int i = Random.Range(0, _onGameStartSounds.Length);
        _onGameStartSounds[i].Play();
    }
	public void PlayRandomBulletSound()
    {
        int i = Random.Range(0, _randomBulletSound.Length);
        _randomBulletSound[i].Play();
    }
	public void StopSound (string _name)
	{
		for (int i = 0; i < sounds.Length; i++) 
		{
			if (sounds [i].name == _name) 
			{
				sounds [i].Stop ();
				return;
			}
		}
	}

}
