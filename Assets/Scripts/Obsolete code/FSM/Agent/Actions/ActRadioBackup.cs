using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM
{
    [CreateAssetMenu(menuName = "Agent/Action/ActRadioBackup")]
    public class ActRadioBackup : AgentAction
    {
        public override void Act(AgentFSM agent)
        {
            CallBackUp callBackUP = new CallBackUp();
            callBackUP.go = agent.gameObject;
            MyEventSystem.Instance.RunEvent(callBackUP);
            Debug.Log("Backup Radioed");
        }
    }
}