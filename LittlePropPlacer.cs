using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace LittlePropPlacer
{
    public class LittlePropPlacerMain : MelonMod
    {
		public static Il2CppAssetBundle propBundle;
		

		public override void OnApplicationStart()
		{
			ClassInjector.RegisterTypeInIl2Cpp<CustomPrefab>();
			ClassInjector.RegisterTypeInIl2Cpp<SelectionOutlineController>();
			ClassInjector.RegisterTypeInIl2Cpp<UIObjectPreviewAdvanced>();
			

			propBundle = Il2CppAssetBundleManager.LoadFromFile("Mods\\LittlePropPlacer.unity3d");
			PrefabLoader.LoadAllPropsFromBundle();
			

			MyInput.thisMod = this;
		}
		
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			//if (sceneName == "Placemaker")
			if (sceneName == "FlatscreenUi")
			{								
				// Initializing ModUI
				MyModUI.Initialize(this);
				MyModUI.UpdatePreview();
				PrefabInstancer.Setup();
			}
		}
		
		public override void OnUpdate()
		{
			MyInput.GetInput();
			PropEditor.Action();			
		}	
	}
}
