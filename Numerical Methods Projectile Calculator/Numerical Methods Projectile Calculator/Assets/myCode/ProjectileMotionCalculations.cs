using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotionCalculations : MonoBehaviour
{
    [SerializeField] float initialVelocity;
    [SerializeField] float projectileAngle;
    [SerializeField] private GameObject projectile;
    public float gravity = -15f;
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            float angle = projectileAngle* Mathf.Deg2Rad;
            StopAllCoroutines();
            StartCoroutine(ProjectileMovement(initialVelocity,angle));
        }
    }
    public IEnumerator ProjectileMovement(float v0, float angle)
    {
        float t= 0;
        while(t<100)
        {
            if(projectile.GetComponent<CharacterController>().isGrounded)
            {
            float x = v0*t*Mathf.Cos(angle);
            float y = v0*t *Mathf.Sin(angle);
            projectile.transform.position = new Vector3(x,y,0);
            t += Time.deltaTime;
            yield return null;
            }
            
        }
    }
}
