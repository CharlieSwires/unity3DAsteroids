using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using UnityEngine.VideoModule.VideoPlayer;

public class ShipExplosionScript : MonoBehaviour {
    public AudioClip explosionSound;
    public AudioClip deathScore;
    AudioSource audioSrc;
    AudioSource scoreAudioSrc;
    public GameObject deathScoreObject;
    float time = 0.0f;

    public Image img;
	// Use this for initialization
	void Start () {
        audioSrc = GetComponent<AudioSource>();
        scoreAudioSrc = deathScoreObject.GetComponent<AudioSource>();
        scoreAudioSrc.mute = true;
        loadVideo(img);
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= 5.0f && scoreAudioSrc.mute == true)
        {
            scoreAudioSrc.PlayOneShot(deathScore, GamePersistence.control.scoreLevel);
            scoreAudioSrc.mute = false;
        }
    }
    private void loadVideo(Image img1)
    {
        audioSrc.PlayOneShot(explosionSound, GamePersistence.control.effectsLevel);
        audioSrc.mute = false;
        //((VideoPlayer)img1.GetComponent<MeshRenderer>().material.mainTexture).Play();

    }
}
