using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hittable interface to the hittable things
public interface IHIttable
{
    int Health { get; }
    void GetHit(WeponItemSO weapon, Vector3 hitpoint);
}
