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
    [CreateAssetMenu(menuName = "Director/State")]
    public class DirectorState : ScriptableObject
    {
        public DirectorAction[] startActions;
        public DirectorAction[] updateActions;
        public DirectorTransition[] transitions;

        /// <summary>
        /// Is called once by the Director when this State is first selected.
        /// </summary>
        /// <param name="director"></param>
        public void Enter(DirectorFSM director)
        {
            StartActions(director);
        }

        /// <summary>
        /// Is called evary frame by the Director whilest this State is selected.
        /// </summary>
        /// <param name="director"></param>
        public void UpdateState(DirectorFSM director)
        {
            UpdateActions(director);
            CheckTransitions(director);
        }

        /// <summary>
        /// Is called by the Director before state is chanaged.
        /// </summary>
        /// <param name="director"></param>
        public void Exit(DirectorFSM director)
        {
            ExitActions(director);
        }

        /// <summary>
        /// Runs evary Action whithin a array.
        /// </summary>
        /// <param name="director"></param>
        public void StartActions(DirectorFSM director)
        {
            for (int i = 0; i < startActions.Length; i++)
            {
                startActions[i].Act(director);
            }
        }

        /// <summary>
        /// Runs evary Action whithin a array.
        /// </summary>
        /// <param name="director"></param>
        private void UpdateActions(DirectorFSM director)
        {
            for (int i = 0; i < updateActions.Length; i++)
            {
                updateActions[i].Act(director);
            }
        }

        /// <summary>
        /// Runs evary Atcion within a array.
        /// </summary>
        /// <param name="director"></param>
        public void ExitActions(DirectorFSM director)
        {
            for (int i = 0; i < startActions.Length; i++)
            {
                startActions[i].Act(director);
            }
        }

        /// <summary>
        /// Is used to check to see if the Director switch State.
        /// </summary>
        /// <param name="director"></param>
        private void CheckTransitions(DirectorFSM director)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionSucceeded = transitions[i].decision.Decide(director);

                if (decisionSucceeded)
                {
                    director.TransitionToState(transitions[i].trueState);
                }
                else
                {
                    director.TransitionToState(transitions[i].falseState);
                }
            }
        }
    }
}