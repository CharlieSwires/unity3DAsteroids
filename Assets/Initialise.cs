using UnityEngine;
using System.Collections;

public class Initialise : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GamePersistence.control.Load();
        PlayMusic.Activate(false);
        PlayMusic.Activate(true);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
