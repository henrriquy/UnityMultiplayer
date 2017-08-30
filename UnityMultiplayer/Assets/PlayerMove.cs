﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMove : NetworkBehaviour
{
    public GameObject bulletPrefab;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * 0.1f;
        var z = Input.GetAxis("Vertical") * 0.1f;

        transform.Translate(x, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Command function is called from the client, but invoked on the server
            CmdFire();
        }

    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    [Command]
    void CmdFire()
    {
        //This [Command] code is run on the server!

        //Create the bullet object locally
        //Create the bullet object from the bullet prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            transform.position - transform.forward,
            Quaternion.identity
            );

        //make the bullet move away in front of the player
        bullet.GetComponent<Rigidbody>().velocity = -transform.forward * 4;

        //spaw the bullet on the clients
        NetworkServer.Spawn(bullet);

        //When the bullet is destroyt on the server
        Destroy(bullet, 2.0f);

    }

    //void Fire()
    //{
    //    //Create the bullet object from the bullet prefab
    //    var bullet = (GameObject)Instantiate(
    //        bulletPrefab,
    //        transform.position - transform.forward,
    //        Quaternion.identity
    //        );
    //
    //    //make the bullet move away in front of the player
    //    bullet.GetComponent<Rigidbody>().velocity = -transform.forward * 4;
    //
    //    //make bullet disappear after 2 seconds
    //    Destroy(bullet, 2.0f);
    //}
}
