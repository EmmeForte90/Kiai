using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine;
using Spine.Unity;

public class ChangeHeroSkin : MonoBehaviour
{
	// Character skins
	[SpineSkin] public string DressSkin = "default";

	[SpineSkin] public string katana = "default";
	
	SkeletonAnimation skeletonAnimation;
	Skeleton skeleton;
	Skin characterSkin;

	// for repacking the skin to a new atlas texture
	private Material runtimeMaterial;
	private Texture2D runtimeAtlas;

public static ChangeHeroSkin Instance;

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
		skeletonAnimation = this.GetComponent<SkeletonAnimation>();
	}

	

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
			UpdateCharacterSkin();
			UpdateCombinedSkin();
		}
    }


	public void OptimizeSkin()
	{
		// Create a repacked skin.
		Skin previousSkin = skeletonAnimation.Skeleton.Skin;
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
		AtlasUtilities.ClearCache();
		Resources.UnloadUnusedAssets();
	}

	public void UpdateCharacterSkin()
	{
		skeleton = skeletonAnimation.Skeleton;
		if(skeleton == null)
		{print("niente");}
		SkeletonData skeletonData = skeleton.Data;
		characterSkin = new Skin("character-base");
		characterSkin.AddSkin(skeletonData.FindSkin(DressSkin));
	}



	void AddEquipmentSkinsTo(Skin combinedSkin)
	{
		skeleton = skeletonAnimation.Skeleton;
		SkeletonData skeletonData = skeleton.Data;
		if (!string.IsNullOrEmpty(DressSkin)) combinedSkin.AddSkin(skeletonData.FindSkin(DressSkin));
		if (!string.IsNullOrEmpty(katana)) combinedSkin.AddSkin(skeletonData.FindSkin(katana));
		
	}

	public void UpdateCombinedSkin()
	{
		skeleton = skeletonAnimation.Skeleton;
		if(skeleton == null)
		{print("niente");}
		Skin resultCombinedSkin = new Skin("character-combined");

		resultCombinedSkin.AddSkin(characterSkin);
		AddEquipmentSkinsTo(resultCombinedSkin);

		skeleton.SetSkin(resultCombinedSkin);
		skeleton.SetSlotsToSetupPose();
	}

	
}

