using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class BaseFriendly : MonoBehaviour, Change
{
    private int damage = 0;
    public int armsDumpMissiles = 2000;
    private float fuelDump = 400.0f;
    public CapsulePhysics cp;
    public InstantiateExplosion explosion;
    private bool active = true;
    public Rigidbody rb;
    private Vector3 rbtemp;
    public float scale;
    public Vector3 initialV;
    private static double gravity = 5.68e26d * 6.674e-11d / (10000.0d * 10000.0d * 10000.0d);
    public int asteroids = 0;
    // Use this for initialization
    private bool firstTime = true;
    public void HandleChange()
    {
        if (rb != null)
            if (GamePersistence.control.multiplyer > -0.1 && GamePersistence.control.multiplyer < 0.1)
            {
                rbtemp = new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z);
                rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else if (GamePersistence.control.multiplyer > 0.9 && GamePersistence.control.multiplyer < 1.1 && !firstTime)
            {
                if (rb.velocity.magnitude > -0.1f && rb.velocity.magnitude < 0.1f)
                    rb.velocity = rbtemp;
                else
                    rb.velocity *= 0.1f;
            }
            else if (GamePersistence.control.multiplyer > 9.9 && GamePersistence.control.multiplyer < 10.1)
            {
                rb.velocity *= 10.0f;
            }
        firstTime = false;
    }
    public void SetActive(bool active)
    {
        this.active = active;

    }
    void Start()
    {
        GamePersistence.control.Register(this);
        rb.velocity = initialV;
        rbtemp = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        transform.localScale = new Vector3(scale, scale, scale);
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            rb.AddForce(-rb.position.normalized * (float)(gravity * GamePersistence.control.multiplyer / ((double)rb.position.magnitude * (double)rb.position.magnitude)));
        }
    }
    public bool OnRearm()
    {
 
            Debug.Log("Landed on friendly base");
            int ammount = CapsulePhysics.numMissiles - cp.missileCount;
            ammount = armsDumpMissiles > ammount ? ammount : armsDumpMissiles;
            cp.missileCount += ammount;
            bool transfered = ammount > 0;
            armsDumpMissiles -= ammount;

        return transfered;
      
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
            damage += 20;
        }
        else if (col.gameObject.name.Contains("Asteroid(Clone)") || col.gameObject.name.Contains("Asteroid (1)(Clone)"))
        {
            col.gameObject.GetComponent<Prison>().TransferPrisoners();
        }
        if (damage >= 100)
        {
            GamePersistence.control.Remove(this);

            explosion.InstantiateExplo(transform.position, transform.rotation);
            Destroy(gameObject); 
        }
    }

}
