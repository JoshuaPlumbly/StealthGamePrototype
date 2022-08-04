using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : MonoBehaviour
{
    [SerializeField] private Text ammoUI;
    private Gun gun;

    private void Awake()
    {
        gun = GetComponentInChildren<Gun>();
        UpdateUI();
    }

    public void UpdateUI ()
    {
        ammoUI.text = gun.Clip.ToString() + " / " + gun.CurrentAmmo.ToString();
	}
	
}
