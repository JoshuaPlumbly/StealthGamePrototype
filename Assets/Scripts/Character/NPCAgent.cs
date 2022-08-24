using Assets.Code.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.NPC
{
    [RequireComponent(typeof(NavMeshAgent),typeof(AgentFSM))]
    public class NPCAgent: MonoBehaviour
    {
        [Header("Refrances")]
        [SerializeField] NavMeshAgent _navMesh;
        [SerializeField] AgentFSM _finiteStateMachine;

        public void Awake()
        {
            _navMesh = this.GetComponent<NavMeshAgent>();
            _finiteStateMachine = this.GetComponent<AgentFSM>();

        }

        public void Start()
        {
            //_finiteStateMachine.EnterState(FSMStateType.PATROL);
        }
    }
}