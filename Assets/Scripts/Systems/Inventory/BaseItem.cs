using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class BaseItem : MonoBehaviour
{

    [SerializeField] private string name;
    [SerializeField] private bool canPickup = false;
    private GameObject player;
    private Transform _transform;
    private Vector3 originalScale;
    private float t = 0;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _transform = transform;
        originalScale = _transform.localScale;
    }

    private void Update()
    {
        if (!canPickup) return;
        if(Vector2.Distance(_transform.position, player.transform.position) < 1)
        {
             t += Time.deltaTime;
            _transform.position = Vector3.Lerp(_transform.position, player.transform.position, t);
            _transform.localScale = Vector3.Lerp(_transform.localScale, Vector3.zero, t);
        }
     
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            player.GetComponent<Inventory>().AddItem(name);
            Destroy(gameObject, 0.5f);
    
        }

    }



}
