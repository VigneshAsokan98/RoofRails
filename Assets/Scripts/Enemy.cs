using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public Transform Pistol;
    public GameObject bullet;

    GameObject _bulletClone;
    void Shoot()
    {
        _bulletClone = Instantiate(bullet, Pistol);
        _bulletClone.transform.parent = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerpos = PlayerController.instance.transform.position;
            playerpos.y = transform.position.y;
            transform.LookAt(playerpos);

            Shoot();
        }
    }
}
