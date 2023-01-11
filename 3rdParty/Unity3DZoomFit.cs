using MelonLoader;
using System;
using UnityEngine;

namespace LittlePropPlacer
{
	public static class CamZoomFit
	{
		static Bounds GetBound(GameObject go)
		{
			Bounds b = new Bounds(go.transform.position, Vector3.zero);
			var rList = go.GetComponentsInChildren<MeshRenderer>();
			foreach (Renderer r in rList)
			{
				b.Encapsulate(r.bounds);
			}
			return b;
		}

		/// <summary>
		/// Adjust the camera to zoom fit the game object
		/// There are multiple directions to get zoom-fit view of the game object,
		/// if ViewFromRandomDirecion is true, then random viewing direction is chosen
		/// else, the camera's forward direction will be sused
		/// </summary>
		/// <param name="c"> The camera, whose position and view direction will be 
		//                   adjusted to implement zoom-fit effect </param>
		/// <param name="go"> The GameObject which will be zoom-fit. This object may have
		///                   children objects as well </param>
		/// <param name="ViewFromRandomDirecion"> if random viewing direction is chozen. </param>
		public static void ZoomFit(Camera cam, GameObject gameObject, bool ViewFromRandomDirecion = false)
		{
			Bounds b = GetBound(gameObject);
			Vector3 max = b.size;
			float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));

			double Deg2Rad = (3.1415926535897931 * 2) / 360;

			float dist = radius / (Mathf.Sin(cam.fieldOfView * (float)Deg2Rad / 2f));
			dist = dist * 0.6f;

			Vector3 view_direction = ViewFromRandomDirecion ? UnityEngine.Random.onUnitSphere : cam.transform.InverseTransformDirection(Vector3.forward);
						
			Vector3 pos = view_direction * dist + b.center;
			cam.transform.position = pos;
			cam.transform.LookAt(b.center);
		}
	}
}