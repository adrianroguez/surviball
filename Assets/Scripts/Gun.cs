using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Gun : MonoBehaviour
{
    public float range = 20f;
    public float fireDelay = 0.2f;
    public Transform spawn;

    private LineRenderer laserLine;
    private float nextFireTime = 0f;
    private float shotDuration = 0.05f;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
    }

    public void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireDelay;
            FireRaycast();
        }
    }

    void FireRaycast()
    {
        StartCoroutine(ShotEffect());

        Ray ray = new Ray(spawn.position, spawn.forward);
        RaycastHit hit;

        laserLine.SetPosition(0, spawn.position);

        if (Physics.Raycast(ray, out hit, range))
        {
            laserLine.SetPosition(1, hit.point);
            
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
        else
        {
            laserLine.SetPosition(1, ray.origin + ray.direction * range);
        }
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(shotDuration);
        laserLine.enabled = false;
    }
}