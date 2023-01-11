using MelonLoader;
using UnityEngine;

namespace LittlePropPlacer
{
	public static class PreviewManager
	{
		public static bool isSetup = false;

		public static GameObject previewRendererParent;
		
		public static GameObject previewLightParent;
		public static Light previewLight;

		public static GameObject previewObjectHolder;
		
		public static GameObject previewCameraParent;
		public static Camera previewCamera;

		public static UIObjectPreviewAdvanced livePreview;
		
		public static void Setup()
		{
			if(!isSetup)
			{
				MelonLogger.Msg("Setup preview");
				previewRendererParent = new GameObject("PreviewRenderer");
				previewCameraParent = new GameObject("PreviewCamera");
				previewObjectHolder = new GameObject("PreviewObjectHolder");

				previewLightParent = new GameObject("PreviewLight");
				previewLight = previewLightParent.AddComponent<Light>();
				previewLight.type = LightType.Directional;
				previewLight.intensity = 3;
				previewLight.renderMode = LightRenderMode.Auto;

				//previewLightParent.transform.parent = previewRendererParent.transform;

				previewLightParent.transform.parent = previewCameraParent.transform;

				previewLightParent.transform.localPosition = new Vector3(50,-40, 0);

				previewCameraParent.transform.parent = previewRendererParent.transform;
				previewObjectHolder.transform.parent = previewRendererParent.transform;

				previewCamera = previewCameraParent.AddComponent<Camera>();
				
				previewCamera.enabled = true;
				previewCamera.CopyFrom(Camera.main);
				previewCamera.fieldOfView = 25f;
				previewCamera.clearFlags = CameraClearFlags.SolidColor;
				previewCamera.tag = "PreviewCam";
				previewCamera.nearClipPlane = 0.01f;
				previewCameraParent.transform.localPosition = new Vector3(0, 0, 0);
				
				previewObjectHolder.transform.localPosition = new Vector3(0, 0, 0);

				previewRendererParent.transform.position = new Vector3(0, -15, 0);

				
				livePreview = MyModUI.previewImage.gameObject.AddComponent<UIObjectPreviewAdvanced>();				
				livePreview.Setup();

				

				isSetup = true;
			}
		}				
	}
}
