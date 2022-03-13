using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayMusic : MonoBehaviour
{
    public AudioClip score;
    AudioSource audioSrc;
    public GameObject scoreObject;
    float time = 0.0f;
    bool active = true;
    static PlayMusic pm = null;

    // Use this for initialization
    void Awake()
    {
        pm = this;
        pm.audioSrc = scoreObject.GetComponent<AudioSource>();
        pm.audioSrc.mute = true;
    }
    public static void Activate(bool act)
    {
        if (act)
        { pm.active = true; }
        else
        {
            pm.active = false;
            pm.time = 0.0f;
            pm.audioSrc.mute = true;
            pm.audioSrc.Stop();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (pm.active)
        {
            pm.time += Time.deltaTime;
            if (pm.time >= 1.0f && pm.audioSrc.mute == true)
            {
                pm.audioSrc.PlayOneShot(score, GamePersistence.control.scoreLevel);
                pm.audioSrc.mute = false;
            }
        }

    }

}

