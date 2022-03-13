using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class Helipad : MonoBehaviour {
    public int damage = 0;
    public CapsulePhysics cp;
    public InstantiateExplosion explosion;
    public string nextLevel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Capsule")
        {
            Debug.Log("Helipad collided with capsule");
            if (cp.moonoids == cp.totalMoonoids)
            {
                cp.score += 1000;
                cp.score += (int)(cp.fuel * 10.0f);
                cp.lives++;

                Application.LoadLevel(nextLevel);
            }
        }
        else if (col.gameObject.name == "Missile(Clone)")
        {
            Debug.Log("Helipad collided with Missile");
            damage += 20;
        }
        else if (col.gameObject.name == "Bomb(Clone)")
        {
            Debug.Log("Helipad collided with Bomb");
            damage += 100;
        }
        if (damage >= 100)
        {
            Debug.Log("Helipad destroyed");
            cp.score += 100;
            explosion.InstantiateExplo(transform.position, transform.rotation);

            Destroy(gameObject); 
        }

        Debug.Log(col.gameObject.name);

    }
}
