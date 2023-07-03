using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Moltensoft.TankGame {
    public class Cannon : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public float bulletSpeed;
        public float cooldownSeconds;
        public float lastFired;

        public void Fire() {
            var timeSinceLastFired = Time.time - lastFired;
            if (timeSinceLastFired > cooldownSeconds) {
                lastFired = Time.time;
                GetComponentInChildren<ParticleSystem>().Emit(1);
                GameObject bulletObj = Instantiate(bulletPrefab, transform.position, transform.rotation);
                bulletObj.GetComponent<Rigidbody>().velocity = -transform.right * bulletSpeed;
                var owner = transform.parent.gameObject;
                owner.GetComponent<Combatant>().shots++;
                bulletObj.GetComponent<Bullet>().owner = owner;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }


    }
}
