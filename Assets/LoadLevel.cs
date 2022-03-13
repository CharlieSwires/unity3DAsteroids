using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

public void LoadLevelOnCall (string sceneName){
        //SceneManager.LoadScene (sceneName); //For unity 5.4
        if (sceneName == "Quit") Application.Quit();
		else Application.LoadLevel (sceneName);
	}

}
