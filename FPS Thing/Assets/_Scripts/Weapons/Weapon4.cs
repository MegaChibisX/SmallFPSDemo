using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon4 : Weapon
{

    public int fansAvailable = 50;

    protected float _arrowRotation = 0.0f;
    private float charge = 0.0f;



    public Weapon4(Player _player) : base(_player)
    {

    }


    public override void HandleInput()
    {
        if (Input.GetMouseButton(0))
            charge += Time.deltaTime * 1.5f;
        if (Input.GetMouseButtonUp(0))
        {
            ThrowFan(false);
            charge = 0.0f;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            player.GetComponent<Rigidbody>().velocity = CameraCtrl.instance.transform.forward * 100f;
        }
    }


    protected void ThrowFan(bool isMelee)
    {
        if (fansAvailable <= 0)
            return;


        Ray ray = CameraCtrl.cmr.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        GameObject shot = Object.Instantiate(player.wpnGui.W4_Fan.gameObject);
        shot.transform.position = player.transform.position;
        shot.transform.rotation = Quaternion.LookRotation(ray.direction);
        shot.GetComponent<Attack>().owner = player;
        shot.GetComponent<Weapon4_Fan>().ownerWpn = this;
        if (isMelee)
        {
            shot.GetComponent<Weapon4_Fan>().speed = 0.4f;
            shot.GetComponent<Weapon4_Fan>().charge = -2.0f;
        }
        else
            shot.GetComponent<Weapon4_Fan>().charge = Mathf.Clamp(charge, 0.0f, 5.0f);


        fansAvailable--;
    }


    public override void DisplayCrosshair(float x, float y, bool simple)
    {
        if (simple)
        {
            if (player.wpnGui.W1_crossHairFull != null)
                GUI.DrawTexture(new Rect(x * 14, y * 16 - x * _size * 0.5f, x * _size, x * _size), player.wpnGui.W1_crossHairFull);
        }
        else
        {
            float multi = _size;

            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f, y * 16 - x * multi * 0.5f,
                                     x * multi, x * multi), player.wpnGui.W1_arrowsStatic);

            if (Event.current.type.Equals(EventType.Repaint))
            {
                Graphics.DrawTexture(new Rect(x * 16 - x * multi * 0.5f * _hitDistance, y * 16 - x * multi * 0.5f * _hitDistance,
                         x * multi * _hitDistance, x * multi * _hitDistance),
                         player.wpnGui.W1_hitTarget, player.wpnGui.hitMaterial);
            }
        }
    }


    public IEnumerator fadeHitMaterial(RaycastHit hit)
    {
        _hitDistance = hit.distance;
        if (_hitDistance == 0.0f || hit.rigidbody == null)
            _hitDistance = 1.0f;
        else
        {
            _hitDistance = Mathf.Clamp(100.0f - _hitDistance, 10.0f, 100.0f) * 0.01f;
        }
        _hitTargetCooldown = 1.0f;

        Color c = player.wpnGui.hitMaterial.GetColor("_Color");

        yield return null;
        _hitTargetCooldown -= Time.deltaTime;

        while (_hitTargetCooldown > 0.0f)
        {
            if (_hitTargetCooldown == 1.0f)
                yield break;
            c.a = _hitTargetCooldown * 2;
            player.wpnGui.hitMaterial.SetColor("_Color", c);
            _hitTargetCooldown -= Time.deltaTime * 5;
            yield return null;
        }

        c.a = 0;
        player.wpnGui.hitMaterial.SetColor("_Color", c);

    }



}

