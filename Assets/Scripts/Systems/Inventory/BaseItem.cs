using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class BaseItem : MonoBehaviour
{

    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private bool canPickup = false;
    private GameObject player;
    private Transform _transform;
    private float t = 0;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _transform = transform;
    }

    private void Update()
    {
        if (!canPickup) return;
        if(Vector2.Distance(_transform.position, player.transform.position) < 1)
        {
            t += Time.deltaTime/2;
            _transform.position = Vector3.Lerp(_transform.position, player.transform.position, t);
            _transform.localScale = Vector3.Lerp(_transform.localScale, Vector3.zero, t);
            return;
        }
        t = 0;
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
