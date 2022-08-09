using UnityEngine;
using Assets.Code;
using System.Collections;

public class Gun : MonoBehaviour, IWeildReloadable
{
    [SerializeField] protected int firedBy;
    [SerializeField] protected Transform barrel;
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected float nosie;
    [SerializeField] protected float secPerRound;
    [SerializeField] protected float secToReload;
    [SerializeField] private int unloadedAmmo;
    [SerializeField] protected int loadedAmmo;
    [SerializeField] protected int maxLoadedAmmo;

    protected float reloadClipAt;
    protected float timeLastFiredAt;

    public bool IsFullyLoaded() { return loadedAmmo >= maxLoadedAmmo ; }
    public int GetTotalAmmo() { return loadedAmmo + unloadedAmmo; }
    public int GetUnloadedAmmo() => unloadedAmmo;
    public int GetLoadedAmmo() => loadedAmmo;
    public bool IsLoaded() { return loadedAmmo > 0; }
    public bool IsUnloaded() { return !IsLoaded(); }
    public bool CooldownFinished() { return Time.time > timeLastFiredAt + secPerRound; }

    public event System.Action RefreshedAmmoCount;

    private void Start()
    {
        timeLastFiredAt = float.NegativeInfinity;
        firedBy = this.transform.root.GetInstanceID();

        PoolManager.Instance.CreatePool(prefab, 5);
    }

    public virtual void PrimaryUse()
    {
        if (IsLoaded() && CooldownFinished())
        {
            timeLastFiredAt = Time.time;
            loadedAmmo--;
            RefreshedAmmoCount?.Invoke();
            Shoot();
        }
    }

    public virtual void Shoot()
    {
        GameObject obj = PoolManager.Instance.SpawnObjectReturn(prefab, barrel.position, barrel.rotation);
        obj.GetComponent<Bullet>().OnProjectleSpawn(firedBy);
        obj.SetActive(true);
    }

    private IEnumerator _reloadCoroutine = null;
    /// <summary>
    /// Start reloading clip.
    /// </summary>
    /// <param name="_reloadTime"></param>
    public void TriggerReload()
    {
        IEnumerator Reload()
        {
            float t = 0f;

            while (t < secToReload)
            {
                t += Time.deltaTime;
                yield return null;
            }

            FinishReload();
            _reloadCoroutine = null;
        }

        if (_reloadCoroutine != null)
            return;

        _reloadCoroutine = Reload();

        StartCoroutine(_reloadCoroutine);
    }

    /// <summary>
    /// Finish reloading clip.
    /// </summary>
    /// <returns></returns>
    private void FinishReload()
    {
        int reloadAmont = maxLoadedAmmo - loadedAmmo;
        reloadAmont = unloadedAmmo - reloadAmont >= 0 ? reloadAmont : unloadedAmmo;
        loadedAmmo += reloadAmont;
        unloadedAmmo -= reloadAmont;
        RefreshedAmmoCount?.Invoke();
    }
}

public interface IWeildable { void PrimaryUse(); }
public interface IWeildReloadable : IWeildable { void TriggerReload(); }