using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    private CropData curCrop;
    private int plantDay;
    private int daysSinceLastWatered;

    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCrop;
    public static event UnityAction<CropData> onHarvestCrop;

    // Called when the crop has been planted for the first time.
    public void Plant(CropData crop)
    {
        curCrop = crop;
        plantDay = GameManager.instance.curDay;
        daysSinceLastWatered = 1;
        UpdateCropSprite();

        onPlantCrop?.Invoke(crop);
    }

    // Called when a new day ticks over.
    public void NewDayCheck()
    {
        daysSinceLastWatered++;

        if (daysSinceLastWatered > 3)
        {
            Destroy(gameObject);
        }

        UpdateCropSprite();
    }

    // Called when the crop has progressed.
    void UpdateCropSprite()
    {
        int cropProg = CropProgress();

        if (cropProg < curCrop.daysToGrow)
        {
            sr.sprite = curCrop.growProgressSprites[cropProg];
        }
        else
        {
            sr.sprite = curCrop.readyToHarvestSprite;
        }
    }

    // Called when the crop has been watered.
    public void Water()
    {
        daysSinceLastWatered = 0;
    }

    // Called when we want to harvest the crop.
    public void Harvest()
    {
        if (CanHarvest())
        {
            onHarvestCrop?.Invoke(curCrop);
            Destroy(gameObject);
        }
    }

    // Returns the number of days that the crop has been planted for.
    int CropProgress()
    {
        return GameManager.instance.curDay - plantDay;
    }

    // Can we currently harvest the crop?
    public bool CanHarvest()
    {
        return CropProgress() >= curCrop.daysToGrow;
    }
}