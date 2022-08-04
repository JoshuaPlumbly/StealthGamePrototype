using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.FSM
{
    public class DirectorFSM : MonoBehaviour
    {
        public static DirectorFSM instance = null;

        [SerializeField] Transform player;
        [SerializeField] List<Transform> guardList;

        [Header("Spawn System")]
        [SerializeField] Transform spawnPoints;
        [SerializeField] GameObject guardPrefab;

        [SerializeField] DirectorState backUp;

        public DirectorState currentState;
        public DirectorState previousState;
        public DirectorState remainState;
        private float stateStartTime;


        #region Encapalated fields
        public float StateStartTime { get { return stateStartTime; } }
        public GameObject GuardPrefab { get { return guardPrefab; } }

        public Transform Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        public List<Transform> GuardList
        {
            get
            {
                return guardList;
            }

            set
            {
                guardList = value;
            }
        }
        #endregion

        protected virtual void Awake()
        {
            if (instance == null)
                instance = this;

            else if (instance != this)
                Destroy(gameObject);

            previousState = null;

            // Player and guards
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject[] findGameObject = GameObject.FindGameObjectsWithTag("Guard");

            foreach (GameObject gameObj in findGameObject)
            {
                GuardList.Add(gameObj.transform);
            }

            // Set up pool
            PoolManager.Instance.CreatePool(guardPrefab, 4);

            // Set up lisenors
            MyEventSystem.Instance.RegisterListener<CallBackUp>(BackupLisner);
        }

        protected virtual void Update()
        {
            currentState.UpdateState(this);
        }

        public virtual void TransitionToState(DirectorState nextState)
        {
            if (nextState != remainState)
            {
                currentState.Exit(this);
                previousState = currentState;
                currentState = nextState;
                stateStartTime = Time.time;
                currentState.Enter(this);
            }
        }

        public bool CheckIfCountDownElaapsed(float duration)
        {
            return (Time.time - stateStartTime < duration);
        }

        public void BackupLisner(CallBackUp obj)
        {
            TransitionToState(backUp);
        }
    }
}