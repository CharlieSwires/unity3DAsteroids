using UnityEngine;
using System.Collections;

public class ArtificialHorizonScript : MonoBehaviour {
    public CapsulePhysics cp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        {
			transform.rotation = Quaternion.LookRotation(cp.transform.position);

        }

    }
}
