using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon2 : Weapon
{

    protected float _surroundStaticRotation;


    public Weapon2(Player _player) : base(_player)
    {

    }



    public override void Update()
    {
        if (Time.time % 0.7f < 0.2f)
            _surroundStaticRotation += Time.deltaTime * 172.0f;

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
        ShootRocket();
    }


    protected void ShootRocket()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = CameraCtrl.cmr.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Physics.Raycast(ray, out hit);

        if (hit.rigidbody != null)
        {
            //hit.rigidbody.AddExplosionForce(50000.0f, hit.point, 5, 0.2f);
            player.StartCoroutine(fadeHitMaterial(hit));
        }

        GameObject shot = Object.Instantiate(player.wpnGui.W2_Rocket.gameObject);
        shot.transform.position = player.transform.position;
        shot.transform.rotation = Quaternion.LookRotation(ray.direction);
        shot.GetComponent<Attack>().owner = player;
    }

    public override void HandleDisplay()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = CameraCtrl.cmr.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Physics.Raycast(ray, out hit);

        float curDistance = hit.distance;
        if (curDistance == 0.0f)
            curDistance = 1.0f;
        else
        {
            curDistance = Mathf.Clamp(100.0f - curDistance, 10.0f, 100.0f) * 0.01f;
        }

        _curDistance = curDistance;
        _smoothDistance = Mathf.Lerp(_smoothDistance, _curDistance, Time.deltaTime * 5.0f);
    }



    public override void DisplayCrosshair(float x, float y, bool simple)
    {
        if (simple)
        {
            if (player.wpnGui.W1_crossHairFull != null)
                GUI.DrawTexture(new Rect(x * 14, y * 16 - x * _size * 0.5f, x * _size, x * _size), player.wpnGui.W2_crossHairFull);
        }
        else
        {
            float multi = _size * _smoothDistance;

            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f, y * 16 - x * multi * 0.5f,
                                     x * multi, x * multi), player.wpnGui.W2_Cursor);


            multi = _size;
            Matrix4x4 defaultMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(_surroundStaticRotation, new Vector2(x * 16, y * 16));
            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f, y * 16 - x * multi * 0.5f,
                                     x * multi, x * multi), player.wpnGui.W2_surroundStatic);

            GUI.matrix = defaultMatrix;
            GUIUtility.RotateAroundPivot(Mathf.Cos(Time.time * 6.0f) * 10.0f, new Vector2(x * 16, y * 16));

            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f, y * 16 - x * multi * 0.5f,
                                     x * multi, x * multi), player.wpnGui.W2_Wobble);

            GUI.matrix = defaultMatrix;

            multi = _size;

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
