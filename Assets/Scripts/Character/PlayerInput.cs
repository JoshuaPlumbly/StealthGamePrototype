using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Gun gun;
    [SerializeField] private PlayerMovement movement;

    AmmoCounterUI ammoCounterUI;

    private void Awake()
    {
        gun = GetComponentInChildren<Gun>();
        movement = GetComponent<PlayerMovement>();
        ammoCounterUI = GetComponentInChildren<AmmoCounterUI>();

        gun.StartReload(0.0f);
    }

    private void Update()
    {
        if (Input.GetButton("Fire"))
        {
            gun.FireTrigger();
            ammoCounterUI.UpdateUI();
        }

        if (Input.GetButtonDown("Reload"))
        {
            gun.FinishReload();
            ammoCounterUI.UpdateUI();
        }

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            movement.Move(input);
        }
    }
}