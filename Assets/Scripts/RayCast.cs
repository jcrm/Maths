using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {
	public float distance = 50f;
	LineRenderer line;
	public Material lineMaterial;
	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		line.SetVertexCount(2);
		line.renderer.material = lineMaterial;
		line.SetWidth(0.1f, 0.25f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, distance)){
				if(hit.collider.CompareTag("Objects")){
					//draw invisible ray cast/vector
					Debug.DrawLine (ray.origin, hit.point);
					Debug.DrawRay(ray.origin, hit.point, Color.red, 10);
					//log hit area to the console
					Debug.Log(hit.point);
					line.enabled = true;
					line.SetPosition(0, transform.position);
					line.SetPosition(1, hit.point + hit.normal);
				}
			}
			/*Vector3 fwd = transform.TransformDirection (Vector3.forward);
			Vector3 pos = transform.position;
			pos.y +=0.7f;
			if (Physics.Raycast (pos, fwd, 100)){
					print ("There is something in front of the object!");
			}else{
				print ("There is not something in front of the object!");
			}
			Debug.DrawRay(pos, fwd, Color.red);*/
		}
	}
}