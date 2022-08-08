using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1 : Weapon {


    protected float _arrowRotation = 0.0f;



    public Weapon1(Player _player) : base(_player)
    {

    }

    public override void Update()
    {
        _arrowRotation += Time.deltaTime * 30.0f;
        HandleDisplay();
    }



    public override void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
            Fire1();
        else if (Input.GetMouseButtonDown(1))
            Fire2();
    }

    protected override void Fire1()
    {
        ShootBullet();

    }
    protected override void Fire2()
    {

    }
    protected void ShootBullet()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = CameraCtrl.cmr.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Physics.Raycast(ray, out hit);
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForceAtPosition(ray.direction * 50000.0f, hit.point);
            player.StartCoroutine(fadeHitMaterial(hit));
            float distance01 = Mathf.Clamp(100.0f - hit.distance * 0.5f, 50.0f, 100.0f) * 0.01f;
            player.minorSound.PlaySound(player.wpnGui.hitSound, distance01, 0.0f);
        }

        GameObject shot = Object.Instantiate(player.wpnGui.W1_Shot.gameObject);
        shot.transform.position = player.transform.position;
        shot.transform.rotation = Quaternion.LookRotation(ray.direction);
        shot.GetComponent<Weapon1_Shot>().distance = hit.distance;
        shot.GetComponent<Attack>().owner = player;
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

            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f * _smoothDistance, y * 16 - x * multi * 0.5f * _smoothDistance,
                                     x * multi * _smoothDistance, x * multi * _smoothDistance), player.wpnGui.W1_distanceDisplay);


            Matrix4x4 defaultMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(_arrowRotation, new Vector2(x * 16, y * 16));
            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f, y * 16 - x * multi * 0.5f,
                                     x * multi, x * multi), player.wpnGui.W1_arrows);

            multi = _size + Mathf.Cos(Time.time * 10.0f) * 0.3f;

            GUIUtility.RotateAroundPivot(_arrowRotation * 3.3f, new Vector2(x * 16, y * 16));
            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f * _smoothDistance, y * 16 - x * multi * 0.5f * _smoothDistance,
                         x * multi * _smoothDistance, x * multi * _smoothDistance), player.wpnGui.W1_distanceDisplaySlow);

            GUI.matrix = defaultMatrix;

            multi = _size + Mathf.Cos(Time.time * 5.0f) * 0.1f;

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
