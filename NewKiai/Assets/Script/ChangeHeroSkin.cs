using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine;
using Spine.Unity;

public class ChangeHeroSkin : MonoBehaviour
{
	// Character skins
	[SpineSkin] public string DressSkin = "Nude";

	[SpineSkin] public string katana = "default";
	
	SkeletonAnimation skeletonAnimation;
	// This "naked body" skin will likely change only once upon character creation,
	// so we store this combined set of non-equipment Skins for later re-use.
	Skin characterSkin;

	// for repacking the skin to a new atlas texture
	private Material runtimeMaterial;
	private Texture2D runtimeAtlas;

// Equipment skins
public enum ItemSlot
{
	None,
	katana,
	DressSkin
}
	void Awake()
	{
		
		skeletonAnimation = this.GetComponent<SkeletonAnimation>();
	}

	void Start()
	{
		UpdateCharacterSkin();
		UpdateCombinedSkin();
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
			UpdateCharacterSkin();
			UpdateCombinedSkin();
		}
    }

    public void Equip()
	{
		UpdateCharacterSkin();
		UpdateCombinedSkin();
	}

	public void OptimizeSkin()
	{
		// Create a repacked skin.
		Skin previousSkin = skeletonAnimation.Skeleton.Skin;
		// Note: materials and textures returned by GetRepackedSkin() behave like 'new Texture2D()' and need to be destroyed
		if (runtimeMaterial)
			Destroy(runtimeMaterial);
		if (runtimeAtlas)
			Destroy(runtimeAtlas);
		Skin repackedSkin = previousSkin.GetRepackedSkin("Repacked skin", skeletonAnimation.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial, out runtimeMaterial, out runtimeAtlas);
		previousSkin.Clear();

		// Use the repacked skin.
		skeletonAnimation.Skeleton.Skin = repackedSkin;
		skeletonAnimation.Skeleton.SetSlotsToSetupPose();
		skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);

		// `GetRepackedSkin()` and each call to `GetRemappedClone()` with parameter `premultiplyAlpha` set to `true`
		// cache necessarily created Texture copies which can be cleared by calling AtlasUtilities.ClearCache().
		// You can optionally clear the textures cache after multiple repack operations.
		// Just be aware that while this cleanup frees up memory, it is also a costly operation
		// and will likely cause a spike in the framerate.
		AtlasUtilities.ClearCache();
		Resources.UnloadUnusedAssets();
	}

	void UpdateCharacterSkin()
	{
		Skeleton skeleton = skeletonAnimation.Skeleton;
		SkeletonData skeletonData = skeleton.Data;
		characterSkin = new Skin("character-base");
		// Note that the result Skin returned by calls to skeletonData.FindSkin()
		// could be cached once in Start() instead of searching for the same skin
		// every time. For demonstration purposes we keep it simple here.
		characterSkin.AddSkin(skeletonData.FindSkin(DressSkin));
	}

	void AddEquipmentSkinsTo(Skin combinedSkin)
	{
		Skeleton skeleton = skeletonAnimation.Skeleton;
		SkeletonData skeletonData = skeleton.Data;
		if (!string.IsNullOrEmpty(DressSkin)) combinedSkin.AddSkin(skeletonData.FindSkin(DressSkin));
		if (!string.IsNullOrEmpty(katana)) combinedSkin.AddSkin(skeletonData.FindSkin(katana));
		
	}

	void UpdateCombinedSkin()
	{
		Skeleton skeleton = skeletonAnimation.Skeleton;
		Skin resultCombinedSkin = new Skin("character-combined");

		resultCombinedSkin.AddSkin(characterSkin);
		AddEquipmentSkinsTo(resultCombinedSkin);

		skeleton.SetSkin(resultCombinedSkin);
		skeleton.SetSlotsToSetupPose();
	}
}

