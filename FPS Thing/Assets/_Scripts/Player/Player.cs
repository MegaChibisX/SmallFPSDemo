using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public static CameraCtrl cmrCtrl;
    public static Camera cmr;
    protected Rigidbody body;

    public AudioSource minorSound;

    public Weapon.WeaponTypes mainWeapon = Weapon.WeaponTypes.Weapon1;
    public Weapon.WeaponTypes stolenWeapon = Weapon.WeaponTypes.None;
    public Weapon.WeaponTypes stolenBodyMods = Weapon.WeaponTypes.None;

    public Weapon weapon;
    public WeaponGUI wpnGui;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        weapon = Weapon.GetWeapon(mainWeapon, this);
        weapon.Start();
    }

    private void Update()
    {
        HandleInput();

        RaycastHit hit = new RaycastHit();
        Physics.Raycast(cmr.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f)), out hit);
        weapon.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = Weapon.GetWeapon(Weapon.WeaponTypes.Weapon1, this);
            weapon.Start();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = Weapon.GetWeapon(Weapon.WeaponTypes.Weapon2, this);
            weapon.Start();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = Weapon.GetWeapon(Weapon.WeaponTypes.Weapon3, this);
            weapon.Start();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weapon = Weapon.GetWeapon(Weapon.WeaponTypes.Weapon4, this);
            weapon.Start();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weapon = Weapon.GetWeapon(Weapon.WeaponTypes.Weapon5, this);
            weapon.Start();
        }
    }
    private void FixedUpdate()
    {
        Move();

        body.velocity = Vector3.Scale(new Vector3(0.9f, 1, 0.9f), body.velocity);
    }
    private void LateUpdate()
    {
        
    }
    protected void OnGUI()
    {
        DisplayGameGUI();
    }


    protected virtual void Move()
    {
        if (cmrCtrl == null)
            return;

        Vector2 v = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        if (v.sqrMagnitude > 1.0f)
            v.Normalize();
        v *= 4000.0f * Time.fixedDeltaTime;

        body.AddForce(cmrCtrl.forward * v.y + cmrCtrl.right * v.x, ForceMode.Acceleration);
    }
    protected virtual void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
            body.AddForce(Vector3.up * 1000.0f, ForceMode.Acceleration);

        weapon.HandleInput();
    }



    protected void DisplayGameGUI()
    {
        float x = Screen.width / 32.0f;
        float y = Screen.height / 32.0f;

        weapon.DisplayCrosshair(x, y, false);
    }

}

