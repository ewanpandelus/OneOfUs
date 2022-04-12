using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(SpriteRenderer))]
public class BaseItem : MonoBehaviour
{

    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private bool canPickup = false;
    private GameObject player;
    private Transform _transform;
    private bool initiatedPicking = false;
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
             t += Time.deltaTime/20;
            _transform.position = Vector3.Lerp(_transform.position, player.transform.position, t);
            _transform.localScale = Vector3.Lerp(_transform.localScale, Vector3.zero, t);
            initiatedPicking = true;
            return;
        }
        if (initiatedPicking) { _transform.DOScale(originalScale.x,1).SetEase(Ease.InOutElastic); initiatedPicking = false; }
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
