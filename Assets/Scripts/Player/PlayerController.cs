using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] private float movementSpeed;
    [SerializeField] float interactionRadius;
    Vector3 movement = new Vector3 (0,0,0);

    void Update()
    {
        if (dialogueUI.GetShowingText()) return;
        EvaluateInput();
        transform.position += movement * Time.deltaTime;
    }
    private void EvaluateInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        movement.y = Input.GetAxisRaw("Vertical") * movementSpeed;
        EvaluateInteraction();
      
    }
    private void EvaluateInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NPC closestNPC = ClosestNPC();
            if (FacingCharacter(closestNPC))
            {
                if(!dialogueUI.GetShowingText()) closestNPC.RunDialogue();
            }
        }
    }
    public bool FacingCharacter(NPC _closestNPC)  // Placeholder code, will be updated later 
    {
        if (_closestNPC == null) return false;
        _closestNPC = ClosestNPC();
        Vector3 dir = (_closestNPC.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(dir, transform.forward);
        if (dot < 0.5f)
        {
            return true;
        }
        return false;
    }
    private NPC ClosestNPC()
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
    private GameObject ClosestObj(List<GameObject> _objects)
    {
        float min = float.MaxValue;
        GameObject closestObj = null;
        for(int i = 0; i < _objects.Count; i++)
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
