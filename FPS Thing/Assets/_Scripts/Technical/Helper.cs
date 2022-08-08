using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{


    public static void PlaySound(this AudioSource src, AudioClip clip, float volume = 1.0f, float startTime = -1.0f, float stereoPan = 0.0f)
    {
        if (clip == null)
            return;

        src.clip = clip;
        src.Play();
        if (startTime >= 0.0f)
            src.time = 0.0f;
        if (volume >= 0.0f)
            src.volume = volume;
    }


}
