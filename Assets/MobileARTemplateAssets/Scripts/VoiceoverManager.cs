using UnityEngine;

public class VoiceoverManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource voiceoverSource;

    [Header("Voiceover Clips")]
    public AudioClip sportageVoiceover;
    public AudioClip hondaVoiceover;

    public void PlaySportageVoiceover()
    {
        if (voiceoverSource == null || sportageVoiceover == null) return;
        
        voiceoverSource.Stop();
        voiceoverSource.clip = sportageVoiceover;
        voiceoverSource.Play();
    }

    public void PlayHondaVoiceover()
    {
        if (voiceoverSource == null || hondaVoiceover == null) return;
        
        voiceoverSource.Stop();
        voiceoverSource.clip = hondaVoiceover;
        voiceoverSource.Play();
    }

    public void StopVoiceover()
    {
        if (voiceoverSource == null) return;
        voiceoverSource.Stop();
    }
}