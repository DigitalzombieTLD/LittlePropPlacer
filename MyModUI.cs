using MelonLoader;
using UnityEngine;
using ModUI;
using System;
using System.Linq;
using UnityEngine.UI;
using UnhollowerRuntimeLib;
using UnityEngine.Events;

namespace LittlePropPlacer
{
	public static class MyModUI
	{
		public static MelonMod myMod;
		public static ModSettings myModSettings;

		public static ButtonVeryBig previewButton;
		public static RawImage previewImage;
		

		public static bool isInitialized;

		public static SelectionButton refSelectCategory;
		public static SelectionButton refSelectProp;

		public static CustomPrefab holdingPrefab;
		
		public static int selectedCategoryIndex = 0;
		public static int selectedPrefabIndex = 0;

		public static void Initialize(MelonMod thisMod)
		{			
			ModSettings myModSettings = UIManager.Register(thisMod, new Color32(255, 179, 174, 255));

			myModSettings.AddButton("Place", "Modes", new Color32(255, 179, 174, 255), new Action(delegate { PlaceMode(); }));
			myModSettings.AddButton("Select", "Modes", new Color32(255, 179, 174, 255), new Action(delegate { SelectMode(); }));
			myModSettings.AddButton("Delete", "Modes", new Color32(255, 179, 174, 255), new Action(delegate { DeleteMode(); }));
			myModSettings.AddButton("Game", "Modes", new Color32(255, 179, 174, 255), new Action(delegate { GameMode(); }));

			refSelectCategory = myModSettings.AddSelectionButton("Category", "Props", new Color32(255, 179, 174, 255), new Action(delegate { PrevCategory(); }), new Action(delegate { NextCategory(); }), "Category");
			refSelectProp = myModSettings.AddSelectionButton("Prop", "Props", new Color32(255, 179, 174, 255), new Action(delegate { PrevPrefab(); }), new Action(delegate { NextPrefab(); }), "Prop");

			previewButton = myModSettings.AddButtonVeryBig("PropPreview", "Props", new Color32(255, 179, 174, 255), new Action(delegate { MyButtonActionExample(); }));
			previewImage = previewButton.transform.Find("Pic").GetComponent<RawImage>();

			//UIManager.registeredMods[thisMod].GetComponent<ButtonBig>().thisButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate { PreviewManager.Setup(); })));

			myModSettings.AddButton("Save", "Settings", new Color32(255, 179, 174, 255), new Action(delegate { SaveLoad.Save("testprefabsave"); }));
			myModSettings.AddButton("Load", "Settings", new Color32(255, 179, 174, 255), new Action(delegate { SaveLoad.Load("testprefabsave"); }));
			myModSettings.AddButton("Clear All Props", "Settings", new Color32(255, 179, 174, 255), new Action(delegate { PrefabInstancer.ClearAllLoadedPrefabs(); }));

			myModSettings.AddToggle("Autoload on Start", "Settings", new Color32(255, 179, 174, 255),false, new Action<bool>(delegate (bool value) { MyToggleActionExample(value); }));

			isInitialized = true;
		}

		public static void PlaceMode()
		{
			PropEditor.Deselect();
			PrefabInstancer.ClearHoldingPrefab();			
			PrefabInstancer.activePrefab = PrefabInstancer.SpawnSinglePrefab(refSelectProp.selectValue, refSelectCategory.selectValue, new Vector3(0, -50, 0), new Quaternion(0, 0, 0, 0), new Vector3(1, 1, 1), new Color32(255, 255, 255, 255), true, true, true, "");
			PropEditor.Select(PrefabInstancer.activePrefab);
			PropEditor.SwitchMode(PropEditor.Mode.Place);
		}

		public static void SelectMode()
		{
			PropEditor.Deselect();
			PrefabInstancer.ClearHoldingPrefab();			
			PropEditor.SwitchMode(PropEditor.Mode.Select);
		}

		public static void DeleteMode()
		{
			PropEditor.Deselect();
			PrefabInstancer.ClearHoldingPrefab();			
			PropEditor.SwitchMode(PropEditor.Mode.Delete);
		}

		public static void GameMode()
		{
			PropEditor.Deselect();
			PrefabInstancer.ClearHoldingPrefab();
			
			PropEditor.SwitchMode(PropEditor.Mode.Game);
		}
			

		public static void UpdatePreview()
		{
			refSelectCategory.selectValue = PrefabLoader.prefabs.ElementAt(selectedCategoryIndex).Key;
			refSelectProp.selectValue = PrefabLoader.prefabs[refSelectCategory.selectValue].ElementAt(selectedPrefabIndex).Key;
			refSelectCategory.UpdateSettingsValue();
			refSelectProp.UpdateSettingsValue();

			if (!PreviewManager.isSetup)
				return;

			PrefabInstancer.ClearHoldingPrefab();		
			PreviewManager.livePreview.SwitchObjectToPreview(PrefabLoader.GetPrefab(refSelectCategory.selectValue, refSelectProp.selectValue));

			if (PropEditor.currentMode == PropEditor.Mode.Place)
			{				
				PrefabInstancer.activePrefab = PrefabInstancer.SpawnCurrentlySelected(new Vector3(0, -50, 0), new Quaternion(0, 0, 0, 0), new Vector3(1, 1, 1));
				PropEditor.Select(PrefabInstancer.activePrefab);
			}
		}	

		public static void PrevCategory()
		{
			if (!PreviewManager.isSetup)
				return;

			if (selectedCategoryIndex == 0)
			{
				selectedCategoryIndex = PrefabLoader.prefabs.Count - 1;
			}
			else
			{
				selectedCategoryIndex--;
			}

			selectedPrefabIndex = 0;
			UpdatePreview();
		}		

		public static void NextCategory()
		{
			if (!PreviewManager.isSetup)
				return;

			if (selectedCategoryIndex < PrefabLoader.prefabs.Count - 1)
			{
				selectedCategoryIndex++;				
			}
			else
			{
				selectedCategoryIndex = 0;
			}

			selectedPrefabIndex = 0;
			UpdatePreview();
		}

		public static void NextPrefab()
		{
			if (!PreviewManager.isSetup)
				return;

			if (selectedPrefabIndex < PrefabLoader.prefabs[refSelectCategory.selectValue].Count -1)
			{
				selectedPrefabIndex++;
			}
			else
			{
				selectedPrefabIndex = 0;
			}

			UpdatePreview();		
		}

		public static void PrevPrefab()
		{
			if (!PreviewManager.isSetup)
				return;

			if (selectedPrefabIndex == 0)
			{
				selectedPrefabIndex = PrefabLoader.prefabs[refSelectCategory.selectValue].Count - 1;
				
			}
			else
			{
				selectedPrefabIndex--;
			}

			UpdatePreview();
		}



		public static void MyButtonActionExample()
		{
			MelonLogger.Msg("Button was pressed!");
		}

		public static void MySliderActionExample(float value)
		{
			MelonLogger.Msg("Slider value changed to: " + value);
		}

		public static void MyInputActionExample(string value)
		{
			MelonLogger.Msg("Input value changed to: " + value);
		}
		public static void MyToggleActionExample(bool value)
		{
			MelonLogger.Msg("Toggle value changed to: " + value);
		}
	}
}
