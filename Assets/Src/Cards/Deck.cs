﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private readonly Queue<Card> _cards = new Queue<Card>();
    private readonly Queue<Card> _discardedCards = new Queue<Card>();
    private Player _player;

    public CardType Type;

    private void Start()
    {
        _player = transform.GetComponentInParent<Player>();
        foreach (var card in GetComponentsInChildren<Card>())
        {
            card.Player = _player;
            Discard(card);
        }
        Shuffle();
    }
    private void Update()
    {
        foreach (var card in _cards)
        {
            card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, Vector3.zero, 3 * Time.deltaTime);
            card.transform.localEulerAngles = Vector3.Lerp(card.transform.localEulerAngles, Vector3.zero, 3 * Time.deltaTime);
        }
    }

    public Card Draw()
    {
        return _cards.Count > 0 ? _cards.Dequeue() : null;
    }
    public void Discard(Card card)
    {
        _discardedCards.Enqueue(card);
        card.transform.parent = transform;
    }
    public void Shuffle()
    {
        var shuffled = _discardedCards.OrderBy(a => Guid.NewGuid()).ToArray();
        _discardedCards.Clear();
        foreach (var card in shuffled)
        {
            _cards.Enqueue(card);
        }
    }

    public void Click()
    {
        if (_player.Hand.NumCards >= 6) return;
        var card = Draw();
        if (card == null) return;
        _player.Hand.AddCard(card);
    }
}
