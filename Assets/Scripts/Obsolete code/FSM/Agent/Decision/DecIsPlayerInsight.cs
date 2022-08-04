using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This code was made with the aid of the following cited souces:
 * Unity. (2018). Decisions and Looking - Unity. [online] Available at: https://unity3d.com/learn/tutorials/topics/navigation/decisions-and-looking?playlist=17105 [Accessed 29 Oct. 2018].
 */

namespace Assets.Code.FSM
{
    [CreateAssetMenu(menuName = "StateMachine/Decision/PlayerInsight")]
    public class DecIsPlayerInsight : AgentDecision  {
        public override bool Decide(AgentFSM agent)
        {
            bool playerInsight = CheckPlayer(agent);
            return playerInsight;
        }

        public bool CheckPlayer(AgentFSM agent)
        {
            RaycastHit hit;

            // Visualise agent's sight.
            Debug.DrawRay(agent.AgentStats.eyes.position, agent.AgentStats.eyes.forward.normalized * agent.AgentStats.sight, Color.green);

            // Check if the agent is able to see the player.
            if (Physics.SphereCast(agent.AgentStats.eyes.position, agent.AgentStats.identifyPlayerAt, agent.AgentStats.eyes.forward, out hit, agent.AgentStats.identifyPlayerAt)
                && hit.collider.CompareTag("Player"))
            {
                agent.AgentStats.target = hit.transform;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}