using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    public static float AllVol = 1f;
    public static Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
    public static void PlaySnd(string dir, string name,Vector3 pos, float vol)
    {
        AudioClip clip = LoadClip(dir, name);
        if(clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos,vol*AllVol);
        }
        else
        {
            Debug.LogError("Clip is Missing" + name);
        }
    }
    public static AudioClip LoadClip(string dir,string name)
    {
        if (!audioDic.ContainsKey(name))
        {
            string dirMusic = dir + "/" + name;
            AudioClip clip = Resources.Load(dirMusic) as AudioClip;
            if(clip != null)
            {
                audioDic.Add(clip.name, clip);
            }
        }
        return audioDic[name];
    }
}
