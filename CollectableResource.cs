using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum resourceType { wood, stone };
public class CollectableResource : MonoBehaviour
{
    public int resourceValue = 10;

    public resourceType type;
}
