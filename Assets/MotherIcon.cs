using UnityEngine;
using System.Collections;

public class MotherIcon : MonoBehaviour {
	public Camera cp;
    public GameObject sc;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
		Vector3 temp = (sc.transform.position - cp.transform.position);
		transform.position = (Mathf.Log10(temp.magnitude)/Mathf.Log10(1.5f)) * temp.normalized + cp.transform.position;
        transform.localRotation = Quaternion.LookRotation(cp.transform.up, temp);
    }
}
		                             