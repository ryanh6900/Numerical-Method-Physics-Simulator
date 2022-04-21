using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float gravity = 20f;

    //IEnumerator ProjectileMovement(float v0, float angle)
    //{
    //    float t = 0;
    //    while (t < 100)
    //    {
    //        float x = v0 * t * Mathf.Cos(angle);
    //        float y = v0 * t *Mathf.Sin(angle) - (1f/2f)* (-gravity*Mathf.Pow(t,2));
    //        transform.position = new Vector3(x,y,0);
    //        t += Time.deltaTime;

    //    }
    //}
}
