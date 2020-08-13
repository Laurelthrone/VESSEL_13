using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public float lowerLimit;
    public float upperLimit;

    public GameObject prefab;
    private GameObject thisSpawner;

    // Start is called before the first frame update
    void Start()
    {
        thisSpawner = gameObject;
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Spawner()
    {
      while(true)
        {
            yield return new WaitForSeconds(Random.Range(lowerLimit, upperLimit));
            Instantiate(prefab, new Vector3(thisSpawner.transform.position.x, thisSpawner.transform.position.y, 0), Quaternion.identity);
        }
    }

}
