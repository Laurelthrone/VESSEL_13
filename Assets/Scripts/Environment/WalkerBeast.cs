using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerBeast : MonoBehaviour
{
    public GameObject detector;
    
    //False - left
    //True - right
    bool direction = false;
    

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + .75f * (direction ? 1 : -1) * Time.deltaTime * 10, gameObject.transform.position.y);
    }

    void Flip()
    {
        direction = !direction;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1, 4);
    }

    void Killed()
    {
        Destroy(gameObject);
    }
}
