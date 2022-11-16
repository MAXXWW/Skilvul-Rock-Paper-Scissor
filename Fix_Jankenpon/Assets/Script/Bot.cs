using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public CardPlayer player;
    public CardGameManager gameManager;
    public float choosingInterval;
    private float timer;
    int lastSelected = 0;
    Card[] cards;

    private void Start()
    {
        cards = GetComponentsInChildren<Card>();
    }

    void Update()
    {
        if (gameManager.state != CardGameManager.GameState.ChooseAttack)
        {
            timer = 0;
            return;
        }
        if (timer < choosingInterval)
        {
            timer += Time.deltaTime;
            return;
        }
        timer = 0;
        ChooseAttack();
    }

    public void ChooseAttack()
    {
        var random = Random.Range(1, cards.Length);
        var selection = (lastSelected + random) % cards.Length;
        // last + random % length = value
        // (0 + 1) % 3 = 1
        // (0 + 2) % 3 = 2
        // (1 + 1) % 3 = 2
        // (1 + 2) % 3 = 0
        // (2 + 1) % 3 = 0
        // (2 + 2) % 3 = 1
        player.SetChoosenCard(cards[selection]);
        lastSelected = selection;
    }
}
