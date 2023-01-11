using MelonLoader;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace LittlePropPlacer
{
	public static class PrefabInstancer
	{
		public static List<GameObject> customPrefabObjects;
		public static List<CustomPrefab> customPrefabs;

		public static Dictionary<string, GameObject> categoryParents = new Dictionary<string, GameObject>();
		public static GameObject hierarchyParent;

		public static bool isSetup = false;

		public static CustomPrefab activePrefab;
			   
		public static void Setup()
		{
			hierarchyParent = new GameObject();
			hierarchyParent.name = "CustomPrefabs";
			hierarchyParent.transform.position = new Vector3(0, 0, 0);
			hierarchyParent.transform.rotation = new Quaternion(0, 0, 0, 0);

			customPrefabObjects = new List<GameObject>();
			customPrefabs = new List<CustomPrefab>();
			categoryParents = new Dictionary<string, GameObject>();

			

			isSetup = true;
		}
		public static void ClearAllLoadedPrefabs()
		{
			if(!isSetup)
			{
				return;
			}

			foreach(GameObject singlePrefab in customPrefabObjects)
			{
				GameObject.Destroy(singlePrefab);
			}

			customPrefabObjects.Clear();
			customPrefabs.Clear();


			foreach (KeyValuePair<string,GameObject> singleParent in categoryParents)
			{
				GameObject.Destroy(singleParent.Value);
			}

			categoryParents.Clear();
		}

			public static void ClearHoldingPrefab()
		{
			if (activePrefab && activePrefab.gameObject)
			{
				
				GameObject.Destroy(activePrefab.gameObject);
				customPrefabObjects.Remove(activePrefab.gameObject);
				customPrefabs.Remove(activePrefab);

				activePrefab = null;
			}
		}

		public static void MaybeCreateCategoryParent(string category)
		{
			if(!categoryParents.ContainsKey(category))
			{
				categoryParents.Add(category, new GameObject());
				categoryParents[category].transform.parent = hierarchyParent.transform;
				categoryParents[category].transform.localPosition = new Vector3(0, 0, 0);
				categoryParents[category].transform.rotation = new Quaternion(0, 0, 0, 0);
				categoryParents[category].name = category;
			}			
		}

		public static void DeletePrefab(GameObject prefabObject, CustomPrefab customPrefab)
		{
			PropEditor.Deselect();
			customPrefabs.Remove(customPrefab);
			customPrefabObjects.Remove(prefabObject);
			UnityEngine.GameObject.Destroy(prefabObject);
		}

		public static CustomPrefab SpawnCurrentlySelected(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			string name = MyModUI.refSelectProp.selectValue;
			string category = MyModUI.refSelectCategory.selectValue;

			return (SpawnSinglePrefab(name, category, position, rotation, scale, new Color32(255,255,255,255), true, true, true, ""));
		}

		public static CustomPrefab SpawnSinglePrefab(string name, string category, Vector3 position, Quaternion rotation, Vector3 scale, Color32 color, bool visible, bool iskinematic, bool usegravity, string text)
		{
			if(!isSetup)
			{
				Setup();
			}
			
			customPrefabObjects.Add(GameObject.Instantiate<GameObject>(PrefabLoader.GetPrefab(category, name)));			

			if (customPrefabObjects[customPrefabObjects.Count-1])
			{				
				MaybeCreateCategoryParent(category);
			
				customPrefabs.Add(customPrefabObjects[customPrefabObjects.Count - 1].AddComponent<CustomPrefab>());
			
				customPrefabs[customPrefabs.Count - 1].transform.parent = categoryParents[category].transform;
			
				customPrefabs[customPrefabs.Count - 1].Setup(name, category, position, rotation, scale, customPrefabs.Count - 1, color, visible, iskinematic, usegravity, "");
			}
			else
			{				
				customPrefabObjects.RemoveAt(customPrefabObjects.Count - 1);
			}			
			return (customPrefabs[customPrefabs.Count - 1]);
		}		
	}
}
