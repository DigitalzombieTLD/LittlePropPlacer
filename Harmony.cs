using HarmonyLib;
using MelonLoader;
using Placemaker;
using UnityEngine;
using Placemaker.Ui;
using static Placemaker.BootGameCore;

namespace LittlePropPlacer
{
	public static class HarmonyMain
	{
		public static Vector3 mousePosition;
		public static Vector3 colliderOffset = new Vector3(0, 0.1f, 0);
		public static WorldMaster worldMaster;
		//public static bool mouseLightInit = false;
		//public static GameObject mouseLight;
		//public static Light mouseLightLight;
		public static Texture2D fooTexture;

		[HarmonyPatch(typeof(Dim), "SetState")]
		public class StartFinishLoadingPatch
		{
			public static void Postfix(ref Dim __instance, ref Dim.State state)
			{
				if (state == Dim.State.Clear)
				{
					MelonLogger.Msg("Setting screen clear");
					//PropEditor.selectionController = Camera.main.gameObject.AddComponent<SelectionOutlineController>();
					PropEditor.selectionController = worldMaster.uiMaster.orbitalCamera.cam.gameObject.AddComponent<SelectionOutlineController>();
					//PropEditor.selectionController = PreviewManager.previewCamera.gameObject.AddComponent<SelectionOutlineController>();
					//PropEditor.selectionController.Init();
					PropEditor.LoadGizmos();
				}
			}
		}


		[HarmonyPatch(typeof(HoverData), "SetHover")]
		public class SetHoverGetCoords
		{
			public static void Postfix(ref HoverData __instance)
			{
				mousePosition = __instance.pointerHitPos;

				/*
				if (!mouseLightInit)
				{
					mouseLight = new GameObject("MouseLight");
					mouseLightLight = mouseLight.AddComponent<Light>();
					mouseLightLight.type = LightType.Point;
					mouseLightLight.intensity = 1;
					mouseLightLight.renderMode = LightRenderMode.Auto;
					mouseLightInit = true;
				}
				*/
				//mouseLight.transform.position = mousePosition;
			}
		}

		[HarmonyPatch(typeof(ShadowBaker), "OnStart")]
		public class ShadowBakerPAtch
		{
			public static void Prefix(ref ShadowBaker __instance)
			{
				//MelonLogger.Msg("Replacing shadow shader");
				//__instance.shader = PrefabLoader.shaderObjectShader;
				MelonLogger.Msg("ShadowBaker prefix");

				//__instance.shader = PrefabLoader.shaderObjectShader;
			}

			public static void Postfix(ref ShadowBaker __instance)
			{
				worldMaster = __instance.master;


				//MelonLogger.Msg("Replacing shadow shader");
				//__instance.shader = PrefabLoader.shaderObjectShader;
				MelonLogger.Msg("ShadowBakerLoading!!!");
				PreviewManager.Setup();
				MyModUI.selectedCategoryIndex = 0;
				MyModUI.selectedPrefabIndex = 0;
				MyModUI.UpdatePreview();
			}

		}

		/*[HarmonyPatch(typeof(AoBaker), "OnStart")]
		public class aoPAtch2
		{
			public static void Prefix(ref AoBaker __instance)
			{
				MelonLogger.Msg("AO Prefix");

				__instance.shader = PrefabLoader.shaderObjectShader;
			}
		}*/

		[HarmonyPatch(typeof(MaterialMaster), "OnStart")]
		public class MaterialMasterPatch
		{
			public static void Prefix(ref MaterialMaster __instance)
			{
				MelonLogger.Msg("MaterialMasterLoading!!!");
				fooTexture = __instance.materialTexture;

				//Texture mainTex = __instance.houseMaterial.GetTexture("_MainTex"); //_MainTex("Texture", 2D) = "white" { }
				//Texture typeTex = __instance.houseMaterial.GetTexture("_TypeTex"); //_TypeTex("Type Texture", 2D) = "white" { }

				//__instance.houseMaterial.shader = PrefabLoader.shaderObjectShader;

				//__instance.houseMaterial.SetTexture("_MainTex", mainTex);
				//__instance.houseMaterial.SetTexture("_TypeTex", typeTex);
			}
		}

		[HarmonyLib.HarmonyPatch(typeof(MasterClicker), "AddClick")]
		public class AddVoxelPatcher
		{
			public static bool Prefix(ref MasterClicker __instance)
			{
				if (PropEditor.currentMode != PropEditor.Mode.Game)
				{
					return false;
				}

				return true;
			}
		}

		[HarmonyLib.HarmonyPatch(typeof(MasterClicker), "RemoveClick")]
		public class RemoveVoxelPatcher
		{
			public static bool Prefix(ref MasterClicker __instance)
			{
				// Place mode
				if (PropEditor.currentMode != PropEditor.Mode.Game)
				{
					return false;
				}

				return true;
			}
		}
	}
}
