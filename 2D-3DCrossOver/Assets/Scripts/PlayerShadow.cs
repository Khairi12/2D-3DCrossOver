using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour {
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;

	private void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
    }

    private void Update () {
        RaycastHit hit;

        if (Physics.Raycast(player.position, Vector3.down, out hit)) {
            spriteRenderer.sprite = defaultSprite;
            transform.position = new Vector3(player.position.x, hit.point.y + 0.1f, player.position.z);
        }
        else {
            spriteRenderer.sprite = null;
        }
	}
}
