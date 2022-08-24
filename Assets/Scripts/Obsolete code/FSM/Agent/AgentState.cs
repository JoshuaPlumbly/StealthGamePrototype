using Assets.Code.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using Assets.Code;
using Assets.Code.NPC;

namespace Assets.Code.StateMachine
{
    [CreateAssetMenu(menuName = "Agent/State")]
    public class AgentState : ScriptableObject
    {
        public AgentAction[] startActions;
        public AgentAction[] updateActions;
        public AgentAction[] lateUpdateAction;
        public AgentAction[] exitAction;
        public AgentTransition[] transitions;

        /// <summary>
        /// Is called once by the Agent when this State is first selected.
        /// </summary>
        /// <param name="agent"></param>
        public void Enter(AgentFSM agent)
        {
            StartActions(agent);
        }

        /// <summary>
        /// Is called evary frame by the Agent whilest this State is selected.
        /// </summary>
        /// <param name="agent"></param>
        public void UpdateState(AgentFSM agent)
        {
            UpdateActions(agent);
            CheckTransitions(agent);
        }

        public void LateUpdateState(AgentFSM agent)
        {
            LateUpdateActions(agent);
        }

        /// <summary>
        /// Is called by the Agent before state is chanaged.
        /// </summary>
        /// <param name="agent"></param>
        public void Exit(AgentFSM agent)
        {
            ExitActions(agent);
        }

        /// <summary>
        /// Runs evary Action whithin a array.
        /// </summary>
        /// <param name="agent"></param>
        public void StartActions(AgentFSM agent)
        {
            for (int i = 0; i < startActions.Length; i++)
            {
                startActions[i].Act(agent);
            }
        }

        /// <summary>
        /// Runs evary Action whithin a array.
        /// </summary>
        /// <param name="agent"></param>
        private void UpdateActions(AgentFSM agent)
        {
            for (int i = 0; i < updateActions.Length; i++)
            {
                updateActions[i].Act(agent);
            }
        }

        private void LateUpdateActions(AgentFSM agent)
        {
            for (int i = 0; i < lateUpdateAction.Length; i++)
            {
                lateUpdateAction[i].Act(agent);
            }
        }

        /// <summary>
        /// Runs evary Action  within a array.
        /// </summary>
        /// <param name="agent"></param>
        public void ExitActions(AgentFSM agent)
        {
            for (int i = 0; i < startActions.Length; i++)
            {
                startActions[i].Act(agent);
            }
        }

        /// <summary>
        /// Is used to check to see if the Agent switch State.
        /// </summary>
        /// <param name="agent"></param>
        private void CheckTransitions(AgentFSM agent)
        {
            for (int i =0; i < transitions.Length; i++)
            {
                bool decisionSucceeded = transitions[i].decision.Decide(agent);

                if (decisionSucceeded)
                {
                    agent.TransitionToState(transitions[i].trueState);
                }
                else
                {
                    agent.TransitionToState(transitions[i].falseState);
                }
            }
        }
    }
}