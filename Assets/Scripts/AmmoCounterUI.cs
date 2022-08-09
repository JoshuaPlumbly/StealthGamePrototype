using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : MonoBehaviour
{
    [SerializeField] private Text _counterText;
    private Gun gun;

    private void Awake()
    {
        gun = GetComponentInChildren<Gun>();
        gun.RefreshedAmmoCount += Refresh;
    }

    public void Refresh ()
    {
        _counterText.text = gun.GetLoadedAmmo().ToString() + " / " + gun.GetUnloadedAmmo().ToString();
	}
}
