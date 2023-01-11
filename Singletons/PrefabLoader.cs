using MelonLoader;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace LittlePropPlacer
{
	public static class PrefabLoader
	{
		//public static Dictionary<string, PropCategory> prefabCategories = new Dictionary<string, PropCategory>();

		public static Dictionary<string, Dictionary<string, GameObject>> prefabs;
		public static GameObject shaderObject;
		public static Renderer shaderObjectRenderer;
		public static Material shaderObjectMaterial;
		public static Shader shaderObjectShader;
		public static Texture shaderObjectTexture;

		public static GameObject outlinecube;
		public static GameObject targetcube;

		public static Material outlinecubemat;
		public static Material targetcubemat;

		public static Shader outlineshader;
		public static Shader targetshader;

		public static GameObject moveGizmoPrefab;		
		public static GameObject rotateGizmoPrefab;	


		public static void LoadAllPropsFromBundle()
		{
			prefabs = new Dictionary<string, Dictionary<string, GameObject>>();
			
			Il2CppStringArray prefabNames = LittlePropPlacerMain.propBundle.GetAllAssetNames();

			foreach(string singlePrefab in prefabNames)
			{
				if (singlePrefab.Contains("_item_"))
				{					
					string[] nameSplit1 = singlePrefab.Split('/');
					string[] nameSplit2 = nameSplit1[nameSplit1.Length - 1].Split('.');
					string[] nameSplit3 = nameSplit2[0].Split('_');

					MaybeAddPrefab(nameSplit3[0], nameSplit3[2]);
				}					
				else if(singlePrefab.Contains("cube_standard"))
				{
					shaderObject = LittlePropPlacerMain.propBundle.LoadAsset<GameObject>("cube_standard");
					shaderObjectRenderer = shaderObject.GetComponent<MeshRenderer>();
					shaderObjectMaterial = shaderObjectRenderer.material;
					shaderObjectShader = shaderObjectMaterial.shader;
					shaderObjectTexture = shaderObjectRenderer.material.mainTexture;
				}
				else if(singlePrefab.Contains("shaderoutlinecube"))
				{
					outlinecube = LittlePropPlacerMain.propBundle.LoadAsset<GameObject>("shaderoutlinecube");
					outlinecubemat = outlinecube.GetComponent<Renderer>().material;
					outlineshader = outlinecubemat.shader;
					UnityEngine.GameObject.DontDestroyOnLoad(outlinecube);
				}
				else if (singlePrefab.Contains("shadertargetcube"))
				{
					targetcube = LittlePropPlacerMain.propBundle.LoadAsset<GameObject>("shadertargetcube");
					targetcubemat = targetcube.GetComponent<Renderer>().material;
					targetshader = targetcubemat.shader;
					UnityEngine.GameObject.DontDestroyOnLoad(targetcube);
				}
				else if (singlePrefab.Contains("move-gizmo"))
				{
					moveGizmoPrefab = LittlePropPlacerMain.propBundle.LoadAsset<GameObject>("move-gizmo");
					MelonLogger.Msg("Got the move gizmo");
					UnityEngine.GameObject.DontDestroyOnLoad(moveGizmoPrefab);
				}
				else if (singlePrefab.Contains("rotate-gizmo"))
				{
					rotateGizmoPrefab = LittlePropPlacerMain.propBundle.LoadAsset<GameObject>("rotate-gizmo");
					MelonLogger.Msg("Got the rotate gizmo");
					UnityEngine.GameObject.DontDestroyOnLoad(rotateGizmoPrefab);
				}
			}			
		}

		public static GameObject GetPrefab(string category, string prefabname)
		{			
			if (!prefabs.ContainsKey(category))
			{
				return null;
			}

			if(!prefabs[category].ContainsKey(prefabname))
			{
				return null;
			}

			return prefabs[category][prefabname];
		}

		public static void MaybeCreateCategory(string newCategory)
		{
			if(!prefabs.ContainsKey(newCategory))
			{				
				prefabs.Add(newCategory, new Dictionary<string, GameObject>());
			}			
		}

		public static void MaybeAddPrefab(string category, string prefabname)
		{
			MaybeCreateCategory(category);
			
			if (!prefabs[category].ContainsKey(prefabname))
			{
				prefabs[category].Add(prefabname, LittlePropPlacerMain.propBundle.LoadAsset<GameObject>(category + "_Item_" + prefabname));				
			}
		}			
	}
}
