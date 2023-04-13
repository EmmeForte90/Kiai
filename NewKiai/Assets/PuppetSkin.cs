using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine;
using Spine.Unity;

public class PuppetSkin : MonoBehaviour
{
   	// Character skins
	[SpineSkin] public string DressSkin = "Nude";

	[SpineSkin] public string katana = "default";
	
	SkeletonGraphic _skeletonGraphic;
	Skeleton skeleton;
	// This "naked body" skin will likely change only once upon character creation,
	// so we store this combined set of non-equipment Skins for later re-use.
	Skin characterSkin;

	// for repacking the skin to a new atlas texture
	private Material runtimeMaterial;
	private Texture2D runtimeAtlas;

public static PuppetSkin Instance;

// Equipment skins
public enum ItemSlot
{
	None,
	katana,
	DressSkin
}
	void Awake()
	{
		if (Instance == null)
        {
            Instance = this;
        }
		//skeletonAnimation = this.GetComponent<SkeletonAnimation>();
		_skeletonGraphic = this.GetComponent<SkeletonGraphic>();

	}

	
	public void OptimizeSkin()
	{
		// Create a repacked skin.
		Skin previousSkin = _skeletonGraphic.Skeleton.Skin;
		// Note: materials and textures returned by GetRepackedSkin() behave like 'new Texture2D()' and need to be destroyed
		if (runtimeMaterial)
			Destroy(runtimeMaterial);
		if (runtimeAtlas)
			Destroy(runtimeAtlas);
		Skin repackedSkin = previousSkin.GetRepackedSkin("Repacked skin", _skeletonGraphic.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial, out runtimeMaterial, out runtimeAtlas);
		previousSkin.Clear();

		// Use the repacked skin.
		_skeletonGraphic.Skeleton.Skin = repackedSkin;
		_skeletonGraphic.Skeleton.SetSlotsToSetupPose();
		_skeletonGraphic.AnimationState.Apply(_skeletonGraphic.Skeleton);

		// `GetRepackedSkin()` and each call to `GetRemappedClone()` with parameter `premultiplyAlpha` set to `true`
		// cache necessarily created Texture copies which can be cleared by calling AtlasUtilities.ClearCache().
		// You can optionally clear the textures cache after multiple repack operations.
		// Just be aware that while this cleanup frees up memory, it is also a costly operation
		// and will likely cause a spike in the framerate.
		AtlasUtilities.ClearCache();
		Resources.UnloadUnusedAssets();
	}

	
public void UpdateCharacterSkinUI()
{
skeleton = _skeletonGraphic.Skeleton;
if(skeleton == null)
		{print("niente");}
SkeletonData skeletonData = skeleton.Data;
characterSkin = new Skin("character-base");
// Note that the result Skin returned by calls to skeletonData.FindSkin()
// could be cached once in Start() instead of searching for the same skin
// every time. For demonstration purposes we keep it simple here.
characterSkin.AddSkin(skeletonData.FindSkin(DressSkin));
_skeletonGraphic.initialSkinName = characterSkin.Name;
}

public void UpdateCombinedSkinUI()
{
skeleton = _skeletonGraphic.Skeleton;
if(skeleton == null)
		{print("niente");}
Skin resultCombinedSkin = new Skin("character-combined");

resultCombinedSkin.AddSkin(characterSkin);
AddEquipmentSkinsTo(resultCombinedSkin);

skeleton.SetSkin(resultCombinedSkin);
skeleton.SetSlotsToSetupPose();
_skeletonGraphic.Initialize(false);
}



	void AddEquipmentSkinsTo(Skin combinedSkin)
	{
		skeleton = _skeletonGraphic.Skeleton;
		SkeletonData skeletonData = skeleton.Data;
		if (!string.IsNullOrEmpty(DressSkin)) combinedSkin.AddSkin(skeletonData.FindSkin(DressSkin));
		if (!string.IsNullOrEmpty(katana)) combinedSkin.AddSkin(skeletonData.FindSkin(katana));
		
	}
	
}