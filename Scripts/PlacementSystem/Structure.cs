using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public StructureItemSO Data { get; private set; }

    // Set the data to the structe data which is selected
    internal void SetData(StructureItemSO selectedStructureData)
    {
        Data = selectedStructureData;
    }
}
