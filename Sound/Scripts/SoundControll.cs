using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControll : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] clip;

    public void Play()
    {
        if (clip == null) return;
        int index = 0;

        foreach(var cl in clip)
        {
            cl.Stop();
        }

        if (clip.Length > 1)
        {
            index = Random.Range(0, clip.Length - 1);
        }
        clip[index].Play();
    }
    public void Stop()
    {
        foreach (var cl in clip)
            cl.Stop();
    }

}
