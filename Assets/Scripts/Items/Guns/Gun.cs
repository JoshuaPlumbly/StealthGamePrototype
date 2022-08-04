using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;
using System;

public class Gun : MonoBehaviour
{
    [SerializeField] protected int firedBy;
    [SerializeField] protected Transform barrel;
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected float nosie;
    [SerializeField] protected float secPerRound;
    [SerializeField] private int currentAmmo;
    [SerializeField] protected int loadedAmmo;
    [SerializeField] protected int maxLoadedAmmo;

    protected float reloadClipAt;
    protected float timeLastFiredAt;

    public bool IsFullyLoaded() { return loadedAmmo >= maxLoadedAmmo ; }
    public bool IsLoaded() { return loadedAmmo > 0; }
    public bool IsUnloaded() { return !IsLoaded(); }
    public bool CooldownFinished() { return Time.time > timeLastFiredAt + secPerRound; }

    public event System.Action FiredRound;

    #region Encapulated variables
    public int CurrentAmmo
    {
        get
        {
            return currentAmmo;
        }

        set
        {
            currentAmmo = value;
        }
    }

    public int Clip
    {
        get
        {
            return loadedAmmo;
        }

        set
        {
            loadedAmmo = value;
        }
    }
    #endregion

    private void Start()
    {
        timeLastFiredAt = float.NegativeInfinity;
        firedBy = this.transform.root.GetInstanceID();

        PoolManager.Instance.CreatePool(prefab, 5);
    }

    public virtual void FireTrigger()
    {
        if (IsLoaded() && Time.time > timeLastFiredAt)
        {
            timeLastFiredAt = Time.time;
            loadedAmmo--;
            FiredRound?.Invoke();
            Shoot();
        }
    }

    public virtual void Shoot()
    {
        GameObject obj = PoolManager.Instance.SpawnObjectReturn(prefab, barrel.position, barrel.rotation);
        obj.GetComponent<Bullet>().OnProjectleSpawn(firedBy);
        obj.SetActive(true);
    }

    /// <summary>
    /// Start reloading clip.
    /// </summary>
    /// <param name="_reloadTime"></param>
    public void StartReload(float _reloadTime)
    {
        Invoke("FinishReload", _reloadTime);
    }

    /// <summary>
    /// Finish reloading clip.
    /// </summary>
    /// <returns></returns>
    public void FinishReload()
    {
        int reloadAmont = maxLoadedAmmo - loadedAmmo;
        reloadAmont = currentAmmo - reloadAmont >= 0 ? reloadAmont : currentAmmo;
        loadedAmmo += reloadAmont;
        currentAmmo -= reloadAmont;
    }

    public int TotalAmmo() { return loadedAmmo + currentAmmo; }
}
