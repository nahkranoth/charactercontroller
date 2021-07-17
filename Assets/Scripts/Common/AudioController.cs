using System.Collections.Generic;
using UnityEngine;


public class AudioController : MonoBehaviour
{
    public enum AudioClipName
    {
        PlayerHurt,
        PlayerSwing,
        ZombieHurt
    }
    
    public AudioClip playerHurt;
    public AudioClip playerSwing;
    public AudioClip zombieGrowl;
    public List<AudioSource> sources;
    private int cycleStep = 0;
    private Dictionary<AudioClipName, AudioClip> audiomap;
    
    private void Awake()
    {
        audiomap = new Dictionary<AudioClipName, AudioClip>()
        {
            {AudioClipName.PlayerHurt, playerHurt},
            {AudioClipName.PlayerSwing, playerSwing},
            {AudioClipName.ZombieHurt, zombieGrowl}
        };
        
        WorldGraph.Subscribe(this, typeof(AudioController));
    }

    public void PlaySound(AudioClipName name)
    {
        cycleStep = (cycleStep + 1) % sources.Count;
        sources[cycleStep].clip = audiomap[name];
        sources[cycleStep].Play();
    }
}
