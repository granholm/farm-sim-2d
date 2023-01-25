using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTile : MonoBehaviour
{
    private Crop curCrop;
    public GameObject cropPrefab;

    public SpriteRenderer sr;
    private bool tilled;

    [Header("Sprites")]
    public Sprite grassSprite;
    public Sprite tilledSprite;
    public Sprite wateredTilledSprite;

    void Start()
    {
        // Set the default grass sprite.
        sr.sprite = grassSprite;
    }

    // Called when the player interacts with the tile.
    public void Interact()
    {
        if (!tilled)
        {
            Till();
        }
        else if (!HasCrop() && GameManager.instance.CanPlantCrop())
        {
            PlantNewCrop(GameManager.instance.selectedCropToPlant);
        }
        else if (HasCrop() && curCrop.CanHarvest())
        {
            curCrop.Harvest();
        }
        else
        {
            Water();
        }
    }

    // Called when we interact with a tilled tile and we have crops to plant.
    void PlantNewCrop(CropData crop)
    {
        if (!tilled)
            return;

        curCrop = Instantiate(cropPrefab, transform).GetComponent<Crop>();
        curCrop.Plant(crop);

        GameManager.instance.onNewDay += OnNewDay;
    }

    // Called when we interact with a grass tile.
    void Till()
    {
        tilled = true;
        sr.sprite = tilledSprite;
    }

    // Called when we interact with a crop tile.
    void Water()
    {
        sr.sprite = wateredTilledSprite;

        if (HasCrop())
        {
            curCrop.Water();
        }
    }

    // Called every time a new day occurs. 
    // Only called if the tile contains a crop.
    void OnNewDay()
    {
        if (curCrop == null)
        {
            tilled = false;
            sr.sprite = grassSprite;

            GameManager.instance.onNewDay -= OnNewDay;
        }
        else if (curCrop != null)
        {
            sr.sprite = tilledSprite;
            curCrop.NewDayCheck();
        }
    }

    // Does this tile have a crop planted?
    bool HasCrop()
    {
        return curCrop != null;
    }
}