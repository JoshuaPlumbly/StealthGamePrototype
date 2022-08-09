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

        gun.TriggerReload();
    }

    private void Update()
    {
        if (Input.GetButton("Fire"))
        {
            gun.PrimaryUse();
        }

        if (Input.GetButtonDown("Reload"))
        {
            gun.TriggerReload();
        }

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            movement.Move(input);
        }
    }
}