using UnityEngine;
using System.Collections;

public class InstantiateExplosion : MonoBehaviour {
    public GameObject explosion;
    private int numSimulExp = 5;
    private Object[] clone;
    private int itemNum = 0;
    void Start()
    {
        clone = new Object[numSimulExp];
        for (int i=0;i< numSimulExp; i++)
        {
            clone[i] = null;
        }
    }

    public void InstantiateExplo(Vector3 position, Quaternion rotation)
    {
        if (clone[itemNum]== null)
        {
            clone[itemNum] = Instantiate(explosion, position, rotation);
        }
        else
        {
            Destroy(clone[itemNum]);
            clone[itemNum] = Instantiate(explosion, position, rotation);
        }
        itemNum = (itemNum + 1) % numSimulExp;
    }
}
