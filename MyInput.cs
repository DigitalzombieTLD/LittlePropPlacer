using MelonLoader;
using Placemaker;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;


namespace LittlePropPlacer
{
	public static class MyInput
	{
		public static MelonMod thisMod;
		public static KeyCode myKeycode = KeyCode.Space;
		public static void GetInput()
		{
			// Don't accept input before keycodes are loaded from settings
			if(MyModUI.isInitialized)
			{ 			
				


				/*if (Input.GetKeyDown(KeyCode.X))
				{
					//var foundCanvasObjects = UnityEngine.GameObject.FindObjectsOfType<REnder>();

					int count = 0;
						//RenderTexture singleRT;
					MelonLogger.Msg("yoink!");
					
					Texture2D yoink1 = HarmonyMain.worldMaster.texturePngMaster.houseTex.gameTex;
					Texture2D yoink2 = HarmonyMain.worldMaster.texturePngMaster.houseTex.srcTex;
					Texture2D yoink3 = HarmonyMain.worldMaster.texturePngMaster.typeTex.gameTex;
					Texture2D yoink4 = HarmonyMain.worldMaster.texturePngMaster.typeTex.srcTex;


					Il2CppStructArray<byte> bytes1 = UnityEngine.Il2CppImageConversionManager.EncodeToPNG(yoink1);
					Il2CppStructArray<byte> bytes2 = UnityEngine.Il2CppImageConversionManager.EncodeToPNG(yoink2);
					Il2CppStructArray<byte> bytes3 = UnityEngine.Il2CppImageConversionManager.EncodeToPNG(yoink3);
					Il2CppStructArray<byte> bytes4 = UnityEngine.Il2CppImageConversionManager.EncodeToPNG(yoink4);

					string path1 = Application.dataPath + "/../Mods/houseTex.gameTex.png";
					string path2 = Application.dataPath + "/../Mods/houseTex.srcTex.png";
					string path3 = Application.dataPath + "/../Mods/typeTex.gameTex.png";
					string path4 = Application.dataPath + "/../Mods/typeTex.srcTex.png";

					System.IO.File.WriteAllBytes(path1, bytes1);
					System.IO.File.WriteAllBytes(path2, bytes2);
					System.IO.File.WriteAllBytes(path3, bytes3);
					System.IO.File.WriteAllBytes(path4, bytes4);
					

					count++;
					
				}*/

				/*
				if (Input.GetKeyDown(KeyCode.X))
				{
					var foundCanvasObjects = UnityEngine.GameObject.FindObjectsOfType<RenderTexture>();

					int count = 0;

					foreach(RenderTexture singleRT in foundCanvasObjects)
					{
						RenderTexture.active = singleRT;
						Texture2D tex = new Texture2D(singleRT.width, singleRT.height, TextureFormat.RGB24, false);
						tex.ReadPixels(new Rect(0, 0, singleRT.width, singleRT.height), 0, 0);
						RenderTexture.active = null;
												
						Il2CppStructArray<byte> bytes = UnityEngine.Il2CppImageConversionManager.EncodeToPNG(tex);
												
						string path = Application.dataPath + "/../Mods/" + count + ".png";

						System.IO.File.WriteAllBytes(path, bytes);						
						Debug.Log("Saved to " + path);
						count++;
					}
				}*/

				if (Input.GetKeyDown(KeyCode.None))
				{
					Tools.toPNG("MaterialMaster_HouseMaterial", HarmonyMain.worldMaster.materialMaster.houseMaterial.mainTexture);
					Tools.toPNG("MaterialMaster_VoxelMaterial", HarmonyMain.worldMaster.materialMaster.voxelMaterial.mainTexture);
					Tools.toPNG("MaterialMaster_MaterialTexture", HarmonyMain.worldMaster.materialMaster.materialTexture);

					Tools.toPNG("AOBaker_MainTexture", HarmonyMain.worldMaster.aoBaker.material.mainTexture);
					Tools.toPNG("AOBaker_MarchingSquares", HarmonyMain.worldMaster.aoBaker.marchingSquaresTex);
					Tools.toPNG("AOBaker_RenderTexture0", HarmonyMain.worldMaster.aoBaker.renderTexture0);
					Tools.toPNG("AOBaker_RenderTexture1", HarmonyMain.worldMaster.aoBaker.renderTexture1);
				}

				if (Input.GetKeyDown(KeyCode.None))
				{
					var foundStuff = UnityEngine.GameObject.FindObjectsOfType<Renderer>();

					int count = 0;

					foreach(Renderer singleRenderer in foundStuff)
					{
						//MelonLogger.Msg("Material: " + singleRenderer.material.name + " |||||||| Shader: " + singleRenderer.material.shader.name);

						if(singleRenderer.material.shader.name.Contains("House"))
						{
							MelonLogger.Msg("Changing Material: " + singleRenderer.material.name);
							//Tools.toPNG("House_" + count, singleRenderer.material.mainTexture);
							count++;
							//PrefabLoader.shaderObjectMaterial.mainTexture = HarmonyMain.worldMaster.materialMaster.houseMaterial.mainTexture;
							//PrefabLoader.shaderObjectMaterial.mainTexture.wrapMode = HarmonyMain.worldMaster.materialMaster.houseMaterial.mainTexture.wrapMode;
							//PrefabLoader.shaderObjectMaterial.mainTexture.filterMode = HarmonyMain.worldMaster.materialMaster.houseMaterial.mainTexture.filterMode;
							//PrefabLoader.shaderObjectMaterial.mainTexture.dimension = HarmonyMain.worldMaster.materialMaster.houseMaterial.mainTexture.dimension;
							//PrefabLoader.shaderObjectMaterial.mainTexture.anisoLevel = HarmonyMain.worldMaster.materialMaster.houseMaterial.mainTexture.anisoLevel;
							


							singleRenderer.material = PrefabLoader.shaderObjectMaterial;
							//singleRenderer.material.mainTexture = HarmonyMain.worldMaster.materialMaster.materialTexture;*/
						}
					}		
				}

				if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (PropEditor.currentMode != PropEditor.Mode.Game)
					{
						MyModUI.GameMode();
					}
				}

				// Select mode
				if (PropEditor.currentMode == PropEditor.Mode.Select || PropEditor.currentMode == PropEditor.Mode.Delete)
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;

					LayerMask raymask = 1 << 10;

					if (Physics.Raycast(ray, out hit, 2000f, raymask))
					{
						GameObject targetObject = hit.rigidbody.gameObject;
						CustomPrefab targetCustomComponent = targetObject.GetComponent<CustomPrefab>();

						if(targetCustomComponent)
						{
							//MelonLogger.Msg("Highlight because raycast");
							PropEditor.Highlight(targetCustomComponent);
														
							if (Input.GetKeyDown(KeyCode.Mouse0))
							{
								if (PropEditor.currentMode == PropEditor.Mode.Select)
								{
									MelonLogger.Msg("Select component");
									PropEditor.Select(targetCustomComponent);
								}

								if (PropEditor.currentMode == PropEditor.Mode.Delete)
								{
									MelonLogger.Msg("Delete Component");
									PrefabInstancer.DeletePrefab(targetObject, targetCustomComponent);
									PropEditor.ClickEffect();
								}
							}						
						}
					}
				}

				if (PropEditor.currentMode == PropEditor.Mode.Place)
				{
					if (Input.GetKeyDown(KeyCode.Mouse0))
					{
						if (PrefabInstancer.activePrefab)
						{
							PropEditor.Deselect();
							PrefabInstancer.activePrefab = PrefabInstancer.SpawnCurrentlySelected(new Vector3(0, -50, 0), new Quaternion(0, 0, 0, 0), new Vector3(1, 1, 1));
							PropEditor.Select(PrefabInstancer.activePrefab);
							PropEditor.ClickEffect();
						}
					}
				}
			}
		}	
	}
}
