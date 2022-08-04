using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM
{
    [CreateAssetMenu(menuName = "Director/Action/ActSpawnBackUp")]
    public class ActSpawnBackUp : DirectorAction
    {
        public override void Act(DirectorFSM director)
        {
            GameObject obj = PoolManager.Instance.SpawnObjectReturn(director.GuardPrefab, Vector3.zero, Quaternion.identity);
            director.GuardList.Add(obj.transform);
            obj.SetActive(true);
            Debug.Log(obj.name);
        }
    }
}