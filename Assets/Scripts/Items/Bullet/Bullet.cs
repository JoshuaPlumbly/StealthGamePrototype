using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class Bullet : BaseProjectle
    {
        [SerializeField] float speed;
        [SerializeField] private float damage;
        [SerializeField] private float dropRate;
        [SerializeField] private float drag;
        private Vector3 gravity;
        private Vector3 prevPos;
        [SerializeField] private Vector3 velocity;

        private void Awake()
        {
            gravity = Physics.gravity;
        }

        public override void OnProjectleSpawn(int _firedBy)
        {
            base.OnProjectleSpawn(_firedBy);
        }

        public override void Update()
        {
            base.Update();
        }

        private void FixedUpdate()
        {
            velocity = transform.forward * speed * Time.deltaTime;

            // Store current position as previous before moving
            prevPos = transform.position;

            // Move object to new position
            this.transform.position += velocity;

            // Detect if projectile has moved thought something between position update or has collided with anything
            RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, (transform.position - prevPos).normalized), (transform.position - prevPos).magnitude);
            
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != null)
                    Hit(hits[i].collider.transform);
            }
        }

        /// <summary>
        /// Projectile has collided with an object.
        /// </summary>
        /// <param name="hit"></param>
        private void Hit(Transform hit)
        {
            // Prevent self collision.
            if (hit.transform.root.GetInstanceID() == firedBy)
                return;

            if (hit.transform.GetInstanceID() == transform.GetInstanceID())
                return;

            // Check for health component
            Health health = hit.root.GetComponent<Health>();

            // Take damage if health is detected
            if (health != null)
            {
                health.TakeDamage((int)damage);
            }

            // Deactivate
            base.OnDesapwn();
        }
    }
}