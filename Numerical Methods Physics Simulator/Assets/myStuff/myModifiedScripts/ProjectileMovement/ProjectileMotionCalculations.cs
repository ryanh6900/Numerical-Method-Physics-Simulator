using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotionCalculations : MonoBehaviour 
{
	public float gravity = -15f;
	//public LineRenderer aimLine;
	//public Transform firePoint;
	//public GameObject character;
	public float FindLandingTime(Vector3 targetPos, float angle, Vector2 velOnJump, float initialHeight)
	{
		float timeOfLanding;
		float yFinal = targetPos.y;//should be zero 
		float vY0 = velOnJump.y;
		float vZ0 = velOnJump.x; //technically we dont need these as we just need to find time when projectile hits ground.
		float g = -gravity;
		float a = (-0.5f * g);
		float b = vY0;
		float c = initialHeight;
		float tPlus = QuadraticEquation(a, b, c, 1);
		float tMin = QuadraticEquation(a, b, c, -1);
		timeOfLanding = tPlus > tMin ? tPlus : tMin;
		Debug.Log("Time Of Landing = " + timeOfLanding);
		return timeOfLanding;
	}
	public float FindTimeToApex(Vector3 targetPos, float angle, Vector2 velOnJump, float initialHeight)
	{
		float timeOfApex = velOnJump.y*Mathf.Sin(angle)/gravity;
		Debug.Log("Time of Apex = "+timeOfApex);
		return timeOfApex;
	}
	public float QuadraticEquation(float a, float b, float c, float sign)
	{
		return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
	}

	public void CalculatePathWithHeight(Vector3 targetPos, float h, ref Vector2 v0, out float angle, out float time)
	{
		float zT = targetPos.z;
		float yT = targetPos.y;
		float g = -gravity;
		float b = Mathf.Sqrt(2 * g * h);
		float a = (-0.5f * g);
		float c = -yT;
		float tPlus = QuadraticEquation(a, b, c, 1);
		Debug.Log(tPlus);
		float tMin = QuadraticEquation(a, b, c, -1);
		Debug.Log(tMin);
		time = tPlus > tMin ? tPlus : tMin;
		Debug.Log("Time Calculate Path with Height: " + time);
		angle = Mathf.Atan(b * time / zT);
		v0.y = b / Mathf.Sin(angle);
	}
	public void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
	{
		float zT = targetPos.z;
		float yT = targetPos.y;
		float g = -gravity;
		float v1 = Mathf.Pow(zT, 2) * g;
		float v2 = 2 * zT * Mathf.Sin(angle) * Mathf.Cos(angle);
		float v3 = 2 * yT * Mathf.Pow(Mathf.Cos(angle), 2);
		v0 = Mathf.Sqrt(v1 / (v2 - v3));
		time = zT / (v0 * Mathf.Cos(angle));
		Debug.Log("Time from calculate path: " + time);
	}
	
	public void DrawPath(LineRenderer aimLine, Transform firePoint,Vector3 direction, Vector2 currVelocity, float angle, float time, float step)
	{
		step = Mathf.Max(0.01f, step);
		time = Mathf.Abs(time);
		aimLine.positionCount = (int)(time / step) + 2;
		int count = 0;
		for (float i = 0; i < time; i += step)
		{
			float z = currVelocity.magnitude * Mathf.Cos(angle);
			float y = currVelocity.magnitude * i * Mathf.Sin(angle) - (0.5f * -gravity * Mathf.Pow(i, 2));
			//aimLine.SetPosition(count, firePoint.position + direction * z + Vector3.up * y);
			aimLine.SetPosition(count, firePoint.position + direction *z + Vector3.up * y);
			count++;
		}
		float zFinal = currVelocity.magnitude * time * Mathf.Cos(angle);
		float yFinal = currVelocity.magnitude * time * Mathf.Sin(angle) - (0.5f * -gravity * Mathf.Pow(time, 2));
		aimLine.SetPosition(count, firePoint.position + direction * zFinal + Vector3.up * yFinal);
	}
	public IEnumerator ProjectileMotionMovement(GameObject projectile, Transform firePoint, Vector3 direction, Vector2 currVelocity, float angle, float time)
	{
		float t = 0;
		while (t < time)
		{
			float x = currVelocity.magnitude * t * Mathf.Cos(angle);
			float y = currVelocity.magnitude * t * Mathf.Sin(angle) + (0.5f * (gravity * Mathf.Pow(t, 2)));
			projectile.transform.position = firePoint.position + direction * x + Vector3.up * y;
			t += Time.deltaTime;
			yield return null;
		}

		Debug.Log("Projectile Coroutine executed.");
	}
}
