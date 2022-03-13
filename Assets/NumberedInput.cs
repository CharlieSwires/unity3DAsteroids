using UnityEngine;
using System.Collections;

public class NumberedInput : MonoBehaviour {
    public string[] screens;
    public LoadLevel ll;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            int index = KeyToInt();
            if (index != -1 && index < screens.Length)
            {
                ll.LoadLevelOnCall(screens[index]);
            }
        }


    }

    int KeyToInt()
    {
             if (Input.GetKeyDown(KeyCode.Alpha1))
                return 0;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                return 1;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                return 2;
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                return 3;
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                return 4;
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                return 5;
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                return 6;
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                return 7;
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                return 8;
            else if (Input.GetKeyDown(KeyCode.Alpha0))
                return 9;
            else
                return -1;
 
    }
}
