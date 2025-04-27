using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoPoint : MonoBehaviour
{
	public Vector2 size = new Vector2(1f, 1f);
	public Color color = Color.red;

	private void OnDrawGizmos()
	{
		Gizmos.color = color;
		Vector3 center = transform.position;
		Vector3 cubeSize = new Vector3(size.x, size.y, 0.01f); // mỏng theo trục Z
		Gizmos.DrawCube(center, cubeSize);
	}
}
