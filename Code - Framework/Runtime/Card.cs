using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject//, IPhysicalCollecteable<CardData>
{
  
}

[System.Serializable]
public class Card : IPhysicalCollecteable<Card>
{
    [SerializeField, EditorReadOnly]
    private ICollection<Card> physicalCollection;
    public ICollection<Card> PhysicalCollection
    {
        get { return physicalCollection; }
        set { physicalCollection = value; }
    }
    public CardData Data;

    public override string ToString()
    {
        return string.Format("Card - {0}", Data.ToString());//, physicalCollection.Count());
    }
}

[SerializeField]
public class CardPile : StackCollection<Card> { }
[SerializeField]
public class PhysicalCardPile : PhysicalStackCollection<Card> { }
