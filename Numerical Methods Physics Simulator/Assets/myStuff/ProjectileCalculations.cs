using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCalculations : MonoBehaviour 
{
	public float gravity = -15f;
	//public LineRenderer aimLine;
	//public Transform firePoint;
	//public GameObject character;
	public float QuadraticEquation(float a, float b, float c, float sign)
	{
		return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
	}
	public void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
	{
		float xT = targetPos.x;
		float yT = targetPos.y;
		float g = -gravity;
		float v1 = Mathf.Pow(xT, 2) * g;
		float v2 = 2 * xT * Mathf.Sin(angle) * Mathf.Cos(angle);
		float v3 = 2 * yT * Mathf.Pow(Mathf.Cos(angle), 2);
		v0 = Mathf.Sqrt(v1 / (v2 - v3));
		time = xT / (v0 * Mathf.Cos(angle));
	}
	public void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
	{
		float xT = targetPos.x;
		float yT = targetPos.y;
		float g = -gravity;
		float b = Mathf.Sqrt(2 * g * h);
		float a = (-0.5f * g);
		float c = -yT;
		float tPlus = QuadraticEquation(a, b, c, 1);
		float tMin = QuadraticEquation(a, b, c, -1);
		time = tPlus > tMin ? tPlus : tMin;

		angle = Mathf.Atan(b * time / xT);
		v0 = b / Mathf.Sin(angle);

	}
	public void DrawPath(LineRenderer aimLine, Transform firePoint,Vector3 direction, float v0, float angle, float time, float step)
	{
		step = Mathf.Max(0.01f, step);
		//float totalTime = 10f;
		aimLine.positionCount = (int)(time / step) + 2;
		int count = 0;
		for (float i = 0; i < time; i += step)
		{
			float z = v0 * Mathf.Cos(angle);
			float y = v0 * i * Mathf.Sin(angle) + (0.5f * gravity * Mathf.Pow(i, 2));
			aimLine.SetPosition(count, firePoint.position + direction * z + Vector3.up * y);
			count++;
		}
		float zFinal = v0 * time * Mathf.Cos(angle);
		float yFinal = v0 * time * Mathf.Sin(angle) + (0.5f * gravity * Mathf.Pow(time, 2));
		aimLine.SetPosition(count, firePoint.position + direction * zFinal + Vector3.up * yFinal);
	}
	public IEnumerator ProjectileMovement(GameObject projectile, Transform firePoint, Vector3 direction, float v0, float angle, float time)
	{
		float t = 0;
		while (t < time)
		{
			float x = v0 * t * Mathf.Cos(angle);
			float y = v0 * t * Mathf.Sin(angle) + (0.5f * (gravity * Mathf.Pow(t, 2)));
			projectile.transform.position = firePoint.position + direction * x + Vector3.up * y;
			t += Time.deltaTime;
			yield return null;
		}
	}
}
