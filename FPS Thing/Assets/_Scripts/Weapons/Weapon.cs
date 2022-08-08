using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Weapon
{

    protected float _size = 4.0f;
    protected float _curDistance = 1.0f;
    protected float _smoothDistance = 1.0f;

    protected float _hitTargetCooldown = 0.0f;
    protected float _hitDistance = 1.0f;

    public enum WeaponTypes { None, Weapon1, Weapon2, Weapon3, Weapon4, Weapon5, End }
    protected Player player;


    public static Weapon GetWeapon(WeaponTypes type, Player owner)
    {
        switch (type)
        {
            default:
            case WeaponTypes.None:
                return new Weapon(owner);
            case WeaponTypes.Weapon1:
                return new Weapon1(owner);
            case WeaponTypes.Weapon2:
                return new Weapon2(owner);
            case WeaponTypes.Weapon3:
                return new Weapon3(owner);
            case WeaponTypes.Weapon4:
                return new Weapon4(owner);
            case WeaponTypes.Weapon5:
                return new Weapon5(owner);
        }
    }


    public Weapon(Player _player)
    {
        player = _player;
    }


    public virtual void Start()
    {

    }
    public virtual void Update()
    {

    }


    protected virtual void Fire1()
    {

    }
    protected virtual void Fire2()
    {

    }


    public virtual void HandleInput()
    {

    }
    public virtual void HandleDisplay()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = CameraCtrl.cmr.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Physics.Raycast(ray, out hit);

        float curDistance = hit.distance;
        if (curDistance == 0.0f || hit.rigidbody == null)
            curDistance = 1.0f;
        else
        {
            curDistance = Mathf.Clamp(100.0f - curDistance, 10.0f, 100.0f) * 0.01f;
        }

        _curDistance = curDistance;
        _smoothDistance = Mathf.Lerp(_smoothDistance, _curDistance, Time.deltaTime * 5.0f);
    }

    public virtual void DisplayCrosshair(float x, float y, bool simple)
    {

    }


}
