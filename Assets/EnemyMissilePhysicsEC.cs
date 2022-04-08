using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class EnemyMissilePhysicsEC : MonoBehaviour
{
    public Rigidbody rb;
	private Vector3 rbtemp;
    private float time = 0.0f;
    private float thrust = 10.0f*.9807f;
    private float gravity = -0.1622f;
    private bool active = false;
    public CapsulePhysics cp;
    public EnemyCraft be;
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
        initialPosn = rb.position + new Vector3(0.0f, 0.0f, 2.0f);
        rb.position = initialPosn;
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
                Destroy(gameObject); if (be != null) be.clone[instanceNumber] = null;
            }
        }
        else if (col.gameObject.name == "EnemyCraft")
        {
            Debug.Log("Enemy Missile crashed with Enemy Craft");
          
        } else
        {
            Debug.Log("Enemy Missile crashed into " + col.gameObject.name);
        }

    }
    private float accumulation_of_error;
    private float derivative_of_error;
    private float last_error;
    private float accumulation_of_error2;
    private float derivative_of_error2;
    private float last_error2;
    private float oldAngle1;
    private float oldAngle2;
    private float closeness;
    private bool doneOnce;

    void FixedUpdate()
    {
        if (!active)
        {
            accumulation_of_error = 0.0f;
            derivative_of_error = 0.0f;
            last_error = 0.0f;
            accumulation_of_error2 = 0.0f;
            derivative_of_error2 = 0.0f;
            last_error2 = 0.0f;
            closeness = 100000.0f;
            doneOnce = false;
        }
        else
        {
            time += Time.deltaTime;
            if (time < 40.0f)
            {
                if (afterburnClone != null)
                {
                    ((GameObject)afterburnClone).transform.position = transform.position;
                }
                if (afterburnClone == null) afterburnClone = Instantiate(afterburn, transform.position, Quaternion.LookRotation(-(cp.transform.position - initialPosn).normalized, Vector3.up));
                transform.rotation = Quaternion.LookRotation((cp.transform.position - transform.position).normalized, Vector3.up);
                float kp = -26.714428f, ki = 73.656044f, kd = 103.96094f;
                float Kp = kp;
                float Ki = ki;
                float Kd = kd;
                Vector3 diff = cp.transform.position - transform.position;
                float[] initialTheta = new float[2];
                if (!doneOnce)
                {
                    rb.velocity = thrust * Time.deltaTime * diff.normalized;
                    initialTheta[0] = Mathf.Atan2((diff.z), (diff.x));
                    initialTheta[1] = Mathf.Atan2((diff.y), (diff.x));
                }
                float angle1 = (float)(Mathf.Atan2((diff.z), (diff.x)));
                float angle2 = (float)(Mathf.Atan2((diff.y), (diff.x)));

                float error = angle1;
                float error2 = angle2;
                error = error - initialTheta[0];
                error2 = error2 - initialTheta[1];
       
                accumulation_of_error += error * Time.deltaTime;
                accumulation_of_error = (float)((accumulation_of_error < Mathf.PI / 8.0) ? (accumulation_of_error > -Mathf.PI / 8.0) ? accumulation_of_error : -Mathf.PI / 8.0 : Mathf.PI / 8.0);
                derivative_of_error = (error - last_error) / Time.deltaTime;
                last_error = error;
                float output = (error * Kp) + (accumulation_of_error * Ki) + (derivative_of_error * Kd);
                accumulation_of_error2 += error2 * Time.deltaTime;
                accumulation_of_error2 = (float)((accumulation_of_error2 < Mathf.PI / 8.0) ? (accumulation_of_error2 > -Mathf.PI / 8.0) ? accumulation_of_error2 : -Mathf.PI / 8.0 : Mathf.PI / 8.0);
                derivative_of_error2 = (error2 - last_error2) / Time.deltaTime;
                last_error2 = error2;
                float output2 = (error2 * Kp) + (accumulation_of_error2 * Ki) + (derivative_of_error2 * Kd);

                float pvx = thrust * (Mathf.Cos(output2 + initialTheta[1]) - Mathf.Sin(output2 + initialTheta[1])) * Time.deltaTime;
                float pvy = thrust * (Mathf.Sin(output2 + initialTheta[1]) + Mathf.Cos(output2 + initialTheta[1])) * Time.deltaTime;
                float pvx2 =thrust * (Mathf.Cos(output + initialTheta[0]) * pvx - Mathf.Sin(output + initialTheta[0])) * Time.deltaTime;
                float pvz = thrust * (Mathf.Sin(output + initialTheta[0]) * pvx + Mathf.Cos(output + initialTheta[0])) * Time.deltaTime;

                
                Vector3 missileDirection = new Vector3(pvx2, pvy, pvz);
                rb.velocity = rb.velocity + missileDirection;

                doneOnce = true;
                if (closeness > diff.magnitude || time < 4.0f)
                {
                    closeness = diff.magnitude;
                } else
                {
                    cp.rb.AddForce(diff.normalized*((2000f*.987f)  / (diff.magnitude * diff.magnitude)));
                    cp.damage += 100.0f * ((10f * .987f) / (diff.magnitude * diff.magnitude))*Time.deltaTime;
                    cp.playExplosion(100.0f * ((10f * .987f) / (diff.magnitude * diff.magnitude)) * Time.deltaTime);
                    if (afterburnClone != null)
                    {
                        Destroy(afterburnClone);
                        afterburnClone = null;
                    }
                    explosion.InstantiateExplo(transform.position, transform.rotation);
                    Destroy(gameObject); if (be != null) be.clone[instanceNumber] = null;

                }
            }
            else
            {
                if (afterburnClone != null)
                {
                    Destroy(afterburnClone);
                    afterburnClone = null;
                }
                Debug.Log("Enemy Missle ran out of time PM");
                if (active)
                {
                    explosion.InstantiateExplo(transform.position, transform.rotation);
                    Destroy(gameObject); if (be != null) be.clone[instanceNumber] = null;
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
           
        }


    }
}
