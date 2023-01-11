using IniParser;
using IniParser.Model;
using MelonLoader;
using System.Collections.Generic;
using System.IO;
using UnhollowerBaseLib;
using UnityEngine;



namespace LittlePropPlacer
{
	public static class SaveLoad
	{
		public static string settingsPath = Application.dataPath + "/../Mods/_PlacedProps/";

		public static void Save(string fileName)
		{
			FileIniDataParser thisIniParser = new FileIniDataParser();	
			IniData iniData = new IniData();
			thisIniParser.Parser.Configuration.AllowCreateSectionsOnFly = true;

			// Header
			SetValueString("Savename","Header", fileName, iniData);
			SetValueInt("PrefabCount", "Header", PrefabInstancer.customPrefabs.Count, iniData);

			// Prefabs
			for(int index = 0; index < PrefabInstancer.customPrefabs.Count-1; index++)
			{
				SavePrefab(index, PrefabInstancer.customPrefabs[index], iniData);
			}
					   
			// Footer
			thisIniParser.WriteFile(settingsPath + fileName + ".placement", iniData);
		}

		public static void Load(string fileName)
		{
			if(!File.Exists(settingsPath + fileName + ".placement"))
			{
				return;
			}

			PrefabInstancer.ClearAllLoadedPrefabs();

			FileIniDataParser thisIniParser = new FileIniDataParser();			
			IniData iniData = new IniData();
			iniData = thisIniParser.ReadFile(settingsPath + fileName + ".placement");

			// Header
			string savename = GetValueString("Savename", "Header", iniData);
			int prefabcount = GetValueInt("PrefabCount", "Header", iniData);

			// Prefabs
			for (int index = 0; index < prefabcount - 1; index++)
			{
				LoadPrefab(index, iniData);
			}

			// Footer			
		}

		public static void SavePrefab(int index, CustomPrefab prefab, IniData iniData)
		{
			SetValueString("Name", "Prefab_" + index, prefab.prefabName, iniData);
			SetValueString("Category", "Prefab_" + index, prefab.category, iniData);
			SetValueVector3("Position", "Prefab_" + index, prefab.initialPosition, iniData);
			SetValueVector3("Rotation", "Prefab_" + index, prefab.initialRotation.eulerAngles, iniData);
			SetValueVector3("Scale", "Prefab_" + index, prefab.prefabScale, iniData);
			SetValueColor32("Color", "Prefab_" + index, prefab.someColor, iniData);
			SetValueBool("Visible", "Prefab_" + index, prefab.isVisible, iniData);
			SetValueBool("isKinematic", "Prefab_" + index, prefab.isKinematic, iniData);
			SetValueBool("UseGravity", "Prefab_" + index, prefab.useGravity, iniData);
			SetValueString("Text", "Prefab_" + index, prefab.text, iniData);
		}

		public static void LoadPrefab(int index, IniData iniData)
		{			
			string name = GetValueString("Name", "Prefab_" + index, iniData);
			string category = GetValueString("Category", "Prefab_" + index, iniData);
			Vector3 position = GetValueVector3("Position", "Prefab_" + index, iniData);
			Vector3 rotation = GetValueVector3("Rotation", "Prefab_" + index, iniData);
			Vector3 scale = GetValueVector3("Scale", "Prefab_" + index, iniData);
			Color32 color = GetValueColor32("Color", "Prefab_" + index, iniData);
			bool visible = GetValueBool("Visible", "Prefab_" + index, iniData);
			bool iskinematic = GetValueBool("isKinematic", "Prefab_" + index, iniData);
			bool usegravity = GetValueBool("UseGravity", "Prefab_" + index, iniData);
			string text = GetValueString("Text", "Prefab_" + index, iniData);


			if (PrefabInstancer.isSetup)
			{
				PrefabInstancer.SpawnSinglePrefab(name, category, position, Quaternion.Euler(rotation), scale, color, visible, iskinematic, usegravity, text);
			}
		}
				

		public static void CheckIfSectionExist(string section, IniData iniData)
		{
			if (!iniData.Sections.ContainsSection(section))
			{				
				iniData.Sections.AddSection(section);
			}
		}

		public static void SetValueString(string name, string section, string value, IniData iniData)
		{
			CheckIfSectionExist(section, iniData);

			if (!iniData[section].ContainsKey(name))
			{
				iniData[section].AddKey(name, value);
			}
			else
			{
				iniData[section][name] = value;
			}
		}

		public static void SetValueFloat(string name, string section, float value, IniData iniData)
		{
			SetValueString(name, section, value.ToString(), iniData);
		}

		public static void SetValueBool(string name, string section, bool value, IniData iniData)
		{
			SetValueString(name, section, value.ToString(), iniData);
		}

		public static void SetValueInt(string name, string section, int value, IniData iniData)
		{
			SetValueFloat(name, section, value, iniData);
		}
				

		public static void SetValueColor32(string name, string section, Color32 color, IniData iniData)
		{			
			int colorElementR = color.r;
			int colorElementG = color.g;
			int colorElementB = color.b;
			int colorElementA = color.a;

			SetValueInt(name + " Red", section, color.r, iniData);
			SetValueInt(name + " Green", section, color.g, iniData);
			SetValueInt(name + " Blue", section, color.b, iniData);
			SetValueInt(name + " Alpha", section, color.a, iniData);
		}

		public static void SetValueVector3(string name, string section, Vector3 vector, IniData iniData)
		{
			float vectorX = vector.x;
			float vectorY = vector.y;
			float vectorZ = vector.z;

			SetValueFloat(name + " X", section, vectorX, iniData);
			SetValueFloat(name + " Y", section, vectorY, iniData);
			SetValueFloat(name + " Z", section, vectorZ, iniData);		
		}


		public static string GetValueString(string name, string section, IniData iniData)
		{	
			return iniData[section][name];			
		}

		public static float GetValueFloat(string name, string section, IniData iniData)
		{	
			return float.Parse(GetValueString(name, section, iniData));		
		}

		public static Vector3 GetValueVector3(string name, string section, IniData iniData)
		{
			float x = GetValueFloat(name + " X", section, iniData);
			float y = GetValueFloat(name + " Y", section, iniData);
			float z = GetValueFloat(name + " Z", section, iniData);

			return new Vector3(x, y, z);		
		}

		public static Color32 GetValueColor32(string name, string section, IniData iniData)
		{
			byte r = byte.Parse(GetValueString(name + " Red", section, iniData));
			byte g = byte.Parse(GetValueString(name + " Green", section, iniData));
			byte b = byte.Parse(GetValueString(name + " Blue", section, iniData));
			byte a = byte.Parse(GetValueString(name + " Alpha", section, iniData));


			return new Color32(r, g, b, a);
		}

		public static bool GetValueBool(string name, string section, IniData iniData)
		{
			return bool.Parse(GetValueString(name, section, iniData));
		}


		public static int GetValueInt(string name, string section, IniData iniData)
		{
			return int.Parse(GetValueString(name, section, iniData));
		}

	}
}
