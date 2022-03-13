using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class Prison : MonoBehaviour , Change {
    public int size = 4;
    public int damage = 0;
    // Use this for initialization
    private int prisoners = 0;
    public CapsulePhysics cp;
    public BaseFriendly bf;
    public InstantiateExplosion explosion;
    public Prison[] clone;
    public Prison m;
    public Prison m2;
    private bool attachedToMother = false;
    private MeshRenderer mr;
    private bool active = false;
    public Rigidbody rb;
    private Vector3 rbtemp;
    public float scale;
    public Vector3 initialV;
    private static double gravity = 5.68e26d * 6.674e-11d / (10000.0d * 10000.0d * 10000.0d);
    public Vector3 savedVel;
    private bool attached = false;
	public float rv;
	public float spacex, spacey, spacez;
    private bool firstTime = true;
    public Rigidbody pirate;

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
        this.active = active;

    }
    // Use this for initialization
    void Start()
    {
        GamePersistence.control.Register(this);
        rbtemp = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        if (this.size == 1)
        {
            
            m.SetActive(false);
            m2.SetActive(true);
            m2.size = this.size;
			m2.rb.velocity = m.rb.velocity = savedVel;// + new Vector3((Random.value - 0.5f) * scale / size, (Random.value - 0.5f) * scale / size, (Random.value - 0.5f) * scale / size);
            m2.rb.angularVelocity = m.rb.angularVelocity = new Vector3((Random.value - 0.5f) * rv / size, (Random.value - 0.5f) * rv / size, (Random.value - 0.5f) * rv / size);
            m2.transform.localScale = m.transform.localScale = transform.localScale / 2;
            m.prisoners = 0;
            m2.prisoners = 1;
            cp.totalMoonoids += m2.prisoners;

        }
        m.mr = m.GetComponent<MeshRenderer>();
        m.mr.enabled = false;
        m2.mr = m2.GetComponent<MeshRenderer>();
        m2.mr.enabled = false;
        if (size == 4)
        {
            
            m.SetActive(true);
            m2.SetActive(false);
            m2.rb.velocity=rb.velocity = initialV;
            m.prisoners = 1;
            m2.prisoners = 0;

        }
        else if (this.size > 1 && this.size < 4)
        {
            
            m.SetActive(true);
            m2.SetActive(false);
			m2.rb.velocity=m.rb.velocity = savedVel;// + new Vector3((Random.value - 0.5f) * scale / size, (Random.value - 0.5f) * scale / size, (Random.value - 0.5f) * scale / size);
            m2.rb.angularVelocity = m.rb.angularVelocity = new Vector3((Random.value - 0.5f) * rv / size, (Random.value - 0.5f) * rv / size, (Random.value - 0.5f) * rv / size);
            m2.transform.localScale = m.transform.localScale = transform.localScale / 2;
            m.prisoners = 1;
            m2.prisoners = 0;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        m.rb.angularVelocity = m2.rb.angularVelocity = rb.angularVelocity;
        if (m.active && !m.attached && !m.attachedToMother)
        {
            m.mr.enabled = true;
            m.rb.AddForce(-rb.position.normalized * (float)(gravity * GamePersistence.control.multiplyer / ((double)rb.position.magnitude * (double)rb.position.magnitude)));
            m2.rb.AddForce(-rb.position.normalized * (float)(gravity * GamePersistence.control.multiplyer / ((double)rb.position.magnitude * (double)rb.position.magnitude)));
            m.savedVel = rb.velocity;
            m2.savedVel = savedVel;
            m2.transform.position = m.transform.position;
            m2.mr.enabled = !m.mr.enabled;

        }
        else if (m.active && m.attached && !m.attachedToMother)
        {
            m.mr.enabled = true;
            m.rb.velocity = cp.rb.velocity;
            m2.rb.velocity = m.rb.velocity;
			m2.rb.transform.position = m.rb.transform.position = cp.rb.transform.position - 2.0f * cp.rb.transform.forward;

        }
        else if (m.active && m.attachedToMother)
        {
            m.mr.enabled = true;
            m.rb.velocity = bf.rb.velocity;
            m2.rb.velocity = m.rb.velocity;

        }
        if (m2.active && !m2.attached && !m2.attachedToMother)
        {
            m2.mr.enabled = true;
            m2.rb.AddForce(-rb.position.normalized * (float)(gravity / ((double)rb.position.magnitude * (double)rb.position.magnitude)));
            m.rb.AddForce(-rb.position.normalized * (float)(gravity / ((double)rb.position.magnitude * (double)rb.position.magnitude)));
            m2.savedVel = rb.velocity;
            m.savedVel = m2.savedVel;
            m.transform.position = m2.transform.position;
            m2.mr.enabled = !m.mr.enabled;

        }
        else if (m2.active && m2.attached && !m2.attachedToMother)
        {
            m2.mr.enabled = true;
            m2.rb.velocity = cp.rb.velocity;
            m.rb.velocity = m2.rb.velocity;
			m2.rb.transform.position = m.rb.transform.position = cp.rb.transform.position - 2.0f * cp.rb.transform.forward;

        }
        else if (m2.active && m2.attachedToMother)
        {
            m2.mr.enabled = true;
            m.rb.velocity = bf.rb.velocity;
            m2.rb.velocity = m.rb.velocity;

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
    public bool RemovePrisoners()
    {
        Debug.Log("Prison collided with capsule");
        if (size == 1)
        {
            cp.moonoids += m2.prisoners;
            cp.asteroidsInPlay -= cp.moonoids;

            //cp.totalMoonoids -= cp.moonoids;
            bool moonoidsTransferred = m2.prisoners > 0;
            cp.score += m2.prisoners * 200;
            m2.prisoners = 0;
            m2.rb.velocity = m.rb.velocity = cp.rb.velocity;
			m2.rb.transform.position = m.rb.transform.position = cp.rb.transform.position - 2.0f * cp.rb.transform.forward;
            attached = true;
            return moonoidsTransferred;
        }
        else return false;
    }
    public bool TransferPrisoners()
    {
        Debug.Log("Prison collided with Base Friendly");
        if (size == 1)
        {
            bf.asteroids += cp.moonoids;
            bool moonoidsTransferred = cp.moonoids > 0;
            cp.score += cp.moonoids * 500;
            cp.moonoids = 0;
            m2.rb.velocity = m.rb.velocity = bf.rb.velocity;
            attached = false;
            attachedToMother = true;
            return moonoidsTransferred;
        }
        else return false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (active && col.gameObject.name == "Missile(Clone)" && col.gameObject.GetComponent<MissilePhysics>().active)
        {
            Debug.Log("Prison collided with Missile");
            Destroy(col.gameObject);
            if (size > 1)
            {
                cp.score += 100;
                clone = new Prison[8];
                this.size--;
				m.size =m2.size = this.size;
                if (this.size == 1)
                {

               
					clone[0] = (Prison)Instantiate(m2, rb.transform.position+new Vector3(spacex,spacey,spacez)*size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[0].savedVel = this.savedVel+ new Vector3((Random.value) * clone[0].scale / clone[0].size, (Random.value) * clone[0].scale / clone[0].size, (Random.value) * clone[0].scale / clone[0].size);
					clone[0].size = this.size;
					clone[0].damage = 0;
					clone[0].SetActive(true);
					clone[1] = (Prison)Instantiate(m2, rb.transform.position + new Vector3(-spacex, spacey, spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[1].savedVel = this.savedVel+ new Vector3((-Random.value) * clone[1].scale / clone[1].size, (Random.value) * clone[1].scale / clone[1].size, (Random.value) * clone[1].scale / clone[1].size);
					clone[1].size = this.size;
					clone[1].damage = 0;
					clone[1].SetActive(true);
					clone[2] = (Prison)Instantiate(m2, rb.transform.position + new Vector3(spacex, -spacey, spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[2].savedVel = this.savedVel+ new Vector3((Random.value) * clone[2].scale / clone[2].size, (-Random.value) * clone[2].scale / clone[2].size, (Random.value) * clone[2].scale / clone[2].size);
					clone[2].size = this.size;
					clone[2].damage = 0;
					clone[2].SetActive(true);
					clone[3] = (Prison)Instantiate(m2, rb.transform.position + new Vector3(-spacex, -spacey, spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[3].savedVel = this.savedVel+ new Vector3((-Random.value) * clone[3].scale / clone[3].size, (-Random.value) * clone[3].scale /  clone[3].size, (Random.value) *  clone[3].scale /  clone[3].size);
					clone[3].size = this.size;
					clone[3].damage = 0;
					clone[3].SetActive(true);
					clone[4] = (Prison)Instantiate(m2, rb.transform.position + new Vector3(spacex, spacey, -spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[4].savedVel = this.savedVel+ new Vector3((Random.value) * clone[4].scale / clone[4].size, (Random.value) * clone[4].scale / clone[4].size, (-Random.value) * clone[4].scale / clone[4].size);
					clone[4].size = this.size;
					clone[4].damage = 0;
					clone[4].SetActive(true);
					clone[5] = (Prison)Instantiate(m2, rb.transform.position + new Vector3(-spacex, spacey, -spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[5].savedVel = this.savedVel+ new Vector3((-Random.value) * clone[5].scale /  clone[5].size, (Random.value) *  clone[5].scale /  clone[5].size, (-Random.value) *  clone[5].scale /  clone[5].size);
					clone[5].size = this.size;
					clone[5].damage = 0;
					clone[5].SetActive(true);
					clone[6] = (Prison)Instantiate(m2, rb.transform.position + new Vector3(spacex, -spacey, -spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[6].savedVel = this.savedVel+ new Vector3((Random.value) * clone[6].scale / clone[6].size, (-Random.value) * clone[6].scale / clone[6].size, (-Random.value) * clone[6].scale / clone[6].size);
					clone[6].size = this.size;
					clone[6].damage = 0;
					clone[6].SetActive(true);
					clone[7] = (Prison)Instantiate(m2, rb.transform.position + new Vector3(-spacex, -spacey, -spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[7].savedVel = this.savedVel+ new Vector3((-Random.value) * clone[7].scale / clone[7].size, (-Random.value) * clone[7].scale / clone[7].size, (-Random.value) * clone[7].scale / clone[7].size);
					clone[7].size = this.size;
					clone[7].damage = 0;
					clone[7].SetActive(true);
                    explosion.InstantiateExplo(transform.position, transform.rotation);
                    gameObject.SetActive(false);
                    cp.asteroidsInPlay += 7;
                    //Destroy(gameObject);
                }else
                {
					
					
					clone[0] = (Prison)Instantiate(m, rb.transform.position+new Vector3(spacex,spacey,spacez)*size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[0].savedVel = this.savedVel+ new Vector3((Random.value) * clone[0].scale / clone[0].size, (Random.value) * clone[0].scale / clone[0].size, (Random.value) * clone[0].scale / clone[0].size);
					clone[0].size = this.size;
					clone[0].damage = 0;
					clone[0].SetActive(true);
					clone[1] = (Prison)Instantiate(m, rb.transform.position + new Vector3(-spacex, spacey, spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[1].savedVel = this.savedVel+ new Vector3((-Random.value) * clone[1].scale / clone[1].size, (Random.value) * clone[1].scale / clone[1].size, (Random.value) * clone[1].scale / clone[1].size);
					clone[1].size = this.size;
					clone[1].damage = 0;
					clone[1].SetActive(true);
					clone[2] = (Prison)Instantiate(m, rb.transform.position + new Vector3(spacex, -spacey, spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[2].savedVel = this.savedVel+ new Vector3((Random.value) * clone[2].scale / clone[2].size, (-Random.value) * clone[2].scale / clone[2].size, (Random.value) * clone[2].scale / clone[2].size);
					clone[2].size = this.size;
					clone[2].damage = 0;
					clone[2].SetActive(true);
					clone[3] = (Prison)Instantiate(m, rb.transform.position + new Vector3(-spacex, -spacey, spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[3].savedVel = this.savedVel+ new Vector3((-Random.value) * clone[3].scale / clone[3].size, (-Random.value) * clone[3].scale /  clone[3].size, (Random.value) *  clone[3].scale /  clone[3].size);
					clone[3].size = this.size;
					clone[3].damage = 0;
					clone[3].SetActive(true);
					clone[4] = (Prison)Instantiate(m, rb.transform.position + new Vector3(spacex, spacey, -spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[4].savedVel = this.savedVel+ new Vector3((Random.value) * clone[4].scale / clone[4].size, (Random.value) * clone[4].scale / clone[4].size, (-Random.value) * clone[4].scale / clone[4].size);
					clone[4].size = this.size;
					clone[4].damage = 0;
					clone[4].SetActive(true);
					clone[5] = (Prison)Instantiate(m, rb.transform.position + new Vector3(-spacex, spacey, -spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[5].savedVel = this.savedVel+ new Vector3((-Random.value) * clone[5].scale /  clone[5].size, (Random.value) *  clone[5].scale /  clone[5].size, (-Random.value) *  clone[5].scale /  clone[5].size);
					clone[5].size = this.size;
					clone[5].damage = 0;
					clone[5].SetActive(true);
					clone[6] = (Prison)Instantiate(m, rb.transform.position + new Vector3(spacex, -spacey, -spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[6].savedVel = this.savedVel+ new Vector3((Random.value) * clone[6].scale / clone[6].size, (-Random.value) * clone[6].scale / clone[6].size, (-Random.value) * clone[6].scale / clone[6].size);
					clone[6].size = this.size;
					clone[6].damage = 0;
					clone[6].SetActive(true);
					clone[7] = (Prison)Instantiate(m, rb.transform.position + new Vector3(-spacex, -spacey, -spacez) * size, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
					clone[7].savedVel = this.savedVel+ new Vector3((-Random.value) * clone[7].scale / clone[7].size, (-Random.value) * clone[7].scale / clone[7].size, (-Random.value) * clone[7].scale / clone[7].size);
					clone[7].size = this.size;
					clone[7].damage = 0;
					clone[7].SetActive(true);
                    explosion.InstantiateExplo(transform.position, transform.rotation);
                    gameObject.SetActive(false);
                    //Destroy(gameObject);
                    cp.asteroidsInPlay += 7;

                }


            }
            else if (size ==1)
            {
                cp.score += 200;
                cp.totalMoonoids--;
                explosion.InstantiateExplo(transform.position, transform.rotation);
                GamePersistence.control.Remove(this);
                gameObject.SetActive(false);
                cp.asteroidsInPlay--;

                //Destroy(gameObject);
            }
        }
        if (active && col.gameObject.name.Contains("Asteroid") && col.gameObject.GetComponent<Prison>().active)
        {

            //col.gameObject.GetComponent<Prison>().rb.angularVelocity = -col.gameObject.GetComponent<Prison>().rb.angularVelocity;
            m.rb.angularVelocity = m2.rb.angularVelocity = -rb.angularVelocity * 0.7f;
            //col.gameObject.GetComponent<Prison>().rb.velocity = -col.gameObject.GetComponent<Prison>().rb.velocity;
            //m.rb.velocity = m2.rb.velocity = -rb.velocity;
        }
        if (active && col.gameObject.name.Contains("Pirate"))
        {

            //col.gameObject.GetComponent<Prison>().rb.angularVelocity = -col.gameObject.GetComponent<Prison>().rb.angularVelocity;
            m.rb.velocity = m2.rb.velocity = -rb.velocity - pirate.velocity;
            //col.gameObject.GetComponent<Prison>().rb.velocity = -col.gameObject.GetComponent<Prison>().rb.velocity;
            //m.rb.velocity = m2.rb.velocity = -rb.velocity;
        }

    }

}
