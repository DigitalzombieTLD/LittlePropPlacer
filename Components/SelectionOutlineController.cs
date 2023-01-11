using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using MelonLoader;

/*
 *  Not compatible with URP and HDRP at this moment.
    This function requires 3 full-screen-size rendertextures,and the outline shader contains for-loop.
    The cost of this is acceptable on PC,but if you are gonna use it on mobile platforms, you'd better optimize this by yourself.
*/
//[ExecuteInEditMode]
namespace LittlePropPlacer
{
	public class SelectionOutlineController : MonoBehaviour
	{
		public SelectionOutlineController(IntPtr intPtr) : base(intPtr) { }
		public static IntPtr foobar;

		public enum SelMode : int
		{
			OnlyParent = 0,
			AndChildren = 1
		}
		public enum AlphaType : int
		{
			KeepHoles = 0,
			Intact = 1
		}
		public enum OutlineMode : int
		{
			Whole = 0,
			ColorizeOccluded = 1,
			OnlyVisible = 2
		}

		private Material OutlineMat;
		private Material TargetMat;
		private Shader OutlineShader, TargetShader;
		private RenderTexture Mask, Outline;
		private Camera cam;
		private CommandBuffer cmd;
		private bool Ini = false;
		public SelMode SelectionMode = SelMode.OnlyParent;

		public OutlineMode OutlineType = OutlineMode.ColorizeOccluded;

		public AlphaType AlphaMode = AlphaType.KeepHoles;

		///public Renderer[] ChildrenRenderers;

		public Color OutlineColor = new Color(1f, 1f, 1f), OccludedColor = new Color(0.5f, 0.5f, 0.5f);

		public float OutlineWidth = 0.08f;

		public float OutlineHardness = 0.85f;

		public CustomPrefab currentTarget;
		public CustomPrefab lastTarget;
		public bool gotTarget = false;

		public void OnEnable()
		{
			Init();
		}

		public void Init()
		{
			MelonLogger.Msg("Init");
			OutlineShader = PrefabLoader.outlineshader;
			//OutlineShader = PrefabLoader.shaderObjectShader;
			TargetShader = PrefabLoader.targetshader;
			TargetMat = new Material(TargetShader);

			if (OutlineShader == null || TargetShader == null)
			{
				MelonLogger.Msg("Can't find the outline shaders,please check the Always Included Shaders in Graphics settings.");
				return;
			}

			cam = GetComponent<Camera>();
			cam.depthTextureMode = OutlineType > 0 ? DepthTextureMode.None : DepthTextureMode.Depth;
			OutlineMat = new Material(OutlineShader);

			if (OutlineType > 0)
			{
				Shader.EnableKeyword("_COLORIZE");
				Mask = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.RFloat);
				Outline = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.RG16);
				if (OutlineType == OutlineMode.OnlyVisible)
					Shader.EnableKeyword("_OCCLUDED");
				else
					Shader.DisableKeyword("_OCCLUDED");

			}
			else
			{
				Shader.DisableKeyword("_OCCLUDED");
				Shader.DisableKeyword("_COLORIZE");
				Mask = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.R8);
				Outline = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.R8);
			}
			cam.RemoveAllCommandBuffers();
			//cmd = new CommandBuffer { name = "Outline Command Buffer" };
			//cmd = new CommandBuffer(CommandBuffer.InitBuffer());

			foobar = CommandBuffer.InitBuffer();

			cmd = new CommandBuffer(foobar);
			cmd.m_Ptr = foobar;
			cmd.SetRenderTarget(Mask);


			//  System.MissingMethodException: string string.Format(string,Il2CppSystem.Object)
			cam.AddCommandBufferImpl(CameraEvent.BeforeImageEffects, cmd);

			OutlineMat.SetFloat("_OutlineWidth", OutlineWidth * 10f);
			OutlineMat.SetFloat("_OutlineHardness", 8.99f * (1f - OutlineHardness) + 0.01f);
			OutlineMat.SetColor("_OutlineColor", OutlineColor);
			OutlineMat.SetColor("_OccludedColor", OccludedColor);

			OutlineMat.SetTexture("_Mask", Mask);
			OutlineMat.SetTexture("_Outline", Outline);

			Ini = true;
			OnValidateManual();
		}
		public void OnValidateManual()
		{
			if (!Ini)
			{
				Init();
			}

			MelonLogger.Msg("OnValidtae");

			cam.depthTextureMode = OutlineType > 0 ? DepthTextureMode.Depth : DepthTextureMode.None;
			if (OutlineType > 0)
			{
				Shader.EnableKeyword("_COLORIZE");

				if (OutlineType == OutlineMode.OnlyVisible)
					Shader.EnableKeyword("_OCCLUDED");
				else
					Shader.DisableKeyword("_OCCLUDED");

			}
			else
			{
				Shader.DisableKeyword("_OCCLUDED");
				Shader.DisableKeyword("_COLORIZE");
			}
		}
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{		
			if (OutlineMat == null)
			{
				Init();
				if (!Ini)
					return;
			}

			Graphics.Blit(source, Outline, OutlineMat, 0);

			Graphics.Blit(source, destination, OutlineMat, 1);
			//Graphics.Blit(Outline, destination);

		}
		private void RenderTarget(Renderer target)
		{			
			bool MainTexFlag = false;
			string[] attrs = target.sharedMaterial.GetTexturePropertyNames();
			foreach (var c in attrs)
			{
				if (c == "_MainTex")
				{
					MainTexFlag = true;
					break;
				}
			}
			if (MainTexFlag && target.sharedMaterial.mainTexture != null && AlphaMode == AlphaType.KeepHoles)
			{
				TargetMat.mainTexture = target.sharedMaterial.mainTexture;
			}

			//cmd.DrawRenderer(target, TargetMat);
			cmd.Internal_DrawRenderer(target, TargetMat);
			Graphics.ExecuteCommandBuffer(cmd);
		}

		public void SetTarget(CustomPrefab newTarget)
		{
			if (lastTarget == null)
			{
				lastTarget = newTarget;
				currentTarget = newTarget;
			}

			if (currentTarget != newTarget)
			{
				lastTarget = currentTarget;
				currentTarget = newTarget;
			}
		
			gotTarget = true;
		}

		private void RenderTarget()
		{			
			if (gotTarget)
			{
				if (currentTarget.thisRenderer != null)
				{
					cmd.SetRenderTarget(Mask);
					cmd.ClearRenderTarget(true, true, Color.black);
					RenderTarget(currentTarget.thisRenderer);
				}
			}
		}

		public void ClearTargets()
		{
			cmd.ClearRenderTarget(true, true, Color.black);

			Graphics.ExecuteCommandBuffer(cmd);
			cmd.Clear();

			gotTarget = false;
		}

		// Update is called once per frame
		public void Update()
		{
			RenderTarget();			
		}
	}
}