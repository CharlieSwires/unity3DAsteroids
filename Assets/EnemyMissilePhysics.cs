using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class EnemyMissilePhysics : MonoBehaviour {
    public Rigidbody rb;
	private Vector3 rbtemp;
    private float time = 0.0f;
	private static double gravity = 5.68e26d * 6.674e-11d / (10000.0d * 10000.0d * 10000.0d);
	private static double thrust =  10000d* gravity /(5823.2d*5823.2d);
    private bool active = false;
    public CapsulePhysics cp;
    public BaseEnemy be;
    private Vector3 initialPosn;
    public InstantiateExplosion explosion;
    public GameObject afterburn;
    private Object afterburnClone = null;
    public int instanceNumber = 0;
	private bool firstTime = true;


    //public Generator[] generators;
	public void HandleChange()
	{
		if (rb != null)
			if (GamePersistence.control.multiplyer > -0.1 && GamePersistence.control.multiplyer < 0.1)
		{
			rbtemp = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
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
		if (active == true)
		{
			GamePersistence.control.Register(this);
		}else
		{
			GamePersistence.control.Remove(this);
		}
		this.active = active;
		
	}
    // Use this for initialization
    void Start()
    {
        //rb.position = be.transform.position + (Vector3.up * 3.0f);
        initialPosn = rb.position;
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);

    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Destroy()
    {
        {
            explosion.InstantiateExplo(transform.position, transform.rotation);
            Destroy(gameObject);if(be!=null)be.clone[instanceNumber] = null; 
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //if (col.gameObject.name == "Plane_MeshPart0" || col.gameObject.name == "Plane_MeshPart1" || col.gameObject.name == "Plane_MeshPart2")
        //{
         //   Debug.Log("Enemy Missile crashed with plane");
            //if (active) Destroy(gameObject);if(be!=null)be.clone[instanceNumber] = null;
       // }
        //else 
        if (col.gameObject.name == "Capsule")
        {
            Debug.Log("Enemy Missile crashed with capsule");
            if (active)
            {
                explosion.InstantiateExplo(transform.position, transform.rotation);
                Destroy(gameObject);if(be!=null)be.clone[instanceNumber] = null; 
            }
        }

    }

    void FixedUpdate()
    {
        if (active)
        {
            time += Time.deltaTime;
            if (time < 10.0f)
            {
                if (afterburnClone != null)
                {
                    ((GameObject)afterburnClone).transform.position = transform.position;
                }
                if (afterburnClone == null) afterburnClone = Instantiate(afterburn, transform.position, Quaternion.LookRotation(-(cp.transform.position - initialPosn).normalized, Vector3.up));
                transform.rotation = Quaternion.LookRotation((cp.transform.position - initialPosn).normalized, Vector3.up);
                rb.AddForce((cp.transform.position - initialPosn).normalized * (float)thrust);
            }
            else
            {
                if (afterburnClone != null)
                {
                    Destroy(afterburnClone);
                    afterburnClone = null;
                }
            }

			rb.AddForce(-rb.position.normalized *(float)(gravity * GamePersistence.control.multiplyer / ((double)rb.position.magnitude * (double)rb.position.magnitude)));
			//            for (int i = 0; i < generators.Length; i++)
//            {
//                if (generators[i] != null)
//                {
//                    Vector3 direction = transform.position - generators[i].transform.position;
//                    if (!generators[i].generatorDead && direction.magnitude < 30.0f) rb.AddForce(direction.normalized * generators[i].antigravity / (direction.magnitude * direction.magnitude));
//                }
//            }
            if (time > 40f)
            {
                Debug.Log("Enemy Missle ran out of time");
                if (active)
                {
                    explosion.InstantiateExplo(transform.position, transform.rotation);
                    Destroy(gameObject);if(be!=null)be.clone[instanceNumber] = null; 
                }

            }
        }
        if (crossingBoundary())
        {
            Vector3 p = new Vector3(-17611.6f, -0.33f, 1015.0f);
            transform.position -= 1.9f * p;
        }
    }

    bool crossingBoundary()
    {
        Vector3 p = new Vector3(-17611.6f, -0.33f, 1015.0f);
        Vector3 diff = transform.position - p;
        return diff.magnitude >= 1000.0f;

    }
}
