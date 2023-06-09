using System.Collections.Generic;
using UnityEngine;

public class OmitSpaces : MonoBehaviour
{
    public static OmitSpaces instance;
    private static List<BoxCollider> omitSpaces;
    [SerializeField] private LayerMask omitMask = 0;
    private void Awake()
    {
        instance = this;
        CollectOmitSpaceColliders();
    }

    private void CollectOmitSpaceColliders()
    {
        omitSpaces = FindGameObjectsWithLayer<BoxCollider>(omitMask);
    }

    private List<T> FindGameObjectsWithLayer<T>(LayerMask targetMask)
    {
        List<T> ans = new List<T>();
        if (omitMask != 0)
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            foreach (GameObject _object in allObjects)
            {
                if ((targetMask & (1 << _object.layer)) != 0)
                {
                    ans.Add(_object.GetComponent<T>());
                }
            }
        }
        return ans;
    }

    public static bool PositionIsWithinOmitSpace(Vector3 checkPosition)
    {
        bool ans = false;
        foreach (BoxCollider _item in omitSpaces)
        {
            if (_item.bounds.Contains(checkPosition))
            {
                ans = true;
            }
        }
        return ans;
    }

}
