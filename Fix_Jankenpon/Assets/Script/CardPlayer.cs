using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CardPlayer : MonoBehaviour
{
    [SerializeField] Card choosenCard;
    public Transform atkPosRef;
    public TMP_Text nameText;
    public TMP_Text healthText;
    private Tweener animationTweener;
    public float Health;
    public float MaxHealth;
    public HealthBar healthBar;
    public TMP_Text Name { get => nameText; }

    public void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PlayerPropertyNames.Room.MaxHealth, out var maxHealth))
        {
            Health = (float)maxHealth;
            MaxHealth = (float)maxHealth;
            healthText.text = Health + " / " + MaxHealth;
        }

        maxHealth = 100f;
        Health = MaxHealth;
    }
    public Attack? AttackValue
    {
        get => choosenCard == null ? null : choosenCard.AttackValue;
    }

    public void SetChoosenCard(Card newCard)
    {
        // set attack logic
        if (choosenCard != null)
        {
            choosenCard.Reset();
        }

        choosenCard = newCard;
        choosenCard.transform.DOScale(choosenCard.transform.localScale * 1.2f, 0.2f);
    }

    public void AnimateAttack()
    {
        animationTweener = choosenCard.transform.DOMove(atkPosRef.position, 0.5f).SetEase(Ease.Linear).SetDelay(0.5f);
    }

    public bool IsAnimating()
    {
        return animationTweener.IsActive();
    }

    public void AnimateDamage()
    {
        var image = choosenCard.GetComponent<Image>();
        animationTweener = image.DOColor(Color.red, 0.1f).SetLoops(3, LoopType.Yoyo).SetDelay(0.5f);
    }

    public void AnimateDraw()
    {
        animationTweener = choosenCard.transform.DOMove(choosenCard.OriginalPosition, 1).SetEase(Ease.InBack).SetDelay(0.2f);
        // var image = choosenCard.GetComponent<Image>();
        // animationTweener = image.DOColor(Color.blue, 0.1f).SetLoops(3, LoopType.Yoyo).SetDelay(0.2f);
        // animationTweener = image.transform.DOMove(choosenCard.OriginalPosition, 1f).SetEase(Ease.InBack).SetDelay(0.5f);
    }

    public void Reset()
    {
        if (choosenCard != null)
        {
            choosenCard.Reset();
        }

        choosenCard = null;
        // choosenCard.transform.DOScale(choosenCard.transform.localScale * 1.2f, 0.2f);
    }

    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        healthBar.UpdaterBar(Health / MaxHealth);
        healthText.text = Health + " / " + MaxHealth;
    }

    public void isClickAble(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.SetCLickAble(value);
        }
    }
}
