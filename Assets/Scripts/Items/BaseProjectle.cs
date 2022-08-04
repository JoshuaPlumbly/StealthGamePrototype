using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class BaseProjectle : PooledObjects
    {
        [SerializeField] protected bool ignoreLifeTime;
        [SerializeField] protected float lifeTime;
        [SerializeField] protected int firedBy = 0;

        private float disableAt;
        private bool checkTime;

        public virtual void OnProjectleSpawn(int _firedBy)
        {
            firedBy = _firedBy;
            disableAt = Time.time + lifeTime;
            checkTime = true;
        }

        public virtual void Update()
        {
            if (!ignoreLifeTime && checkTime && Time.time > disableAt)
                OnDesapwn();
        }

        public virtual void OnDesapwn()
        {
            checkTime = false;
            gameObject.SetActive(false);
        }
    }
}