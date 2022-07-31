using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    float timer = 0;
    private void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (timer <= 1.5)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(this);
        }
    }
}
