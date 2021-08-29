using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class UVMapping : MonoBehaviour
{
    [SerializeField] Material uvMaterial;
    void Start()
    {
		Vector2[] uvs = {
			new Vector2(0, 0.66f),
			new Vector2(0.25f, 0.66f),
			new Vector2(0, 0.33f),
			new Vector2(0.25f, 0.33f),

			new Vector2(0.5f, 0.66f),
			new Vector2(0.5f, 0.33f),
			new Vector2(0.75f, 0.66f),
			new Vector2(0.75f, 0.33f),

			new Vector2(1, 0.66f),
			new Vector2(1, 0.33f),

			new Vector2(0.25f, 1),
			new Vector2(0.5f, 1),

			new Vector2(0.25f, 0),
			new Vector2(0.5f, 0),
		};

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.uv = uvs;
		mesh.Optimize();
		mesh.RecalculateNormals();
	}
}
