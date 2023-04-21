using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine;
using Spine.Unity;

public class PuppetSkin : MonoBehaviour
{
   	// Character skins
	[SpineSkin] public string DressSkin = "default";
	[SpineSkin] public string katana = "default";
	
	SkeletonGraphic _skeletonGraphic;
	Skeleton skeleton;
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
		AtlasUtilities.ClearCache();
		Resources.UnloadUnusedAssets();
	}

	
public void UpdateCharacterSkinUI(string CH)
{
_skeletonGraphic.Skeleton.SetSkin(CH);
 characterSkin = new Skin(CH);
_skeletonGraphic.LateUpdate();
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