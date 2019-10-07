using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixerGroup _Mixer;
    public Slider _Slider;

    private void Start()
    {
        if(_Mixer.name == "Master")
        {
            _Slider.value = PlayerPrefs.GetFloat("Volume");
        }
        else if(_Mixer.name == "Music")
        {
            _Slider.value = PlayerPrefs.GetFloat("VolumeMusic");
        }
        else if(_Mixer.name == "Sound FX")
        {
            _Slider.value = PlayerPrefs.GetFloat("VolumeSFX");
        }
    }

    public void SetLevel(float sliderValue)
    {
        _Mixer.audioMixer.SetFloat("Volume", sliderValue);
        PlayerPrefs.SetFloat("Volume", sliderValue);
    }
    public void SetSFXLevel(float sliderValue)
    {
        _Mixer.audioMixer.SetFloat("VolumeSFX", sliderValue);
        PlayerPrefs.SetFloat("VolumeSFX", sliderValue);
    }
    public void SetMusicLevel(float sliderValue)
    {
        _Mixer.audioMixer.SetFloat("VolumeMusic", sliderValue);
        PlayerPrefs.SetFloat("VolumeMusic", sliderValue);
    }
}
