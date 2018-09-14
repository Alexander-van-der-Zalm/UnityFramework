using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class CollecteableTester
{
    private class PhysicalCollecteable : IPhysicalCollecteable<PhysicalCollecteable>
    {
        public int Number = 0;

        private ICollection<PhysicalCollecteable> physicalCollection;

        public ICollection<PhysicalCollecteable> PhysicalCollection
        {
            get { return physicalCollection; }
            set { physicalCollection = value; }
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }

    [Test]
    public void Add5x_EmptyCollection_Have5()
    {
        PhysicalStackCollection<PhysicalCollecteable> testDeck = new PhysicalStackCollection<PhysicalCollecteable>();
        5.Times(() => testDeck.Add(new PhysicalCollecteable()));
        Assert.IsTrue(testDeck.Count == 5);
    }

    [Test]
    public void Draw5x_Collectionof5times_Have0()
    {
        PhysicalStackCollection<PhysicalCollecteable> testDeck = new PhysicalStackCollection<PhysicalCollecteable>();
        5.Times(() => testDeck.Add(new PhysicalCollecteable()));
        5.Times(() => testDeck.DrawTop());
        Assert.IsTrue(testDeck.Count == 0);
    }

    [Test]
    public void Add_InCollectionNoOwner_HasOwner()
    {
        PhysicalStackCollection<PhysicalCollecteable> testDeck = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalCollecteable PhysicalCollecteable = new PhysicalCollecteable();

        Assert.IsNull(PhysicalCollecteable.PhysicalCollection);

        testDeck.Add(PhysicalCollecteable);

        Assert.AreEqual(PhysicalCollecteable.PhysicalCollection, testDeck);
    }

    [Test]
    public void Remove_InCollectionHasOwner_HasNoOwner()
    {
        PhysicalStackCollection<PhysicalCollecteable> testDeck = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalCollecteable PhysicalCollecteable = new PhysicalCollecteable();

        Assert.IsNull(PhysicalCollecteable.PhysicalCollection);

        testDeck.Add(PhysicalCollecteable);

        Assert.AreEqual(PhysicalCollecteable.PhysicalCollection, testDeck);

        testDeck.Remove(PhysicalCollecteable);

        Assert.IsNull(PhysicalCollecteable.PhysicalCollection);
    }

    [Test]
    public void Draw_InCollectionHasOwner_HasNoOwner()
    {
        PhysicalStackCollection<PhysicalCollecteable> testDeck = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalCollecteable collecteable = new PhysicalCollecteable();

        Assert.IsNull(collecteable.PhysicalCollection);

        testDeck.Add(collecteable);

        Assert.AreEqual(collecteable.PhysicalCollection, testDeck);

        PhysicalCollecteable drawn = testDeck.DrawTop();

        Assert.IsNull(collecteable.PhysicalCollection);
    }

    [Test]
    public void Draw1x_InCollection_IsOutOfCollection()
    {
        PhysicalStackCollection<PhysicalCollecteable> testDeck = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalCollecteable collecteable = new PhysicalCollecteable();

        testDeck.Add(collecteable);

        Assert.AreEqual(collecteable.PhysicalCollection, testDeck);

        PhysicalCollecteable drawn = testDeck.DrawTop();

        Assert.AreEqual(collecteable, drawn);
        Assert.IsFalse(testDeck.Contains(collecteable));
    }

    [Test]
    public void MoveAllTo_A5B0_A0B5()
    {
        PhysicalStackCollection<PhysicalCollecteable> A = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalStackCollection<PhysicalCollecteable> B = new PhysicalStackCollection<PhysicalCollecteable>();

        5.Times(() => A.Add(new PhysicalCollecteable()));

        Assert.IsTrue(A.Count == 5);
        Assert.IsTrue(B.Count == 0);

        A.MoveAllTo(B);

        Assert.IsTrue(A.Count == 0);
        Assert.IsTrue(B.Count == 5);
    }

    [Test]
    public void MoveAllTo_A5B0Ordered_A0B5SameOrder()
    {
        PhysicalStackCollection<PhysicalCollecteable> A = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalStackCollection<PhysicalCollecteable> B = new PhysicalStackCollection<PhysicalCollecteable>();

        int i = 0;
        5.Times(() => { A.Add(new PhysicalCollecteable() { Number = i }); i++; });

        Assert.IsTrue(A.Count == 5);
        Assert.IsTrue(B.Count == 0);

        i = 0;
        5.Times(() => { Assert.AreEqual(A[i].Number, i); i++; });

        A.MoveAllTo(B);

        Assert.IsTrue(A.Count == 0);
        Assert.IsTrue(B.Count == 5);

        i = 0;
        5.Times(() => { Assert.AreEqual(B[i].Number, i); i++; });
    }

    [Test]
    public void MoveSpecificTo_A5Spec5NormB0Spec0Norm_A0Spec5NormB5Spec0Norm()
    {
        PhysicalStackCollection<PhysicalCollecteable> A = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalStackCollection<PhysicalCollecteable> B = new PhysicalStackCollection<PhysicalCollecteable>();

        5.Times(() => A.Add(new PhysicalCollecteable() { Number = 0 }));
        5.Times(() => A.Add(new PhysicalCollecteable() { Number = 10 }));

        Assert.IsTrue(A.Count == 10);
        Assert.IsTrue(B.Count == 0);

        A.MoveSpecificTo(B,item => item.Number == 10);

        Assert.IsTrue(A.Count == 5);
        Assert.IsTrue(B.Count == 5);
    }

    [Test]
    public void MoveSpecificTo_A5Spec5NormB0Spec0NormOrdered_A0Spec5NormB5Spec0NormOrdered()
    {
        PhysicalStackCollection<PhysicalCollecteable> A = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalStackCollection<PhysicalCollecteable> B = new PhysicalStackCollection<PhysicalCollecteable>();

        int i = 0;
        10.Times(() => { A.Add(new PhysicalCollecteable() { Number = i }); i++; } );

        Assert.IsTrue(A.Count == 10);
        Assert.IsTrue(B.Count == 0);

        A.MoveSpecificTo(B, item => item.Number < 5);

        Assert.IsTrue(A.Count == 5);
        Assert.IsTrue(B.Count == 5);

        i = 0;
        5.Times(() => { Assert.AreEqual(B[i].Number, i); i++; });
    }

    [Test]
    public void AccessorGet_NumberedCollection_ItemHasNumber()
    {
        PhysicalStackCollection<PhysicalCollecteable> A = new PhysicalStackCollection<PhysicalCollecteable>();

        int i = 0;
        5.Times(() => { A.Add(new PhysicalCollecteable() { Number = i }); i++; });

        i = 0;
        5.Times(() => { Assert.AreEqual(A[i].Number, i); i++; });
    }

    [Test]
    public void AccessorSet_NumberedCollection_ReplacedCorrectly()
    {
        PhysicalStackCollection<PhysicalCollecteable> A = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalCollecteable replace = new PhysicalCollecteable() { Number = 6 };

        int i = 0;
        5.Times(() => { A.Add(new PhysicalCollecteable() { Number = i }); i++; });

        i = 0;
        5.Times(() => { Assert.AreEqual(A[i].Number, i); i++; });

        A[0] = replace;

        i = 1;
        4.Times(() => { Assert.AreEqual(A[i].Number, i); i++; });
        Assert.AreEqual(A[0].Number, 6);
    }

    [Test]
    public void Flip_CollectionInOrder_CollectionReverseOrder()
    {
        PhysicalStackCollection<PhysicalCollecteable> A = new PhysicalStackCollection<PhysicalCollecteable>();

        int i = 0;
        5.Times(() => { A.Add(new PhysicalCollecteable() { Number = i }); i++; });

        i = 0;
        5.Times(() => { Assert.AreEqual(A[i].Number, i); i++; });

        A.Flip();

        int j = 0;
        i = 4;
        5.Times(() => { Assert.AreEqual(A[j].Number, i); j++; i--; });
    }

    [Test]
    public void Shuffle100x_CollectionInOrder_SignificantOrderDifference()
    {
        PhysicalStackCollection<PhysicalCollecteable> A = new PhysicalStackCollection<PhysicalCollecteable>();
        PhysicalStackCollection<PhysicalCollecteable> B = new PhysicalStackCollection<PhysicalCollecteable>();

        int i = 0;
        5.Times(() => { A.Add(new PhysicalCollecteable() { Number = i }); i++; });
        i = 0;
        5.Times(() => { B.Add(new PhysicalCollecteable() { Number = i }); i++; });

        int different = 0;
        int tests = 100;
        for (int test = 0; test < tests; test++)
        {
            i = 0;
            5.Times(() => { Assert.IsTrue(A[i].Number == B[i].Number); i++; });

            B.Shuffle();

            i = 0;
            5.Times(() => { different += A[i].Number != B[i].Number ? 1 : 0; i++; } );

            //string order = "A: ";
            //i = 0;
            //5.Times(() => { order += A[i].Number; i++; });
            //i = 0;
            //order += " B: ";
            //5.Times(() => { order += B[i].Number; i++; });
            //Debug.Log(order);

            i = 0;                                      // Reset
            5.Times(() => {B[i].Number = i; i++; });
        }
        //Debug.Log(different);
        Assert.Greater(different, tests * 3);
    }
}
