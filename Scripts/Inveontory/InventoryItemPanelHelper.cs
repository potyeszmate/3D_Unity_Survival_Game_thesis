using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemPanelHelper : ItemPanelHelper, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Action<int, bool> OnClickEvent;

    public Action<PointerEventData> DragStopCallback, DragContinueCallback;
    public Action<PointerEventData, int> DragStartCallback, DropCalback;

    [SerializeField]
    private Text countText = null;
    public int itemCount;
    public bool isEmpty = true;
    public bool isHotbarItem = false;

    public Image equippedIndicator;
    private bool equipped = false;

    // Sets the item datas that we need to show. The name, count, and the image
    public void SetItemUI(string name, int count, Sprite image)
    {
        itemName = name;
        itemCount = count;
        // If its not hotbar than we need the item name to show
        if (!isHotbarItem)
            nameText.text = itemName;
        // Sets the itemcount
        if (count < 0)
        {
            countText.text = "";
        }
        else
        {
            countText.text = itemCount + "";
        }
        isEmpty = false;
        SetImageSprite(image);

        // Shows the equipped indicator if the weapon is equipped
        if (equipped)
        {
            ModifyEquippedIndicatorAlpha(1);
        }
        else
        {
            ModifyEquippedIndicatorAlpha(0);
        }
    }

    // The normal items UI (first the count is always 0)
    public override void SetItemUI(string name, Sprite image)
    {
        itemName = name;
        itemCount = 0;
        if (!isHotbarItem)
            nameText.text = itemName;
        countText.text = "";
        isEmpty = false;
        SetImageSprite(image);

        if (equipped)
        {
            ModifyEquippedIndicatorAlpha(1);
        }
        else
        {
            ModifyEquippedIndicatorAlpha(0);
        }
    }

    // Sets the item UI with the data that we set to the item
    public void SwapWithData(string name, int count, Sprite image, bool isEmpty)
    {
        SetItemUI(name, count, image);
        this.isEmpty = isEmpty;
    }

    // Deletes the item from the UI (from the intventory panel or the hotbar, not just when we delete it)
    public override void ClearItem()
    {
        itemName = "";
        itemCount = -1;
        countText.text = "";
        if (!isHotbarItem)
            nameText.text = itemName;
        ResetImage();
        isEmpty = true;
        ToggleHighlight(false);
    }

    // Simple Click event. Checks if we click to something
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent.Invoke(GetInstanceID(), isEmpty);
    }
    
    //  When we start to drag something we call this method
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEmpty)
            return;
        DragStartCallback.Invoke(eventData, GetInstanceID());
    }

    // During the Drag we call the method
    public void OnDrag(PointerEventData eventData)
    {
        if (isEmpty)
            return;
        DragContinueCallback.Invoke(eventData);
    }

    // End of the drag we call this method (drag an item to other position)
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End drag");
        if (isEmpty)
            return;
        DragStopCallback.Invoke(eventData);
    }

    // Updating the number of items
    internal void UpdateCount(int count)
    {
        itemCount = count;
        countText.text = itemCount + "";
    }

    // When we drop the item we call this method
    public void OnDrop(PointerEventData eventData)
    {
        DropCalback.Invoke(eventData, GetInstanceID());
    }

    // If the item is equipped it sets an image's alpha to 1, otherwise to 0 (show and not show)
    public void ToggleEquippedIndicator()
    {
        if(equipped == false)
        {
            ModifyEquippedIndicatorAlpha(1);
            equipped = true;
        }
        else
        {
            ModifyEquippedIndicatorAlpha(0);
            equipped = false;
        }
    }

    // Sets the image-s alpha to a certain level
    private void ModifyEquippedIndicatorAlpha(int alpha)
    {
        Color c = equippedIndicator.color;
        c.a = Mathf.Clamp01(alpha);
        equippedIndicator.color = c;
    }

}
