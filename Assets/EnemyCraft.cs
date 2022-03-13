using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class EnemyCraft : MonoBehaviour {

    public Rigidbody rigidBodyReference;
    public static int numMissiles = 1000;
    private int simulMumMissiles = 10;
    public EnemyMissilePhysicsEC m;
    public EnemyMissilePhysicsEC[] clone;
    public int missileCount = numMissiles;
    public float doorOpenTime = 0.0f;
    public SphereCollider cc;
    private float damage = 0f;
    private Vector3 oldVelocity = new Vector3(0f, 0f, 0f);
    public GameObject afterburn;
    public InstantiateExplosion explosion;
    private Object afterburnClone = null;
    private float time = 0.0f;
    private bool active = false;
    private float thrust = 2f * 0.981f;
    public int instanceNumber = 0;
    public BaseEnemy be;
    public CapsulePhysics cp;
    Vector3 savedRb;
    Vector3 savedRbv;
    bool onJourney = false;
    private float pirateTime = 0.0f;
    // Use this for initialization
    void Start()
    {
        savedRb = transform.position;
        savedRbv = rigidBodyReference.velocity;
        clone = new EnemyMissilePhysicsEC[simulMumMissiles];
        for (int i = 0; i < simulMumMissiles; i++)
        {
            clone[i] = null;
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (damage >= 100.0f)
        {
            if (afterburnClone != null)
            {
                Destroy(afterburnClone);
                afterburnClone = null;
            }
            explosion.InstantiateExplo(transform.position, transform.rotation);
            transform.position = savedRb;
            rigidBodyReference.velocity = savedRbv;

        }
 
    }


    void OnTriggerEnter(Collider col)
    {
        if (onJourney && col.gameObject.name == "Missile(Clone)" && col.gameObject.GetComponent<MissilePhysics>().active)
        {
            Debug.Log("Pirate collided with " + col.gameObject.name);
            damage += 100.0f;
            cp.score += 2000;
            onJourney = false;
            pirateTime = 0.0f;

        }
        else if (col.gameObject.name == "Capsule")
        {
            Debug.Log("Pirate collided with " + col.gameObject.name);
            damage += 100.0f;
        }    
 
    }


    void FixedUpdate()
    {
        pirateTime += Time.deltaTime;

        if (active)
        {
            rigidBodyReference.rotation = Quaternion.LookRotation(Vector3.forward, -rigidBodyReference.velocity.normalized);

            time += Time.deltaTime;
            //       Vector3 currentVelocity = rigidBodyReference.velocity;
            //       if ((oldVelocity - currentVelocity).magnitude > (5.0f * .9807f * Time.deltaTime))
            //       {
            //           Debug.Log("EC You crashed too many Gs ");
            //           {
            //                   if (afterburnClone != null)
            //                   {
            //                       Destroy(afterburnClone);
            //                       afterburnClone = null;
            //                   }
            //                   explosion.InstantiateExplo(transform.position, transform.rotation);
            //                   Destroy(gameObject);if(be!=null)be.ecclone[instanceNumber] = null; 
            //                   //Hi score table
            //           }
            //
            //       }
            //        oldVelocity = currentVelocity;

            doorOpenTime += Time.deltaTime;

            rigidBodyReference.velocity = thrust * new Vector3(Random.value * Time.deltaTime, Random.value * Time.deltaTime, Random.value * Time.deltaTime);
            if (rigidBodyReference.position.x > 50f && rigidBodyReference.velocity.x > 0.0f)
            {
                rigidBodyReference.velocity = new Vector3(-rigidBodyReference.velocity.x, rigidBodyReference.velocity.y, rigidBodyReference.velocity.z);
            }
            if (rigidBodyReference.position.x < -50f && rigidBodyReference.velocity.x < 0.0f)
            {
                rigidBodyReference.velocity = new Vector3(-rigidBodyReference.velocity.x, rigidBodyReference.velocity.y, rigidBodyReference.velocity.z);
            }
            if (rigidBodyReference.position.y > 50f && rigidBodyReference.velocity.y > 0.0f)
            {
                rigidBodyReference.velocity = new Vector3(rigidBodyReference.velocity.x, -rigidBodyReference.velocity.y, rigidBodyReference.velocity.z);
            }
            if (rigidBodyReference.position.y < 0f && rigidBodyReference.velocity.y < 0.0f)
            {
                rigidBodyReference.velocity = new Vector3(rigidBodyReference.velocity.x, -rigidBodyReference.velocity.y, rigidBodyReference.velocity.z);
            }
            if (rigidBodyReference.position.z > 50f && rigidBodyReference.velocity.z > 0.0f)
            {
                rigidBodyReference.velocity = new Vector3(rigidBodyReference.velocity.x, rigidBodyReference.velocity.y, -rigidBodyReference.velocity.z);
            }
            if (rigidBodyReference.position.z < -50f && rigidBodyReference.velocity.z < 0.0f)
            {
                rigidBodyReference.velocity = new Vector3(rigidBodyReference.velocity.x, rigidBodyReference.velocity.y, -rigidBodyReference.velocity.z);
            }
            if (rigidBodyReference.velocity.magnitude > 0.0f)
            {
                if (afterburnClone != null)
                {
                    ((GameObject)afterburnClone).transform.position = transform.position;
                }
                if (afterburnClone == null) afterburnClone = Instantiate(afterburn, rigidBodyReference.velocity, Quaternion.LookRotation(new Vector3(1f, 1f, 1f), rigidBodyReference.velocity));
                //transform.rotation = Quaternion.LookRotation(-Vector3.up, Vector3.up);
            }
            else
            {
                if (afterburnClone != null)
                {
                    Destroy(afterburnClone);
                    afterburnClone = null;
                }
            }



            if (time > 10.0f)
            {
                if (missileCount > 0)
                {
                    --missileCount;

                    if (clone[missileCount % simulMumMissiles] != null)
                    {
                        clone[missileCount % simulMumMissiles].Destroy();
                        clone[missileCount % simulMumMissiles] = null;
                    }
                    cc.isTrigger = true;
                    doorOpenTime = 0.0f;
                    clone[missileCount % simulMumMissiles] = (EnemyMissilePhysicsEC)Instantiate(m, transform.position, transform.rotation);
                    clone[missileCount % simulMumMissiles].instanceNumber = missileCount % simulMumMissiles;
                    clone[missileCount % simulMumMissiles].SetActive(true);
                }
                time = 0f;

            }
            if (doorOpenTime < 4.0f)
            {
                cc.isTrigger = true;
            }
            else if (doorOpenTime > 4.0f)
            {
                cc.isTrigger = false;

            }
        }
        if (pirateTime > 20.0f && !onJourney)
        {
            Debug.Log("Here comes the pirate ");

            Vector3 p = new Vector3(-17611.6f, -0.33f + 300.0f, 1015.0f);
            transform.position = p;
            onJourney = true;
        }
        if (onJourney)
        {
            active = true;
            Vector3 p = new Vector3(-17611.6f, -0.33f, 1015.0f);
            Vector3 diff = transform.position - p;
            if (diff.magnitude > 300.0f)
            {
                transform.position = savedRb;
                rigidBodyReference.velocity = savedRbv;
                active = false;
                onJourney = false;
            }
            rigidBodyReference.velocity = new Vector3(0.0f, -3.0f, 0.0f);
        }
    }
    internal void SetActive(bool v)
    {
        active = v;
    }

  

    public void Destroy()
    {
        Destroy(gameObject);if(be!=null)be.ecclone[instanceNumber] = null;
    }
}

