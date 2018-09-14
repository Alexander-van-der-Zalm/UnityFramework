using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

#region Readme

/// <summary>
/// Contains a set of cards
/// Basic version:
/// x - Succes cards
/// y - Fail cards
/// succes % : x / (x + y)
/// fail % : y / (x + y)
/// On each resolve a card is drawn: succes is succes, fail is a fail
/// Afterwards it is discarded (move to discardpile)
/// If all cards are discarded (move discardpile into drawpile & shuffle)
/// This method normalizes chance in a more understandable & more predictable format
/// 
/// Additional formats/ides:
///  - include crit cards
///  - manipulating odd by deck building (adding and discarding cards)
///  - choose from multiple cards
///  - AI deck
///  - Enhancements to cards to evolve cards
///  - have cards do multiple things & choose which effect upon resolve (ala gloomhaven ability cards)
///  - attack modifier cards ala gloomhaven
///     + addition: +1,2,3/-1,2,3
///     + multiplication: x2, x0
///     + add another card effect (choose?)
///     + status effects:
///         Negative:
///         ^ Poison (+1 attack to all attackers)
///         ^ Wound (1 damage start of turn)
///         ^ Immobilize (cannot move)
///         ^ Disarm (no attacks)
///         ^ Stun (no attack, items & move)
///         ^ Muddle (always disadvantaged)
///         ^ Curse (shuffle into deck & remove when resolved)
///         Positive:
///         ^ Invisible (untargeteable)
///         ^ Strengthen (always advantage)
///         ^ Bless (shuffle into deck & remove when resolved)
///     + single use
///     + shuffle
///     + reorder (choose)
///     + discard from drawpile (choose)
///     + place into player or monster deck (like a 2x or x0)
///     + add target, pierce armor, pull, push
///  - Advantage (choose from two)
///  - Disadvantage (choose the worst)
///  - Perk based deck manipulation
/// 
/// Each card type is a Scripteable object
///  - Resolve(Source,Target)
///  - Effect description
///  - Attack modifier
///  - Special effects?
///  - owner?
///  delegate? or just an interface?
/// 
/// Decks/Piles:
/// Deck (owned by x) - CardCollection
/// DiscardPile - CardCollection
/// DrawPile - CardCollection
/// Hand - CardCollection
/// Table - CardCollection
///  - DrawCards (move DrawPile.Pop() -> Hand)
///  - DiscardCard (move Hand -> DiscardPile)
///  - AddToDeck (card.owner = newOwner & move -> Hand/Draw/Discard)
///  - RemoveFromDeck ((card.owner = null & move -> outofgame)/delete card)
///  - Shuffle
///  

/// 
/// Design considerations:
///  - An all set (autosubscribe/register as a card?)
///  - Scripteable Object Enum pattern for typing (owner, list of properties)
///  - Other physical collecteables (marbles, tokens, etc)
///  - FIFO(stack) vs LIFO(Queue)
///  - Location ~ physical - Only one collection can own a card ? ie a collection is a pile of cards & a card can only be in one place
///  - Owner ~ virtual
///  
/// Virtual card collection (like all cardes owned (aka filtered by owner) property)
/// Physical card collection
/// 
/// Scripteable Object Enum pattern for typing (owner, list of properties)
///     Done by equality comparison (ie is this the same .asset file/scripteable object ref)
///     Exmples
///      Owner:ScripteableObject{}
///       Card1.owner = Alex.asset
///       Card2.owner = Pepijn.asset
///      Owners:
///       Alex.asset
///       Pepijn.asset
///      if(Card.owner == owner) {dosomething(); }
///     Properties     
///         Element
///         
///     Generic Properties
///      PropertyType:ScripteableObject
///      Property:ScripteableObject{PropertyType Type}
///      Dictionary<PropertyType.asset, Property.asset></Property>
///      FireElement.asset typeof(Property)
///      Element.asset typeof(Propertytype)
///     Specific Properties
///      Create a class of propertytype:
///      Element:PropertyType/ScripteableObject
///      FireElement.asset typeof(Element)
/// 
/// 
/// Clear base functions:
///  Flip(CardCollection c) - reverse order
///  Shuffle(CardCollection c) - random order
///  
/// Static functions?
///  Add/MoveTo/ChangePhysicalCollection(CardCollection to, Card n) (auto unsubscribe from old collection)
///  MoveAll(CardCollection from, CardCollection to) (Move all from one set to another)
///  MoveSpecific(CardCollection from, CardCollection to, FilterFunc filter)
///  
///  ? GatherAll(filterfunc, CardCollection to) (to quickly gather all of a certain type (ie owned by a player or another property filtereable upon)
///  ? Combine(CordColleciton c1, CardCollection c2)
/// 
/// Implementation:
///  Collection list?
///  
/// Where to store collections?
///  In some singleton like place?
///  CardCollections
/// </summary>
///

#endregion

[Serializable]
public class PlayerCardCollections
{
    public ICollection<Card> Hand;
    public IOrderedCollection<Card> DiscardPile, DrawPile;

    public PlayerCardCollections(ICollection<Card> hand, IOrderedCollection<Card> discardPile, IOrderedCollection<Card> drawPile)
    {
        Hand = hand;
        DiscardPile = discardPile;
        DrawPile = drawPile;
    }
}

public interface IPhysicalCollecteable<Collecteable>
{
    ICollection<Collecteable> PhysicalCollection { get; set; }
}

public interface IMoveCollection<CollecteableType> 
{
    void MoveAllTo(ICollection<CollecteableType> destination);
    void MoveSpecificTo(ICollection<CollecteableType> destination, Func<CollecteableType,bool> isSpecificFilter);
}

public interface IOrderedCollection<CollecteableType> : ICollection<CollecteableType>
{
    void Flip();
    void Shuffle();
    CollecteableType DrawTop();
}

public interface IAccessor<Type>
{
    Type this[int index] { get; set; }
}

[Serializable]
public class PhysicalStackCollection<CollecteableType> : StackCollection<CollecteableType> where CollecteableType : IPhysicalCollecteable<CollecteableType>
{
    public override void Add(CollecteableType item)
    {
        if(item.PhysicalCollection != null)
            item.PhysicalCollection.Remove(item);   // unsuscribe from old collection
        item.PhysicalCollection = this;             // new collectionowner = this
        collecteables.Add(item);                
    }

    public override bool Remove(CollecteableType item)
    {
        if(item.PhysicalCollection == this)
            item.PhysicalCollection = null;
        return collecteables.Remove(item);
    }
}

[Serializable]
public class StackCollection<CollecteableType> : ICollection<CollecteableType>, IAccessor<CollecteableType>, IOrderedCollection<CollecteableType>, IMoveCollection<CollecteableType>
{
    [SerializeField]
    protected List<CollecteableType> collecteables;

    public StackCollection()
    {
        collecteables = new List<CollecteableType>();
    }

    public int Count { get { return collecteables.Count; } }

    public bool IsReadOnly { get { return false; } }

    public CollecteableType this[int index]
    {
        get { return collecteables[index]; }
        set
        {
            Add(value);
            Swap(Count - 1, index);
            Remove(collecteables[Count - 1]);
        }
    }

    public virtual void Add(CollecteableType item)
    {
        collecteables.Add(item);
    }

    public virtual bool Remove(CollecteableType item)
    {
        return collecteables.Remove(item);
    }

    public CollecteableType DrawTop()
    {
        CollecteableType d = collecteables[collecteables.Count - 1];
        Remove(d);
        return d;
    }

    public void Clear()
    {
        collecteables.Clear();
    }

    public void MoveAllTo(ICollection<CollecteableType> destination)
    {
        var selection = collecteables.ToList();
        selection.ForEach(item => { destination.Add(item); });

        collecteables.Clear();
    }

    public void MoveSpecificTo(ICollection<CollecteableType> destination, Func<CollecteableType, bool> isSpecificFilter)
    {
        var selection = collecteables.Where(isSpecificFilter).ToList();
        selection.ForEach(item => { destination.Add(item); Remove(item); } );
    }

    public bool Contains(CollecteableType item)
    {
        return collecteables.Contains(item);
    }

    public void Flip()
    {
        #region SampleData
        // 0 1 2 3 4 5 6 7 8 9
        // a b c d e f g h i j  -> (int)(count/2)
        // j i h g f e d c b a
        // a b c
        // c b a
        // swap 0 & 9 a[i] & a[
        // swap 1 & 8
        // swap 2 & 7
        // swap 3 & 6
        // swap 4 & 5
        #endregion
        int count = collecteables.Count;
        int amount = count / 2;
        for(int i = 0; i < amount; i++)
        {
            Swap(i, count - 1 - i);
        }
    }

    public void Shuffle()
    {
        // Fisher-Yates Shuffle (https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle)
        for (int i = collecteables.Count -1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i);         // find a new random index
            Swap(i, j);                                     // Swap the soon to be out of algorithm value with a random value
        }
    }

    private void Swap(int i, int j)
    {
        CollecteableType replaced = collecteables[j];   // Swap the soon to be out of algorithm value with a random value
        collecteables[j] = collecteables[i];
        collecteables[i] = replaced;
    }

    public void CopyTo(CollecteableType[] array, int arrayIndex)
    {
        collecteables.CopyTo(array, arrayIndex);
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        int i = 0;
        builder.Append(string.Format("Collection {0} of {1} {2}s contains: \n", this.GetType(), collecteables.Count(), typeof(CollecteableType).ToString()));
        collecteables.ForEach(x =>
        {
            builder.Append(string.Format("{0} - {1} \n",i,x.ToString()));
            i++;
        });
        return builder.ToString();
    }

    public IEnumerator<CollecteableType> GetEnumerator()
    {
        return collecteables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return collecteables.GetEnumerator();
    }
}


