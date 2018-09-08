using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class FOWMesh : MonoBehaviour {

	FOW fow;
	Mesh mesh;
	Mesh shadowMesh;
	RaycastHit2D hit;
	[SerializeField] float meshRes = 2;
	[HideInInspector] public Vector3[] vertices;
	[HideInInspector] public int[] triangles;
	[HideInInspector]  public int stepCount;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		fow = GetComponentInParent<FOW>();
		
	}
	void LateUpdate(){
		MakeMesh();
	}

	void MakeMesh(){
		int stepCount = Mathf.RoundToInt(fow.viewAngle * meshRes);
		float stepAngle = stepCount / fow.viewAngle;
		List<Vector3> viewVertex = new List<Vector3>();
		hit = new RaycastHit2D();
		for(int i = 0; i<=stepCount; i++){
			float angle = fow.transform.eulerAngles.y - fow.viewAngle / 2 + stepAngle * i;
			Vector3 dir = fow.DirFromAngle(angle, false);
			hit = Physics2D.Raycast(fow.transform.position, dir, fow.viewRadius, fow.obstacleMask);
			if(hit.collider == null){
				viewVertex.Add(transform.position + dir.normalized * fow.viewRadius);
			}else{
				viewVertex.Add(transform.position + dir.normalized * hit.distance);
			}
		}

		int vertexCount = viewVertex.Count + 1;

		vertices = new Vector3[vertexCount];
		triangles = new int[(vertexCount - 2) * 3];
		vertices[0] = Vector3.zero;
		for(int i = 0; i<vertexCount - 1; i++){
			vertices[i + 1] = transform.InverseTransformPoint(viewVertex[i]);
			if(i < vertexCount - 2){
				//triangles[i*3 + 3] = 0;
				triangles[i*3 + 2] = i + 1;
				triangles[i*3] = i + 2;
			}
			
		}
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}

}
