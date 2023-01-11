using MelonLoader;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace LittlePropPlacer
{
	public static class PropEditor
	{
		public static GameObject selectedPropObject;
		public static CustomPrefab selectedComponent;

		public static SelectionOutlineController selectionController;
		public enum Mode { Game, Place, Select, Delete, MoveX, MoveY, MoveZ, RotateX, RotateY, RotateZ };
		public static Mode currentMode;

		public static GameObject moveGizmoObject;
		public static GameObject moveGizmoCenter;
		public static GameObject moveGizmoXArrow;
		public static GameObject moveGizmoYArrow;
		public static GameObject moveGizmoZArrow;
		public static GameObject moveGizmoXPaddle;
		public static GameObject moveGizmoYPaddle;
		public static GameObject moveGizmoZPaddle;

		public static GameObject rotateGizmoObject;
		public static GameObject rotateGizmoCenter;
		public static GameObject rotateGizmoXArrow;
		public static GameObject rotateGizmoYArrow;
		public static GameObject rotateGizmoZArrow;

		public static Renderer moveGizmoCenterRenderer;
		public static Renderer moveGizmoXArrowRenderer;
		public static Renderer moveGizmoYArrowRenderer;
		public static Renderer moveGizmoZArrowRenderer;
		public static Renderer moveGizmoXPaddleRenderer;
		public static Renderer moveGizmoYPaddleRenderer;
		public static Renderer moveGizmoZPaddleRenderer;

		public static Renderer rotateGizmoCenterRenderer;
		public static Renderer rotateGizmoXArrowRenderer;
		public static Renderer rotateGizmoYArrowRenderer;
		public static Renderer rotateGizmoZArrowRenderer;

		public static bool gotSelection = false;
		public static bool gotHighlight = false;

		public static bool gizmosReady = false;


		public static void Action()
		{		
			if(currentMode == Mode.Place && gotSelection)
			{
				Vector3[] spawnData = GetClickPositionAndNormal();

				if (spawnData[0] != Vector3.zero)
				{
					selectedComponent.transform.position = spawnData[0];
					selectedComponent.transform.rotation = Quaternion.Euler(spawnData[1]);					
				}
			}
		}

		public static void SwitchMode(Mode newMode)
		{
			currentMode = newMode;
		}


		public static void Highlight(CustomPrefab target)
		{
			selectionController.SetTarget(target);								
		}

		public static void Dehighlight()
		{
			selectionController.ClearTargets();
		}

		public static void Deselect()
		{
			Dehighlight();

			if(selectedComponent)
			{
				selectedComponent.isSelected = false;
				selectedPropObject.layer = 10; // raycast enabled
				selectedComponent.initialPosition = selectedComponent.transform.position;
				selectedComponent.initialRotation = selectedComponent.transform.rotation;
			}			
			
			selectedComponent = null;
			selectedPropObject = null;

			gotSelection = false;

			DeactivateMoveGizmo();	
		}
		public static void Select(CustomPrefab target)
		{
			Deselect();

			selectedComponent = target;
			selectedPropObject = target.gameObject;			
			selectedComponent.isSelected = true;
			selectedPropObject.layer = 2; // no raycast

			gotSelection = true;

			ActivateMoveGizmo(target);
		}

		public static void ActivateMoveGizmo(CustomPrefab target)
		{
			LoadGizmos();

			moveGizmoObject.SetActive(true);
			moveGizmoObject.transform.position = selectedComponent.transform.position;
			moveGizmoObject.transform.rotation = selectedComponent.transform.rotation;
		}

		public static void DeactivateMoveGizmo()
		{
			moveGizmoObject.SetActive(false);			
		}

		public static void ClickEffect()
		{
			Vector2 planePos = HarmonyMain.worldMaster.hoverData.dstVert.planePos;

			int height = HarmonyMain.worldMaster.hoverData.dstHeight;
			HarmonyMain.worldMaster.clickEffect.Click(true, planePos, height, Placemaker.VoxelType.Any);
		}

		public static void LoadGizmos()
		{
			if(gizmosReady)
			{
				return;
			}

			moveGizmoObject = GameObject.Instantiate<GameObject>(PrefabLoader.moveGizmoPrefab);
			rotateGizmoObject = GameObject.Instantiate<GameObject>(PrefabLoader.rotateGizmoPrefab);

			moveGizmoCenter = moveGizmoObject.transform.Find("Center").gameObject;
			moveGizmoXArrow = moveGizmoObject.transform.Find("X-Arrow").gameObject;
			moveGizmoYArrow = moveGizmoObject.transform.Find("Y-Arrow").gameObject;
			moveGizmoZArrow = moveGizmoObject.transform.Find("Z-Arrow").gameObject;
			moveGizmoXPaddle = moveGizmoObject.transform.Find("X-Paddle").gameObject;
			moveGizmoYPaddle = moveGizmoObject.transform.Find("Y-Paddle").gameObject;
			moveGizmoZPaddle = moveGizmoObject.transform.Find("Z-Paddle").gameObject;

			rotateGizmoCenter = rotateGizmoObject.transform.Find("Center").gameObject;
			rotateGizmoXArrow = rotateGizmoObject.transform.Find("X-Arrow").gameObject;
			rotateGizmoYArrow = rotateGizmoObject.transform.Find("Y-Arrow").gameObject;
			rotateGizmoZArrow = rotateGizmoObject.transform.Find("Z-Arrow").gameObject;

			moveGizmoCenterRenderer = moveGizmoCenter.GetComponent<Renderer>();
			moveGizmoCenterRenderer.material.shader = Shader.Find("Placemaker/Debris");
			moveGizmoXArrowRenderer = moveGizmoXArrow.GetComponent<Renderer>();
			moveGizmoXArrowRenderer.material.shader = Shader.Find("Placemaker/Debris");
			moveGizmoYArrowRenderer = moveGizmoYArrow.GetComponent<Renderer>();
			moveGizmoYArrowRenderer.material.shader = Shader.Find("Placemaker/Debris");
			moveGizmoZArrowRenderer = moveGizmoZArrow.GetComponent<Renderer>();
			moveGizmoZArrowRenderer.material.shader = Shader.Find("Placemaker/Debris");
			moveGizmoXPaddleRenderer = moveGizmoXPaddle.GetComponent<Renderer>();
			moveGizmoXPaddleRenderer.material.shader = Shader.Find("Placemaker/Debris");
			moveGizmoYPaddleRenderer = moveGizmoYPaddle.GetComponent<Renderer>();
			moveGizmoYPaddleRenderer.material.shader = Shader.Find("Placemaker/Debris");
			moveGizmoZPaddleRenderer = moveGizmoZPaddle.GetComponent<Renderer>();
			moveGizmoZPaddleRenderer.material.shader = Shader.Find("Placemaker/Debris");

			rotateGizmoCenterRenderer = rotateGizmoCenter.GetComponent<Renderer>();
			rotateGizmoCenterRenderer.material.shader = Shader.Find("Placemaker/Debris");
			rotateGizmoXArrowRenderer = rotateGizmoXArrow.GetComponent<Renderer>();
			rotateGizmoXArrowRenderer.material.shader = Shader.Find("Placemaker/Debris");
			rotateGizmoYArrowRenderer = rotateGizmoYArrow.GetComponent<Renderer>();
			rotateGizmoYArrowRenderer.material.shader = Shader.Find("Placemaker/Debris");
			rotateGizmoZArrowRenderer = rotateGizmoZArrow.GetComponent<Renderer>();
			rotateGizmoZArrowRenderer.material.shader = Shader.Find("Placemaker/Debris");

			
			moveGizmoObject.SetActive(false);
			rotateGizmoObject.SetActive(false);

			gizmosReady = true;
		}

		public static Vector3[] GetClickPositionAndNormal()
		{
			Vector3[] returnData = new Vector3[] { Vector3.zero, Vector3.zero }; //0 = spawn positon, 1 = surface normal
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit, 500f))
			{
				returnData[0] = hit.point;
				returnData[1] = hit.normal;
			}

			return returnData;
		}
	}
}
