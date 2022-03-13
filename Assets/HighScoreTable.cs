using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour {
    public Text[] name;
    public Text[] score;
    // Use this for initialization
    void Start () {
	    for(int i = 0;i < 5; i++)
        {
            name[i].text = ""+(i+1)+". "+GamePersistence.control.hiscoretable[i].name;
            score[i].text = ""+GamePersistence.control.hiscoretable[i].hiscore;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
