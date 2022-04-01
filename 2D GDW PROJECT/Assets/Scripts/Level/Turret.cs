using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float detectRange;

    public GameObject player;

    public Transform playerTran;

    bool detected;

    Vector2 direction;

    public GameObject barrel;

    public Transform gunPos;

    public GameObject bullet;

    public float bulletSpeed;

    public float fireRate;

    bool canShoot = true;

    float timeTilNextShot = 0;


    // Update is called once per frame
    void Update()
    {
        canShoot = player.GetComponent<PlayerController>().GetIsRunning();

        Vector2 playerPos = playerTran.position;

        direction = playerPos - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, direction, detectRange);

        if (rayInfo)
        {
            if (rayInfo.collider.gameObject.tag == "Player")
            {
                if (detected == false)
                {
                    detected = true;
                }
            }
            else
            {
                if (detected == true)
                {
                    detected = false;
                }
            }

            if (detected)
            {
                barrel.transform.up = direction;
                if (Time.time > timeTilNextShot)
                {
                    timeTilNextShot = Time.time + 1 / fireRate;
                    if (canShoot) 
                    {
                        Shoot();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    private void Shoot()
    {
       GameObject bulletPrefab = Instantiate(bullet, gunPos.position, Quaternion.identity);

        bulletPrefab.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);
    }
}