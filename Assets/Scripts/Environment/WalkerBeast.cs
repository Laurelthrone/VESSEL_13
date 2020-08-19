using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerBeast : MonoBehaviour
{
    //False - left
    //True - right
    bool direction = false, flipEnabled = true;
    float flipCooldown = .01f;
    public float speed = .75f;

    void Update()
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + speed * (direction ? 1 : -1) * Time.deltaTime * 10, gameObject.transform.position.y);
    }

    void Flip()
    {
        if (flipEnabled)
        {
            flipEnabled = false;
            direction = !direction;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            StartCoroutine(Cooldown()); 
        }
    }

    void Killed()
    {
        Destroy(gameObject);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(flipCooldown);
        flipEnabled = true;
    }
}
