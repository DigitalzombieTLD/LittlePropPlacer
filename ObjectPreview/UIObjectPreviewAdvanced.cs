using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace LittlePropPlacer
{
	public class UIObjectPreviewAdvanced : MonoBehaviour
	{
		public UIObjectPreviewAdvanced(IntPtr intPtr) : base(intPtr) { }
		
		//rotationSpeed controls how fast the object will rotate if the mouse is over this component.
		public float rotationSpeed = 50f;

		public static RenderTexture previewRenderTexture;

		public bool isSetup = false;
				
		public GameObject gameObjectToPreview;
		public Rigidbody rbToPreview;
		public MeshRenderer rendererToPreview;

		public Vector3 positionVector = new Vector3(0, -0.15f, 0.3f);
		public Quaternion rotationVector = new Quaternion(0,0,0,0);
		public Vector3 scaleVector = new Vector3(1, 1, 1);
		

		public void Setup()
		{
			previewRenderTexture = new RenderTexture(512, 512, 24);			
			PreviewManager.previewCamera.targetTexture = previewRenderTexture;
			MyModUI.previewImage.texture = previewRenderTexture; 

			isSetup = true;
		}
				

		private void FixedUpdate()
		{
			if(isSetup)
			{
				Turn();
			}
						
		}

		public void SwitchObjectToPreview(GameObject objectToPreview)
		{
			if(!isSetup)
			{
				return;
			}

			MaybeDestroyCurrentObject();

			gameObjectToPreview = Instantiate(objectToPreview, PreviewManager.previewObjectHolder.transform , false);

			gameObjectToPreview.transform.localScale = scaleVector;
			gameObjectToPreview.transform.localPosition = positionVector;
			gameObjectToPreview.transform.localRotation = rotationVector;

			rbToPreview = gameObjectToPreview.GetComponent<Rigidbody>();
			rbToPreview.useGravity = false;
			rbToPreview.isKinematic = false;
			rbToPreview.maxAngularVelocity = 1;
			rbToPreview.interpolation = RigidbodyInterpolation.Interpolate;

			//rendererToPreview = gameObjectToPreview.GetComponent<MeshRenderer>();
			//rendererToPreview.material.shader = Shader.Find("Placemaker/Debris");

			CamZoomFit.ZoomFit(PreviewManager.previewCamera, gameObjectToPreview, false);
			
		}

		public void MaybeDestroyCurrentObject()
		{
			if(gameObjectToPreview)
			{
				UnityEngine.GameObject.Destroy(gameObjectToPreview);
				rbToPreview = null;
			}
		}

		void Turn()
		{			
			if (gameObjectToPreview != null)
			{
				rbToPreview.AddTorque(Vector3.up);
			}
			else
			{
			
			}
		}		
	}
}
