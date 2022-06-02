using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StackInventory
{
    private readonly int slotLimit;
    private readonly bool usesSlotLimit;
    private readonly Stack<ItemStack> itemStacks = new();

    public int UsedSlotCount => itemStacks.Count;
    public bool UsedAllSlots => usesSlotLimit && UsedSlotCount == slotLimit;
    public bool AtCapactiy => UsedAllSlots && itemStacks.Peek().count == itemStacks.Peek().item.maxStackSize;
    public bool IsEmpty => UsedSlotCount == 0;


    public StackInventory(int slotLimit = 8, bool usesSlotLimit = true)
    {
        this.slotLimit = slotLimit;
        this.usesSlotLimit = usesSlotLimit;
    }

    public List<ItemStack> Clear()
    {
        var stacks = itemStacks.ToList();
        itemStacks.Clear();

        return stacks;
    }

    public bool HasItem(ItemScriptableObject item)
    {
        return GetCount(item) > 0;
    }

    public int GetCount(ItemScriptableObject item)
    {
        int count = 0;

        foreach (ItemStack stack in itemStacks)
        {
            if (stack.item.Equals(item))
            {
                count += stack.count;
            }
        }

        return count;
    }

    public int AddMany(ItemScriptableObject item, int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), count, $"The number of items to add must be positive");

        return ExhaustItems(item, count);
    }

    public ItemStack RemoveStack()
    {
        if (itemStacks.Count == 0)
            return null;

        return itemStacks.Pop();
    }

    public ItemScriptableObject RemoveItem()
    {
        if (itemStacks.Count == 0)
            return null;

        ItemStack currentStack = itemStacks.Peek();

        currentStack.count--;

        if (currentStack.count == 0)
            itemStacks.Pop();

        return currentStack.item;
    }

    public bool Add(ItemScriptableObject item)
    {
        return AddMany(item, 1) == 1;
    }

    public override string ToString()
    {
        StringBuilder sb = new("StackInventory { ");

        sb.AppendJoin(", ", itemStacks);

        sb.Append(" }");

        return sb.ToString();
    }


    // Helper Functions
    private int ExhaustItems(ItemScriptableObject item, int count)
    {
        int remaining = count;

        // If the current stack is of this same item, add items there first
        if (itemStacks.Count > 0 && itemStacks.Peek().item.Equals(item))
            remaining -= AddToCurrentStack(count);


        // Create new stacks of items while there are slots available
        while (remaining > 0 && !UsedAllSlots)
        {
            remaining -= CreateNewItemStack(item, remaining);
        }

        return count - remaining;
    }

    private int CreateNewItemStack(ItemScriptableObject item, int count)
    {
        // No more slots remaining!
        if (itemStacks.Count >= slotLimit)
            return 0;

        // Otherwise, create a new slot at the end of the list
        ItemStack newSlot = new() { item=item, count=0 };
        itemStacks.Push(newSlot);
        
        return AddToCurrentStack(count);
    }

    private int AddToCurrentStack(int count)
    {
        ItemStack currentStack = itemStacks.Peek();
        int oldCount = currentStack.count;
        int newCount = Mathf.Min(oldCount + count, currentStack.item.maxStackSize);

        currentStack.count = newCount;

        return newCount - oldCount;
    }
}

public class ItemStack
{
    public ItemScriptableObject item;
    public int count;

    public override string ToString()
    {
        return $"{count}x '{item.name}'";
    }
}
