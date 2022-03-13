using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CapsulePhysics : MonoBehaviour, Change
{
    public bool tutorial;
    public AudioClip thrusterSound;
    public AudioClip engineSound;
    public AudioClip explosionSound;
    public AudioClip hullFailureIminentSound;
    public AudioClip bombReleaseSound;
    public AudioClip fuelLowSound;
    public AudioClip musicSound;
    AudioSource audioSrc;

    public Rigidbody rb;
    private Vector3 rbtemp;
    private static double gravity = 5.68e26d * 6.674e-11d / (10000.0d * 10000.0d * 10000.0d);
    private static double thrust = 800d * gravity / (5823.2d * 5823.2d);
    float rotationY;// = 0.0f;
    float rotationX;// = 90.0f;
    float rotationZ; //= 0.0f;
    public float sens = 45.0f;
    public float fuel = 100.0f;
    private float fuelDrain = 0.025f;
    public static int numMissiles = 90;
    private int simulMumMissiles = 20;
    public MissilePhysics m;
    public MissilePhysics[] clone;
    public int missileCount = numMissiles;
    private int leftArrowUp = 100;
    private bool rightArrowUp = true;
    public float doorOpenTime = 10.0f;
    public CapsuleCollider cc;
    private bool landed = false;
    public int moonoids;
    public int totalMoonoids;
    //public Slider proxa;
    //public Slider proxb;
    //public Slider proxl;
   // public Slider proxr;
    public Text altSlider;
    //public Slider altSliderTop;
    public Text verticalSlider;
    public Text fuelSlider;
    //public Slider fuelSliderTop;
    public Slider damageSlider;
    //public Slider damageSliderTop;
    public Text xAxisVelocity;
    public Text zAxisVelocity;
    public Camera mainCamera;
    //public Camera topCamera;
    public Text remainingMoonoids;
    //public Text remainingMoonoidsTop;
    public float damage = 0f;
    public int lives;
    public long score;
    public Text livesText;
    public Text scoreText;
    //public Text livesTextTop;
    //public Text scoreTextTop;
    public Text missilesText;
    //public Text bombsText;
    //public Text missilesTopText;
    //public Text bombsTopText;
    public Text bottomCameraText;
    //public Text topCameraText;
    private Vector3 oldVelocity = new Vector3(0f, 0f, 0f);
    public GameObject afterburn;
    public InstantiateExplosion explosion;
    private Object afterburnClone = null;
    public Image img;
    public Image img2;
    public Image img3;
    public Image img4;
    public Image img5;
    private Vector3 imgScale;
    private Vector3 imgScale2;
    private Vector3 imgScale3;
    private Vector3 imgScale4;
    private Vector3 imgScale5;
    public bool dualScreen;
    private bool bottom = true;
    public ArtificialHorizonScript ahs;
    private Vector3 ahsScale;
    private float explosionTime = 0f;
    private bool explosionDetected = false;
    private bool fuelLowDetected = false;
    private AudioSource audioDamage;
    private AudioSource audioMainEngine;
    private AudioSource audioReverseEngine;
    private AudioSource audioFuel;
    private AudioSource audioExplosion;
    private AudioSource audioBomb;
    private AudioSource audioMusic;
    public GameObject audioDamageGO;
    public GameObject audioFueGOl;
    public GameObject audioExplosionGO;
    public GameObject audioMainEngineGO;
    public GameObject audioReverseEngineGO;
    public GameObject audioBombGO;
    public GameObject audioMusicGO;
    private bool downArrowUp = true;
    private bool init = true;
    public Vector3 initialV;
    public MeshRenderer mr;
    public int asteroidsInPlay;


    // Use this for initialization
    void Start()
    {
        rb.isKinematic = false;

        PlayMusic.Activate(false);
        clone = new MissilePhysics[simulMumMissiles];
        for (int i = 0; i < simulMumMissiles; i++)
        {
            clone[i] = null;
        }
        bottomCameraText.text = "Front View";

        GamePersistence.control.Load();
        lives = GamePersistence.control.lives;
        score = GamePersistence.control.score;
        imgScale = img.transform.localScale;
        imgScale2 = img2.transform.localScale;
        imgScale3 = img3.transform.localScale;
        imgScale4 = img4.transform.localScale;
        imgScale5 = img5.transform.localScale;
        ahsScale = ahs.transform.localScale;

        SetMute(img);
        SetMute(img2);
        SetMute(img3);
        SetMute(img4);
        SetMute(img5);

        audioSrc = mainCamera.GetComponent<AudioSource>();
        audioDamage = audioDamageGO.GetComponent<AudioSource>();
        audioMainEngine = audioMainEngineGO.GetComponent<AudioSource>();
        audioReverseEngine = audioReverseEngineGO.GetComponent<AudioSource>();
        audioFuel = audioFueGOl.GetComponent<AudioSource>();
        audioExplosion = audioExplosionGO.GetComponent<AudioSource>();
        audioBomb = audioBombGO.GetComponent<AudioSource>();
        audioMusic = audioMusicGO.GetComponent<AudioSource>();
        audioMusic.mute = false;
        audioMusic.PlayOneShot(musicSound, GamePersistence.control.effectsLevel);

        rotationX = rotationY = rotationZ = 0.0f;
        //initial velocity
        rb.velocity = initialV;
        rbtemp = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        mr.enabled = false;
        GamePersistence.control.Register(this);


    }


    // Update is called once per frame
    void Update()
    {

        altSlider.text = "Alt: " + Mathf.Round(rb.position.magnitude * 10)+"Km";
        //altSliderTop.value = altSlider.value;

        verticalSlider.text = "VS: " + Mathf.Round(10000f * Vector3.Dot(rb.velocity, rb.position.normalized))+"m/s";
        /*fuelSliderTop.value = */fuelSlider.text = "Fuel: "+ Mathf.Round(fuel) +"%";
        /*damageSliderTop.value = */damageSlider.value = damage;

        if (damage >= 65.0f)
        {
            audioDamage.mute = false;
            audioDamage.PlayOneShot(hullFailureIminentSound, GamePersistence.control.effectsLevel);
        }
        else
        {
            audioDamage.mute = true;
            audioDamage.Stop();

        }
        if (fuel <= 20.0f && !fuelLowDetected)
        {
            audioFuel.PlayOneShot(fuelLowSound, GamePersistence.control.effectsLevel);
            audioFuel.mute = false;
            fuelLowDetected = true;
        }
        else if (fuel > 20.0f && fuelLowDetected)
        {
            audioFuel.mute = true;
            audioFuel.Stop();
            fuelLowDetected = false;
        }

        if (damage >= 100.0f)
        {
            LifeLost();
        }
        if (fuel <= 1f && damage < 100.0f)
        {
            LifeLost();
        }
        if (asteroidsInPlay == 0 && damage < 100.0f)
        {
            lives++;
            GamePersistence.control.score = score;
            GamePersistence.control.Save();
            Application.LoadLevel("MainAsteroids");
        }
        /*remainingMoonoidsTop.text = */
        remainingMoonoids.text = "Asteroids: " + (asteroidsInPlay);
        if (tutorial)
        {
            score = 0;
        }
        /*scoreTextTop.text =*/ scoreText.text = "Score: " + score;
        /*livesTextTop.text = */livesText.text = "Lives: " + lives;
        /*missilesTopText.text = */missilesText.text = "Missiles: " + missileCount;
        Vector3 noHVel;
        noHVel = rb.velocity - rb.position.normalized * Vector3.Dot(rb.position.normalized, rb.velocity);
        xAxisVelocity.text = "RightV: " + Mathf.Round(10000f* Vector3.Dot(noHVel, transform.right.normalized)) + "m/s";
        zAxisVelocity.text = "UpV: " + Mathf.Round(10000f * Vector3.Dot(noHVel, transform.up.normalized)) + "m/s";
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePersistence.control.Remove(this);

            GamePersistence.control.score = this.score;
            GamePersistence.control.Save();
            Application.LoadLevel("alternativeEnd");
        }
    }

    private void loadVideo(Image image, Vector3 imageScale1)
    {
        ahs.transform.localScale = new Vector3(0f, 0f, 0f);
        image.color = Color.white;
        image.GetComponent<Renderer>().material.color = Color.white;
        image.transform.localScale = imageScale1;
        //((VideoPlayer)image.GetComponent<MeshRenderer>().material.mainTexture).Play();


    }
    private void SetMute(Image image)
    {
        ahs.transform.localScale = ahsScale;
        image.color = Color.clear;
        image.GetComponent<MeshRenderer>().material.color = Color.clear;
        image.transform.localScale = new Vector3(0f, 0f, 0f);
        //((VideoPlayer)image.GetComponent<MeshRenderer>().material.mainTexture).Stop();


    }
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.name == "MotherShip")
        {
            Debug.Log("You landed on the Base Friendly");
            //if (HasLandedSuccessfully())
            {
                if (col.gameObject.GetComponent<BaseFriendly>().OnRearm())
                {
 //                   loadVideo(img5, imgScale5);
                    audioMusic.mute = true;
                    PlayMusic.Activate(true);

                }
                if (col.gameObject.GetComponent<BaseFriendly>().OnRefueling())
                {
  //                  loadVideo(img, imgScale);
                    audioMusic.mute = true;
                    PlayMusic.Activate(true);

                }

            }
        }
        else if (col.gameObject.name == "Enemy Missile(Clone)")
        {
            Debug.Log("You collided with " + col.gameObject.name);
            //if (!tutorial)
            {
                damage += 30.0f;
            }
            audioExplosion.PlayOneShot(explosionSound, GamePersistence.control.effectsLevel);
            audioExplosion.mute = false;
            explosionDetected = true;

        }
        else if (col.gameObject.GetComponent<Prison>() != null)
        {
            rb.velocity = rb.velocity.magnitude * (rb.velocity.normalized + (-2.0f * col.gameObject.GetComponent<Prison>().rb.velocity.normalized * Vector3.Dot(rb.velocity.normalized, col.gameObject.GetComponent<Prison>().rb.velocity.normalized)));
        }


    }
    public void playExplosion(float close)
    {
        rotationY += (float)(100 * close * (Random.value - 0.5));
        rotationX += (float)(100 * close * (Random.value - 0.5));
        rotationZ += (float)(100 * close * (Random.value - 0.5));
        transform.Rotate(Vector3.up * rotationZ);
        transform.Rotate(Vector3.left * rotationX);
        transform.Rotate(Vector3.forward * rotationY);

        audioExplosion.PlayOneShot(explosionSound, GamePersistence.control.effectsLevel*close);
        audioExplosion.mute = false;
        explosionDetected = true;

    }


    bool HasLandedSuccessfully()
    {
        Vector3 orientation = rb.transform.forward.normalized;
        float velocityMagnitude = rb.velocity.magnitude;
        float velocityOrientation;
        float orientup;
        {
            velocityOrientation = rb.velocity.normalized.y;
            orientup = orientation.y;
        }
        Debug.Log("velocityMagnitude=" + velocityMagnitude + " orientation.y=" + orientup + "velocityOrientation=" + velocityOrientation);
        if (velocityMagnitude < 0.45f && orientup <= -0.98f && (velocityOrientation <= -0.89f || velocityOrientation >= 0.89f || velocityMagnitude == 0.0f))
        {
            landed = true;
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            bottomCameraText.text = "Landed Successfully";
            //topCameraText.text = "Landed Successfully";
            return true;
        }
        else
        {
            Debug.Log("You crashed");
            LifeLost();
            landed = true;
            bottomCameraText.text = "Crashed";
            //topCameraText.text = "Crashed";
            if (bottom)
            {
                loadVideo(img3, imgScale3);

            }
            else
            {
                loadVideo(img4, imgScale4);

            }
            return false;
        }

    }
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
    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.velocity;
        cc.isTrigger = true;

        /*       if (!landed && !((((oldVelocity - currentVelocity).magnitude / (oldVelocity.magnitude == 0 ? currentVelocity.magnitude : oldVelocity.magnitude)) > 1.8f) &&
                   (((oldVelocity - currentVelocity).magnitude / (oldVelocity.magnitude == 0 ? currentVelocity.magnitude : oldVelocity.magnitude)) < 2.2f)) &&
                   (oldVelocity - currentVelocity).magnitude > (5.0f * .9807f * Time.deltaTime))
               {
                   if (!init) { 
                   Debug.Log("You crashed too many Gs ");
                   LifeLost();
                   landed = true;
                   bottomCameraText.text = "Crashed too many Gs";
                   //topCameraText.text = "Crashed too many Gs";
                   if (bottom)
                   {
                       loadVideo(img3, imgScale3);
                   }
                   else
                   {
                       loadVideo(img4, imgScale4);
                   }
                   }else
                   {
                       init = false;
                   }
               }*/
        oldVelocity = currentVelocity;
        doorOpenTime += Time.deltaTime;

        //transform.localEulerAngles = new Vector3(rotationX, rotationY, rotationZ);
        transform.Rotate(Vector3.up * rotationZ);
        transform.Rotate(Vector3.left * rotationX);
        transform.Rotate(Vector3.forward * rotationY);

        if (Input.GetKey(KeyCode.UpArrow) && fuel > 0.0f)
        {
            if (!tutorial)
            {
                fuel -= (float)(fuelDrain * GamePersistence.control.multiplyer);
            }
            PlayMusic.Activate(false);
            audioMusic.mute = false;

            audioMainEngine.PlayOneShot(engineSound, GamePersistence.control.effectsLevel);
            audioMainEngine.mute = false;
            Quaternion outTheBack = Quaternion.LookRotation(-transform.forward.normalized, Vector3.up);
            rb.AddForce(transform.forward.normalized * (float)(thrust * GamePersistence.control.multiplyer));
            if (afterburnClone != null)
            {
                ((GameObject)afterburnClone).transform.rotation = outTheBack;
                ((GameObject)afterburnClone).transform.position = transform.position;
            }

            if (afterburnClone == null) afterburnClone = Instantiate(afterburn, transform.position, outTheBack);
            landed = false;
            SetMute(img);
            SetMute(img2);
            SetMute(img3);
            SetMute(img4);
            SetMute(img5);
            bottomCameraText.text = "Front View";
            //topCameraText.text = "Top Camera";

        }
        else
        {

            if (afterburnClone != null)
            {
                Destroy(afterburnClone);
                afterburnClone = null;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow) && fuel > 0.0f)
        {
            if (!tutorial)
            {
                fuel -= (float)(fuelDrain * GamePersistence.control.multiplyer);
            }
            PlayMusic.Activate(false);
            audioMusic.mute = false;


            audioReverseEngine.PlayOneShot(engineSound, GamePersistence.control.effectsLevel);
            audioReverseEngine.mute = false;
            rb.AddForce(-transform.forward.normalized * (float)(thrust * GamePersistence.control.multiplyer));
            if (afterburnClone != null)
            {
                ((GameObject)afterburnClone).transform.rotation = transform.rotation;
                ((GameObject)afterburnClone).transform.position = transform.position;
            }

            if (afterburnClone == null) afterburnClone = Instantiate(afterburn, transform.position, transform.rotation);
            landed = false;
            SetMute(img);
            SetMute(img2);
            SetMute(img3);
            SetMute(img4);
            SetMute(img5);
            bottomCameraText.text = "Front View";
            //topCameraText.text = "Top Camera";

        }
        else
        {

            if (afterburnClone != null)
            {
                Destroy(afterburnClone);
                afterburnClone = null;
            }
        }
      
        fuel = fuel < 0.0f ? 0.0f : fuel;
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
                transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            rotationX = rotationY = rotationZ = 0.0f;
            rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                if (!audioSrc.mute)
                {
                    audioSrc.mute = true;
                    audioSrc.Stop();
                }
                audioSrc.PlayOneShot(thrusterSound, GamePersistence.control.effectsLevel);
                audioSrc.mute = false;
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A))
        {
            rotationY = +sens * Time.deltaTime;
            if (!bottom) rotationY = -rotationY;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!audioSrc.mute)
                {
                    audioSrc.mute = true;
                    audioSrc.Stop();
                }
                audioSrc.PlayOneShot(thrusterSound, GamePersistence.control.effectsLevel);
                audioSrc.mute = false;

            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
        {
            rotationY = -sens * Time.deltaTime;
            if (!bottom) rotationY = -rotationY;

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!audioSrc.mute)
                {
                    audioSrc.mute = true;
                    audioSrc.Stop();
                }
                audioSrc.PlayOneShot(thrusterSound, GamePersistence.control.effectsLevel);
                audioSrc.mute = false;

            }

        }
        else
        {
            rotationY = 0.0f;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.W))
        {
            rotationX = -sens * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!audioSrc.mute)
                {
                    audioSrc.mute = true;
                    audioSrc.Stop();
                }
                audioSrc.PlayOneShot(thrusterSound, GamePersistence.control.effectsLevel);
                audioSrc.mute = false;

            }

        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.S))
        {
            rotationX = +sens * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (!audioSrc.mute)
                {
                    audioSrc.mute = true;
                    audioSrc.Stop();
                }
                audioSrc.PlayOneShot(thrusterSound, GamePersistence.control.effectsLevel);
                audioSrc.mute = false;

            }

        }
        else
        {
            rotationX = 0.0f;
        }
        if (Input.GetKey(KeyCode.Q) || Input.GetKeyDown(KeyCode.Q))
        {
            rotationZ = -sens * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (!audioSrc.mute)
                {
                    audioSrc.mute = true;
                    audioSrc.Stop();
                }
                audioSrc.PlayOneShot(thrusterSound, GamePersistence.control.effectsLevel);
                audioSrc.mute = false;

            }

        }
        else if (Input.GetKey(KeyCode.E) || Input.GetKeyDown(KeyCode.E))
        {
            rotationZ = +sens * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!audioSrc.mute)
                {
                    audioSrc.mute = true;
                    audioSrc.Stop();
                }
                audioSrc.PlayOneShot(thrusterSound, GamePersistence.control.effectsLevel);
                audioSrc.mute = false;

            }

        }
        else
        {
            rotationZ = 0.0f;
        }
        if (Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            if (!audioSrc.mute)
            {
                audioSrc.mute = true;
                audioSrc.Stop();
            }
            audioSrc.PlayOneShot(thrusterSound, GamePersistence.control.effectsLevel);
            audioSrc.mute = false;

        }

  
        if ((Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKey(KeyCode.LeftArrow)) )
        {
            if (missileCount > 0 && leftArrowUp == 100)
            {
                --missileCount;
                if (clone[missileCount % simulMumMissiles] != null)
                {
                    clone[missileCount % simulMumMissiles].Destroy();
                    clone[missileCount % simulMumMissiles] = null;
                }
                doorOpenTime = 10.0f;//0.0f
                mr.enabled = true;
                clone[missileCount % simulMumMissiles] = (MissilePhysics)Instantiate(m, m.transform.position, Quaternion.LookRotation(-transform.forward.normalized, Vector3.up));
                clone[missileCount % simulMumMissiles].instanceNumber = missileCount % simulMumMissiles;
                clone[missileCount % simulMumMissiles].SetActive(true);
                mr.enabled = false;
            }
            leftArrowUp += -1;
            if (leftArrowUp <= 0) leftArrowUp = 100;

        }
        if (!(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            leftArrowUp = 100;

        }


        if (Input.GetKeyUp(KeyCode.RightArrow) && !rightArrowUp)
        {
            rightArrowUp = true;

        }

        if (!landed)rb.AddForce(- rb.position.normalized* (float)(gravity * GamePersistence.control.multiplyer / ((double)rb.position.magnitude* (double)rb.position.magnitude )));


        if (doorOpenTime < 0.5f)
        {
            cc.isTrigger = true;
        }
        else if (doorOpenTime > 1.0f)
        {
            cc.isTrigger = false;

        }
        if (explosionDetected)
        {
            explosionTime += Time.deltaTime;
        }
        if (explosionTime > 5.0f)
        {
            explosionDetected = false;
            explosionTime = 0f;
        }
        /*        if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
                {
                    if (!audioSrc.mute)
                    {
                        audioSrc.mute = true;
                        audioSrc.Stop();
                    }
                }*/
        if (!explosionDetected)
        {
            if (!audioExplosion.mute)
            {
                audioExplosion.mute = true;
                audioExplosion.Stop();
            }
        }
        if (!Input.GetKey(KeyCode.UpArrow))
        {
            if (!audioMainEngine.mute)
            {
                audioMainEngine.mute = true;
                audioMainEngine.Stop();
            }
        }
        if (!Input.GetKey(KeyCode.DownArrow))
        {
            if (!audioReverseEngine.mute)
            {
                audioReverseEngine.mute = true;
                audioReverseEngine.Stop();
            }
        }
        if (!Input.GetKey(KeyCode.RightArrow))
        {
            if (!audioBomb.mute)
            {
                audioBomb.mute = true;
                audioBomb.Stop();
            }
        }
        if (!audioMusic.isPlaying && audioMusic.mute == false)
        {
            audioMusic.PlayOneShot(musicSound, GamePersistence.control.effectsLevel);
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
        return diff.magnitude > 300.0f;

    }
    void LifeLost()
    {
        if (!tutorial)
        {
            lives--;
            fuel = 100.0f;
            missileCount = 90;
            damage = 0.0f;
        }
        if (lives == 0)
        {
            GamePersistence.control.Remove(this);

            explosion.InstantiateExplo(transform.position, transform.rotation);
            audioExplosion.PlayOneShot(explosionSound, GamePersistence.control.effectsLevel);
            audioExplosion.mute = false;
            explosionDetected = true;
            GamePersistence.control.score = score;
            GamePersistence.control.Save();
            Application.LoadLevel("alternativeEnd");
            //Hi score table
        }
        else
        {
            damage = 0.0f;
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            audioExplosion.PlayOneShot(explosionSound, GamePersistence.control.effectsLevel);
            audioExplosion.mute = false;
            explosionDetected = true;

        }
    }

}