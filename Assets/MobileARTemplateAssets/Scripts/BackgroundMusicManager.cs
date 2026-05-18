using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource music;

    public void StopMusic()
    {
        if (music != null)
            music.Stop();
    }

    public void PlayMusic()
    {
        if (music != null && !music.isPlaying)
            music.Play();
    }
}