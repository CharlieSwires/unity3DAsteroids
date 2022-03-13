using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {
    public Slider scoreLevel;
    public Slider effectsLevel;

	// Use this for initialization
	void Start () {
        GamePersistence.control.Load();
        scoreLevel.value = GamePersistence.control.scoreLevel;
        effectsLevel.value = GamePersistence.control.effectsLevel;
        PlayMusic.Activate(true);
    }

    public void Save()
    {
        GamePersistence.control.scoreLevel = scoreLevel.value;
        GamePersistence.control.effectsLevel = effectsLevel.value;
        GamePersistence.control.Save();
        PlayMusic.Activate(false);
        PlayMusic.Activate(true);

    }
    // Update is called once per frame
    void Update () {
	
	}
}
