using UnityEngine;
using System.Collections;

public class Gizmo : MonoBehaviour {
	public void OnDrawGizmos(){
		Gizmos.DrawWireSphere (transform.position, transform.localScale.x);
	}
}
