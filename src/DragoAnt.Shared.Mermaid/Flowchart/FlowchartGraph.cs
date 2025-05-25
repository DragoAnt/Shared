using System;
using System.Collections.Generic;
using System.Linq;
using DragoAnt.Shared.Mermaid.Flowchart.Interaction;
using DragoAnt.Shared.Text;

namespace DragoAnt.Shared.Mermaid.Flowchart;

public sealed class FlowchartGraph : ElementBase
{
    private static string RootItemName = "###root###";

    private readonly Dictionary<string, FlowchartGraphItem> _items;
    private readonly List<FlowchartRelation> _relations;

    public FlowchartGraph()
    {
        Root = new FlowchartGraphItem(RootItemName, this, null);
        _items = new Dictionary<string, FlowchartGraphItem> { { Root.Id, Root } };
        _relations = [];
    }

    internal FlowchartGraphItem Root { get; }

    public FlowchartGraphDirection Direction
    {
        get => Root.Direction;
        set => Root.Direction = value;
    }

    private Dictionary<string, FlowchartStyleClass> StyleClasses { get; } = new();

    public FlowchartGraphItem? FindItem(string id)
    {
        _items.TryGetValue(id, out var item);
        return item;
    }

    public FlowchartGraphItem GetOrAdd(string id, string? name = null, int countChildren = 0, string? parentItemId = null,
        FlowchartGraphDirection? direction = null,
        string? styleClassId = null,
        FlowchartShape? shape = null)
    {
        var item = FindItem(id);
        if (item is null)
        {
            var parent = Root;
            if (parentItemId is { })
            {
                parent = FindItem(parentItemId);
                if (parent is null)
                {
                    throw new ArgumentException($"Can't find graph item '{parentItemId}'", nameof(parentItemId));
                }
            }
            item = parent.Add(id, name, countChildren);
        }

        if (name is { })
        {
            item.Caption = name;
        }
        if (direction is { } d)
        {
            item.Direction = d;
        }
        if (shape is { } s)
        {
            item.Shape = s;
        }
        if (styleClassId is not null)
        {
            item.StyleClassId = styleClassId;
        }
        return item;
    }

    public bool RemoveItem(string id)
    {
        if (id == RootItemName)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Can't remove root item");
        }

        if (!_items.TryGetValue(id, out var item))
        {
            return false;
        }

        SetStyleClassToItem(null, item);
        item.Parent!._children.Remove(item);
        return _items.Remove(id);
    }

    public bool ChangeItemId(string id, string newId)
    {
        var item = FindItem(id);
        if (item is null)
        {
            throw new ArgumentException("Can't find item", nameof(id));
        }
        var existedItem = FindItem(newId);
        if (existedItem is not null)
        {
            return false;
        }
        item.ChangeId(newId);
        _items.Remove(id);
        _items.Add(item.Id, item);
            
        return true;
    }

    internal void AddItem(FlowchartGraphItem item)
    {
        _items.Add(item.Id, item);
        item.Parent!._children.Add(item);
    }

    public FlowchartStyleClass? FindStyleClass(string? id)
    {
        if (id is null)
        {
            return null;
        }
        StyleClasses.TryGetValue(id, out var ret);
        return ret;
    }

    public FlowchartStyleClass GetOrAddStyleClass(string? id = null)
    {
        string GetAutoId()
        {
            var index = StyleClasses.Count;
            do
            {
                id = $"class{++index}";
            } while (StyleClasses.ContainsKey(id));
            return id;
        }

        return FindStyleClass(id)
               ?? AddStyleClass(new FlowchartStyleClass(id ?? GetAutoId(), this));
    }

    public FlowchartStyleClass AddStyleClass(FlowchartStyleClass styleClass)
    {
        if (!ReferenceEquals(styleClass.Graph, this))
        {
            styleClass = styleClass.Copy(styleClass.Id, this);
        }
        StyleClasses.Add(styleClass.Id, styleClass);
        return styleClass;
    }

    public void AddItemInteractionCallback(string itemId, string reference, string? toolTip = null)
    {
        AddItemInteractionCallback(itemId, config => reference, toolTip);
    }

    public void AddItemInteractionCallback(string itemId, Func<MermaidPrintConfig, string> reference, string? toolTip = null)
    {
        AddInteraction(itemId, new FlowchartInteractionCallback(reference, toolTip));
    }

    public void AddItemInteractionLink(string itemId, string link, string? toolTip = null,
        FlowchartInteractionLinkOpenType openType = FlowchartInteractionLinkOpenType.Blank)
    {
        AddInteraction(itemId, new FlowchartInteractionLink(link, toolTip, openType));
    }

    public void AddItemInteractionTooltip(string itemId, string toolTip)
    {
        AddInteraction(itemId, new FlowchartInteractionTooltip(toolTip));
    }

    private void AddInteraction(string itemId, FlowchartInteraction intercation)
    {
        var item = FindItem(itemId);
        if (item == null)
        {
            throw new ArgumentOutOfRangeException(nameof(itemId), $"Can;t find graph item '{itemId}'");
        }
        intercation.Item = item;
        item.Interaction = intercation;
    }

    public bool RemoveStyleClass(string id)
    {
        if (!StyleClasses.TryGetValue(id, out var styleClass))
        {
            return false;
        }

        foreach (var item in styleClass.Items)
        {
            item._styleClassId = null;
        }
        return StyleClasses.Remove(id);
    }

    internal void SetStyleClassToItem(string? id, FlowchartGraphItem item)
    {
        if (item._styleClassId == id)
        {
            return;
        }

        if (item._styleClassId is { } prevClassId)
        {
            var styleClass = FindStyleClass(prevClassId);
            styleClass?.Items.RemoveAll(i =>i.Id== item.Id);
        }
        if (id is { } classId)
        {
            var styleClass = FindStyleClass(classId);
            if (styleClass is null)
            {
                throw new ArgumentException($"Can't find style class '{id}'", nameof(id));
            }
            styleClass.Items.Add(item);
        }
        item._styleClassId = id;
    }

    public FlowchartRelation AddRelation(string leftItemId, string rightItemId, string? caption = null,
        FlowchartRelationLineEnding leftItemEnding = FlowchartRelationLineEnding.None,
        FlowchartRelationLineStyle lineStyle = FlowchartRelationLineStyle.Line,
        int lineLength = 0,
        FlowchartRelationLineEnding rightItemEnding = FlowchartRelationLineEnding.Arrow)
    {
        var leftItem = GetOrAdd(leftItemId);
        var rightItem = GetOrAdd(rightItemId);

        var relation = new FlowchartRelation(leftItem, rightItem, caption, leftItemEnding, lineStyle, lineLength, rightItemEnding);
        _relations.Add(relation);

        return relation;
    }

    public void CleanNodesWithoutRelations(Func<FlowchartGraphItem, bool>? leaveItems = null)
    {
        leaveItems ??= _ => false;

#if !NETSTANDARD2_0
        var itemsIds = _items.Keys.ToHashSet();
#else
            var itemsIds =new HashSet<string>(_items.Keys);
#endif
            
        foreach (var relation in _relations)
        {
            itemsIds.Remove(relation.LeftItem.Id);
            itemsIds.Remove(relation.RightItem.Id);
        }

        List<string> deletedIds = [];
        var repeat = true;
        while (repeat)
        {
            deletedIds.Clear();
            repeat = false;

            foreach (var itemId in itemsIds)
            {
                var item = FindItem(itemId);
                if (item is null)
                {
                    deletedIds.Add(itemId);
                    continue;
                }
                if (item.Children.Count == 0 && !leaveItems(item))
                {
                    deletedIds.Add(itemId);
                    RemoveItem(item);
                    repeat = true;
                }
            }
                
            foreach (var deletedId in deletedIds)
            {
                itemsIds.Remove(deletedId);
            }
        }
    }

    /// <inheritdoc />
    protected internal override bool Print(AdvStringBuilder builder, MermaidPrintConfig config)
    {
        Root.Print(builder, config);
        builder.AppendLine();
        builder.AppendLine();

        builder.AppendLine("%%% Relations");

        foreach (var relation in _relations)
        {
            if (relation.Print(builder, config))
            {
                builder.AppendLine();
            }
        }

        builder.AppendLine();

        builder.AppendLine("%%% Style classes");

        foreach (var styleClass in StyleClasses.Values.Where(s => s.Items.Count > 0 && s.Modifiers.Count > 0))
        {
            if (styleClass.Print(builder, config))
            {
                builder.AppendLine();
                builder.AppendLine();
            }
        }

        foreach (var item in _items.Values.OrderBy(i=>i.Id))
        {
            if (item.Interaction is {} interaction)
            {
                interaction.Print(builder, config);
                builder.AppendLine();
            }
        }
            
        return true;
    }
}