using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class BaseEnemy : MonoBehaviour {
    private int damage = 0;
    public static int numMissiles = 1000;
    private int simulMumMissiles = 5;
    public EnemyMissilePhysics m;
    public EnemyMissilePhysics[] clone;
    public int missileCount = numMissiles;
    public static int numCraft = 5;
    private int simulCraft = 3;
    public EnemyCraft ec;
    public EnemyCraft[] ecclone;
    public int ecCount = numCraft;
    private float time = 0.0f;
    private float ctime = 0.0f;
    public CapsulePhysics cp;
    public InstantiateExplosion explosion;

    // Use this for initialization
    void Start()
    {
        ecclone = new EnemyCraft[simulCraft];
        clone = new EnemyMissilePhysics[simulMumMissiles];
        for (int i = 0; i < simulMumMissiles; i++)
        {
            clone[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (missileCount > 0 && time > 5f)
        {
            Debug.Log("Launching missile");
            time = 0.0f;
            --missileCount;

            if (clone[missileCount % simulMumMissiles] != null)
            {
                clone[missileCount % simulMumMissiles].Destroy();
                clone[missileCount % simulMumMissiles] = null;
            }
            clone[missileCount % simulMumMissiles] = (EnemyMissilePhysics)Instantiate(m, transform.position + (Vector3.up * 3.0f), new Quaternion(0.0f,0.0f,0.0f,0.0f));
            clone[missileCount % simulMumMissiles].instanceNumber = missileCount % simulMumMissiles;

            clone[missileCount % simulMumMissiles].SetActive(true);
        }
        else
        {
            time += Time.deltaTime;

        }
 //       if (ecCount > 0 && ctime > (80f+Random.value * 500f))
 //       {
 //           Debug.Log("Launching craft");
 //           ctime = 0.0f;
 //           --ecCount;
//
//            if (ecclone[ecCount % simulCraft] != null)
 //           {
//               ecclone[ecCount % simulCraft].Destroy();
//                ecclone[ecCount % simulCraft] = null;
//            }
//            ecclone[ecCount % simulCraft] = (EnemyCraft)Instantiate(ec, transform.position + (new Vector3(Random.value,Random.value,Random.value)* 30.0f), Quaternion.LookRotation(-Vector3.up, Vector3.up));
//            ecclone[ecCount % simulCraft].instanceNumber = ecCount % simulCraft;
//
//            ecclone[ecCount % simulCraft].SetActive(true);
//        }
//        else
//        {
//            ctime += Time.deltaTime;
//
//        }
}

void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Capsule")
        {
            Debug.Log("Landed on enemy base");
        }
        else if (col.gameObject.name == "Missile(Clone)")
        {
            Debug.Log("Missile on enemy base");

            damage += 30;
        }
        else if (col.gameObject.name == "Bomb(Clone)")
        {
            Debug.Log("Bomb on enemy base");

            damage += 100;
        }
        if (damage >= 100)
        {
            cp.score += 500;
            explosion.InstantiateExplo(transform.position, transform.rotation);

            Destroy(gameObject); 
        }
    }

}
