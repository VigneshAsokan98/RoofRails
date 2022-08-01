using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestruct : MonoBehaviour
{
    float timer=0;
    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer >=4)
            Destroy(this.gameObject);
    }
}
