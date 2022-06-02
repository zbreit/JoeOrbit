using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StackInventoryTest
{
    private readonly ItemScriptableObject waterBucket = ScriptableObject.CreateInstance<ItemScriptableObject>();
    private readonly ItemScriptableObject rock = ScriptableObject.CreateInstance<ItemScriptableObject>();

    public StackInventoryTest()
    {
        rock.maxStackSize = 20;
        waterBucket.maxStackSize = 40;
    }

    [Test]
    public void GetCountIsZeroForNonexistentItems()
    {
        StackInventory inventory = new();
        Assert.Zero(inventory.GetCount(waterBucket));
    }

    [Test]
    public void GetCountReturnsCorrectNumberOfItems()
    {
        StackInventory inventory = new();

        inventory.AddMany(waterBucket, 12);
        inventory.Add(rock);
        inventory.Add(waterBucket);

        Assert.AreEqual(13, inventory.GetCount(waterBucket));
    }

    [Test]
    public void AddReturnsCorrectNumberOfItems()
    {
        StackInventory inventory = new();
        int added = inventory.AddMany(rock, 8);
        Assert.AreEqual(8, added);
    }

    [Test]
    public void AddManyCreatesNewStacksWhenNeeded()
    {
        StackInventory inventory = new();
        int added = inventory.AddMany(rock, rock.maxStackSize + 1);

        Assert.AreEqual(2, inventory.UsedSlotCount);
        Assert.AreEqual(rock.maxStackSize + 1, added);
    }

    [Test]
    public void AddManyIsLimitedByInventorySize()
    {
        int slotCount = 10;
        StackInventory inventory = new(slotCount);
        int added = inventory.AddMany(rock, rock.maxStackSize * slotCount + 1);

        Assert.AreEqual(rock.maxStackSize * slotCount, added);
    }

    [Test]
    public void AddGroupsSequentialItems()
    {
        StackInventory inventory = new();
        inventory.Add(rock);
        inventory.Add(rock);

        Assert.AreEqual(2, inventory.GetCount(rock));
        Assert.AreEqual(1, inventory.UsedSlotCount);
    }

    [Test]
    public void AddSeparatesItemsBasedOnAddTime()
    {
        StackInventory inventory = new();
        inventory.Add(rock);
        inventory.Add(waterBucket);
        inventory.Add(rock);

        Assert.AreEqual(2, inventory.GetCount(rock));
        Assert.AreEqual(3, inventory.UsedSlotCount);
    }

    [Test]
    public void ComputedFieldsAreAccurate()
    {
        int slotCount = 10;
        StackInventory inventory = new(slotCount);

        Assert.True(inventory.IsEmpty);

        // Add some items, leaving one slot open at the end
        inventory.AddMany(rock, rock.maxStackSize * (slotCount - 1));
        Assert.False(inventory.IsEmpty);
        Assert.AreEqual(slotCount - 1, inventory.UsedSlotCount);
        Assert.False(inventory.UsedAllSlots);
        Assert.False(inventory.AtCapactiy);

        // Add one more item, which adds an additional stack of items
        inventory.Add(rock);
        Assert.AreEqual(slotCount, inventory.UsedSlotCount);
        Assert.True(inventory.UsedAllSlots);
        Assert.False(inventory.AtCapactiy);

        // Fill up the inventory
        inventory.AddMany(rock, rock.maxStackSize - 1);
        Assert.True(inventory.AtCapactiy);
    }


    [Test]
    public void RemoveReturnsNullForEmptyInventory()
    {
        StackInventory inventory = new();
        Assert.Null(inventory.RemoveItem());
        Assert.Null(inventory.RemoveStack());
    }

    [Test]
    public void RemoveReturnsValidItemReference()
    {
        StackInventory inventory = new();
        inventory.Add(rock);
        Assert.AreEqual(rock, inventory.RemoveItem());
    }

    [Test]
    public void RemoveStackReturnsValidStack()
    {
        StackInventory inventory = new();

        inventory.AddMany(waterBucket, 3);
        inventory.AddMany(waterBucket, 3);
        ItemStack stack = inventory.RemoveStack();

        Assert.AreEqual(waterBucket, stack.item);
        Assert.AreEqual(6, stack.count);
    }

    [Test]
    public void RemoveItemReduceSlotCountWhenStackIsExhausted()
    {
        StackInventory inventory = new();
        inventory.Add(rock);
        inventory.RemoveItem();
        Assert.Zero(inventory.UsedSlotCount);
    }
}
