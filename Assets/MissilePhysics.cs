using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;
public class MissilePhysics : MonoBehaviour, Change
{
    public Rigidbody rb;
    private Vector3 rbtemp;
    private float time = 0.0f;
    private static double gravity = 5.68e26d * 6.674e-11d / (10000.0d * 10000.0d * 10000.0d);
    private static double thrust =  10000d* gravity /(5823.2d*5823.2d);
    public bool active = false;
    public CapsulePhysics cp;
    public InstantiateExplosion explosion;
    public GameObject afterburn;
    private Object afterburnClone = null;
    public int instanceNumber = 0;
    public AudioClip missileSound;
    AudioSource audioSrc;
	public MeshRenderer mr;
    private bool firstTime = true;

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
            audioSrc = GetComponent<AudioSource>();
            audioSrc.PlayOneShot(missileSound, GamePersistence.control.effectsLevel);
            audioSrc.mute = false;
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
        rb.isKinematic = false;
        rb.velocity = cp.GetComponent<Rigidbody>().velocity;
        rbtemp = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);

        mr = GetComponent<MeshRenderer> ();
		mr.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Destroy()
    {
        //explosion.InstantiateExplo(transform.position, transform.rotation);
        GamePersistence.control.Remove(this);

        Destroy(gameObject);if(cp!=null)cp.clone[instanceNumber] = null; 
    }

    void OnTriggerEnter(Collider col)
    {
    if (col.gameObject.name.Contains("BaseFriendly"))
        {
            Debug.Log("Missile crashed with BaseFriendly");
            if (active)
            {
                if (afterburnClone != null)
                {
                    Destroy(afterburnClone);
                    afterburnClone = null;
                }
                GamePersistence.control.Remove(this);

                explosion.InstantiateExplo(transform.position, transform.rotation);
                Destroy(gameObject);if(cp!=null)cp.clone[instanceNumber] = null; 
            }

        }
        else if (col.gameObject.name.Contains("BaseEnemy"))
        {
            Debug.Log("Missile crashed with BaseEnemy");
            if (active)
            {
                if (afterburnClone != null)
                {
                    Destroy(afterburnClone);
                    afterburnClone = null;
                }
                GamePersistence.control.Remove(this);

                explosion.InstantiateExplo(transform.position, transform.rotation);
                Destroy(gameObject); if (cp != null) cp.clone[instanceNumber] = null;
            }

        }
        else if (col.gameObject.name.Contains("Sphere(Clone)"))
        {
            Debug.Log("Missile hit asteroid");
            if (active)
            {
                if (afterburnClone != null)
                {
                    Destroy(afterburnClone);
                    afterburnClone = null;
                }
                GamePersistence.control.Remove(this);

                explosion.InstantiateExplo(transform.position, transform.rotation);
                Destroy(gameObject); if (cp != null) cp.clone[instanceNumber] = null;
            }

        }
        else if (col.gameObject.name.Contains("Fuel"))
        {
            Debug.Log("Missile crashed with Fuel");
            if (active)
            {
                if (afterburnClone != null)
                {
                    Destroy(afterburnClone);
                    afterburnClone = null;
                }
                GamePersistence.control.Remove(this);

                explosion.InstantiateExplo(transform.position, transform.rotation);
                Destroy(gameObject);if(cp!=null)cp.clone[instanceNumber] = null; 
            }

        }
        else if (col.gameObject.name.Contains("Helipad"))
        {
            Debug.Log("Missile crashed with Helipad");
            if (active)
            {
                if (afterburnClone != null)
                {
                    Destroy(afterburnClone);
                    afterburnClone = null;
                }
                GamePersistence.control.Remove(this);

                explosion.InstantiateExplo(transform.position, transform.rotation);
                Destroy(gameObject);if(cp!=null)cp.clone[instanceNumber] = null; 
            }

        }
    }

    void FixedUpdate()
    {
     
        if (active)
        {
            rb.WakeUp();
            //transform.localRotation = Quaternion.LookRotation(-transform.forward.normalized, Vector3.up); ;

            mr.enabled = true;

           time += Time.deltaTime;
           if (time < 1.0f)
            {
                if (afterburnClone != null)
                {
                    ((GameObject)afterburnClone).transform.position = transform.position;
                }
                if (afterburnClone == null) afterburnClone = Instantiate(afterburn, transform.position, Quaternion.LookRotation(transform.forward.normalized, Vector3.up));
                rb.AddForce(-transform.forward.normalized * (float)(thrust * GamePersistence.control.multiplyer));
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

            if (time > 20f)
            {
                if (active)
                {
                    if (afterburnClone != null)
                    {
                        Destroy(afterburnClone);
                        afterburnClone = null;
                    }
                    GamePersistence.control.Remove(this);

                    explosion.InstantiateExplo(transform.position, transform.rotation);
                    Destroy(gameObject);if(cp!=null)cp.clone[instanceNumber] = null; 
                }
            }
        }
        else
        { 
        
            rb.Sleep();
            rb.isKinematic = false;
            Vector3 temp = -cp.transform.up.normalized;
            temp = Quaternion.Euler(cp.transform.rotation.x, cp.transform.rotation.y, cp.transform.rotation.z) * temp;
            transform.localPosition = temp;

            transform.localRotation = Quaternion.LookRotation(-cp.transform.forward.normalized, Vector3.up);
        }
        if (crossingBoundary())
        {
            Vector3 p = new Vector3(-17611.6f, -0.33f, 1015.0f);
            Vector3 diff = transform.position - p;

            transform.position -= 1.9f * diff;
        }
    }

    bool crossingBoundary()
    {
        Vector3 p = new Vector3(-17611.6f, -0.33f, 1015.0f);
        Vector3 diff = transform.position - p;
        return diff.magnitude >= 300.0f;

    }
}
