using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvaluateEnvironment : MonoBehaviour
{
    [SerializeField] float interactionRadius = 0.0f;
    
    

    public NPC ClosestNPC()
    {
        Collider2D[] surroundingColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
        List<GameObject> surroundingGameObjects = new List<GameObject>();
        if (surroundingColliders.Length > 0)
        {
            foreach (Collider2D col2D in surroundingColliders) surroundingGameObjects.Add(col2D.gameObject);
           
            List<GameObject> surroundingNPCS = new List<GameObject>();
            surroundingNPCS = surroundingGameObjects.Where(x => x.GetComponent<NPC>() != null).ToList();
            if (surroundingNPCS.Count > 0) { return ClosestObj(surroundingNPCS).GetComponent<NPC>(); }
        }
        return null;
    }


    public GameObject ClosestObj(List<GameObject> _objects)
    {
        float min = float.MaxValue;
        GameObject closestObj = null;
        for (int i = 0; i < _objects.Count; i++)
        {
            if (Vector2.Distance(transform.position, _objects[i].transform.position) < min)
            {
                min = Vector2.Distance(transform.position, _objects[i].transform.position);
                closestObj = _objects[i];
            }
        }
        return closestObj;
    }
}
