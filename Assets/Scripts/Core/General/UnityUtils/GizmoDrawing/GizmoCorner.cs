using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GizmoCorner : MonoBehaviour
{
	public enum Corner
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
	}

	public float size = 0.5f; // độ dài cạnh của góc
	public Color color = Color.green;
	public float thickness = 3f;
	public Corner corner;

	private void OnDrawGizmos()
	{
		Handles.color = color;

		Vector3 pos = transform.position;

		switch (corner)
		{
			case Corner.TopLeft:
				// Vẽ một góc vuông ở góc dưới trái
				Handles.DrawAAPolyLine(thickness, pos, pos + Vector3.right * size);     // ngang
				Handles.DrawAAPolyLine(thickness ,pos, pos + Vector3.down * size);        // dọc
				break;
			case Corner.TopRight:
				// Vẽ một góc vuông ở góc dưới trái
				Handles.DrawAAPolyLine(thickness, pos, pos + Vector3.left * size);     // ngang
				Handles.DrawAAPolyLine(thickness, pos, pos + Vector3.down * size);        // dọc
				break;
			case Corner.BottomLeft:
				// Vẽ một góc vuông ở góc dưới trái
				Handles.DrawAAPolyLine(thickness, pos, pos + Vector3.right * size);     // ngang
				Handles.DrawAAPolyLine(thickness, pos, pos + Vector3.up * size);        // dọc
				break;
			case Corner.BottomRight:
				// Vẽ một góc vuông ở góc dưới trái
				Handles.DrawAAPolyLine(thickness, pos, pos + Vector3.left * size);     // ngang
				Handles.DrawAAPolyLine(thickness, pos, pos + Vector3.up * size);        // dọc
				break;
		}
	}
}
