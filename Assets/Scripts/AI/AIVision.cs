using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AIVision : MonoBehaviour 
{
	RaycastCone cone;
	Camera camera;

	void Awake()
	{
		cone = GetComponent<RaycastCone>();
		camera = GetComponent<Camera>();

		SetupCamera();
	}

	void SetupCamera()
	{
		camera.fieldOfView = cone.fieldOfView;
		camera.farClipPlane = cone.viewRange;
	}
}
