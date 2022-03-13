using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HiScore : MonoBehaviour {

    public Text scoreText;
    public InputField newname;
    public Button submitButton;

	// Use this for initialization
	void Start () {
        GamePersistence.control.Load();
        if (GamePersistence.control.IsHiScore())
        {
            scoreText.text = "High Score: " + GamePersistence.control.score;
            newname.enabled = true;
            submitButton.enabled = true;
        }
        else
        {
            scoreText.text = "Score: " + GamePersistence.control.score;
            newname.enabled = false;
            submitButton.enabled = false;


        }

    }

    public void SetName()
    {
        if (GamePersistence.control.IsHiScore())
        {
            GamePersistence.control.EnterName(newname.text);
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}
