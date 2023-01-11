using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;

namespace LittlePropPlacer
{
	public static class Tools
	{
		static string path = Application.dataPath + "/../Mods/";

		public static void toPNG(string filename, RenderTexture textureToExport)
		{
			if (!textureToExport)
			{
				MelonLogger.Msg("ERROR - IS NULL: " + filename);
				return;
			}

			//Texture2D yoink = new Texture2D(textureToExport.width, textureToExport.height, TextureFormat.RGBA32, false);
			RenderTexture.active = textureToExport;
			Texture2D tex = new Texture2D(textureToExport.width, textureToExport.height);
			tex.ReadPixels(new Rect(0, 0, textureToExport.width, textureToExport.height), 0, 0);
			tex.Apply();

			toPNG(filename, tex);
		}

		public static void toPNG(string filename, Texture textureToExport)
		{
			if(!textureToExport)
			{
				MelonLogger.Msg("ERROR - IS NULL: " + filename);
				return;
			}

			Texture2D yoink = new Texture2D(textureToExport.width, textureToExport.height, TextureFormat.RGBA32, false);
				
			RenderTexture renderTexture = new RenderTexture(textureToExport.width, textureToExport.height, 32);
			Graphics.Blit(textureToExport, renderTexture);

			toPNG(filename, renderTexture);
		}

		public static void toPNG(string filename, Texture2D textureToExport)
		{
			if (!textureToExport)
			{
				MelonLogger.Msg("ERROR - IS NULL: " + filename);
				return;
			}

			Texture2D yoink = textureToExport;
			Il2CppStructArray<byte> bytes = UnityEngine.Il2CppImageConversionManager.EncodeToPNG(yoink);
			MelonLogger.Msg("Exporting texture: " + filename);
			System.IO.File.WriteAllBytes(path + filename + ".png", bytes);
		}
	}
}
