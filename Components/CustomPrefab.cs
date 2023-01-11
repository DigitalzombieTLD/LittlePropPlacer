using MelonLoader;
using System;
using UnityEngine;

namespace LittlePropPlacer
{
	public class CustomPrefab : MonoBehaviour
	{
		public CustomPrefab(IntPtr intPtr) : base(intPtr) { }

		public string category;
		public string prefabName;

		public Vector3 initialPosition;
		public Quaternion initialRotation;
		public Vector3 prefabScale;

		public string text;

		public Color32 someColor;

		public float highlightTimer = 0;

		public Rigidbody thisRigid;
		public MeshRenderer thisRenderer;
			   
		public bool useGravity = false;
		public bool isKinematic = true;

		public Light testLight;
		public GameObject testLightObject;
				
		public bool isVisible = true;
		//private bool placeMode = false;
		public int thisIndexID;

		
		public bool isSelected = false;
		public bool isSetup = false;

		

		public void Setup(string prefabname, string prefabcategory, Vector3 position, Quaternion rotation, Vector3 scale, int indexID, Color32 color, bool visible, bool iskinematic, bool usegravity, string newtext)
		{
			category = prefabcategory;
			prefabName = prefabname;
			thisIndexID = indexID;

			thisRenderer = GetComponent<MeshRenderer>();
			thisRenderer.material.shader = Shader.Find("Placemaker/Debris");
			someColor = color;
			isVisible = visible;
			isKinematic = iskinematic;
			useGravity = usegravity;
			prefabScale = scale;
			text = newtext;

			/*
			testLightObject = new GameObject("PreviewLight");
			testLight = testLightObject.AddComponent<Light>();
			testLight.type = LightType.Directional;
			testLight.intensity = 1;
			testLight.renderMode = LightRenderMode.Auto;
			testLightObject.transform.parent = this.transform;
			testLightObject.transform.localPosition = new Vector3(-10, 0, 0);
			*/


			thisRigid = GetComponent<Rigidbody>();
			thisRigid.useGravity = useGravity;
			thisRigid.isKinematic = isKinematic;

			thisRigid.transform.localScale = prefabScale;
			thisRigid.transform.position = position;
			thisRigid.transform.rotation = rotation;

			isSetup = true;
		}

	

		public void Update()
		{
			if(isSetup)
			{
				
			}
		}
	}
}
