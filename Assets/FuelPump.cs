using UnityEngine;
using System.Collections;


public class FuelPump : MonoBehaviour {
    public float fuelDump = 200.0f;
    // Use this for initialization
    public CapsulePhysics cp;
    private int damage = 0;
    public InstantiateExplosion explosion;
	void Start () {
	
	}

    public bool OnRefueling()
    {
        float ammount = 100.0f - cp.fuel;
        ammount = fuelDump > ammount ? ammount : fuelDump;
        bool fuelTransferred = ammount > 0.0f;
        cp.fuel += ammount;
        fuelDump -= ammount;
        return fuelTransferred;

    }
    void OnTriggerEnter(Collider col)
    {
 if (col.gameObject.name == "Missile(Clone)")
        {
            damage += 34;
        }
        else if (col.gameObject.name == "Bomb(Clone)")
        {
            damage += 100;
        }
        if (damage >= 100)
        {
            explosion.InstantiateExplo(transform.position, transform.rotation);

            Destroy(gameObject); 
        }
    }


    // Update is called once per frame
    void Update () {
	
	}
}
