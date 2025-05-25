#nullable enable
using FluentAssertions;
using NUnit.Framework;
using DragoAnt.Shared.Mermaid.Flowchart;

namespace DragoAnt.Shared.Mermaid.Tests;

[TestFixture]
public class FlowchartExtensionsTests
{
    [Test]
    public void CreationTest()
    {
        var graph = new FlowchartGraph { Direction = FlowchartGraphDirection.TD };

        //--Counterparties
        var counterpartiesClass = graph.GetOrAddStyleClass("counterpartiesClass")
            .SetFill("#ccffcc")
            .SetStrokeWidth("2px")
            .SetStrokeDashArray("2 2");

        graph.GetOrAdd("Domain_Counterparties", "Counterparties", 2, styleClassId: counterpartiesClass);
            
        graph.GetOrAdd("Country", parentItemId: "Domain_Counterparties", direction: FlowchartGraphDirection.LR);
        graph.GetOrAdd("Country_States","States#\"{}()[]<>%~='", parentItemId: "Country", shape: FlowchartShape.Hexagon);
            
        graph.GetOrAdd("CountryState", parentItemId: "Domain_Counterparties", direction: FlowchartGraphDirection.LR);
        graph.GetOrAdd("CountryState_Country","Country", parentItemId: "CountryState", shape: FlowchartShape.BoxRoundEdges);

        //--Client
        var clientClass = graph.AddStyleClass(counterpartiesClass.Copy("clientClass")
            .SetFill("#bbf"));

        graph.GetOrAdd("Domain_Client", "Client", 1, styleClassId: clientClass);
            
        graph.GetOrAdd("Person", parentItemId: "Domain_Client", direction: FlowchartGraphDirection.LR);
        graph.GetOrAdd("Person_Country", "Country", parentItemId: "Person", shape: FlowchartShape.BoxRoundEdges);

            
        graph.GetOrAdd("Domain_Standalone", "Standalone", 1);
        graph.GetOrAdd("EntityStandalone", "EntityStandalone", parentItemId: "Domain_Standalone");
            
        graph.AddRelation("Country", "Person_Country", "fa:fa-1 CountryId fa:fa-N", 
            FlowchartRelationLineEnding.None, 
            FlowchartRelationLineStyle.BoldLine,
            0, FlowchartRelationLineEnding.None);
        graph.AddRelation("Country", "Person_Country", "fa:fa-1 CountryId fa:fa-N", FlowchartRelationLineEnding.None, FlowchartRelationLineStyle.BoldLine, 3);
        graph.AddRelation("Country_States", "CountryState_Country", "fa:fa-1 CountryId #\"{}()[]<>%~=' fa:fa-N", FlowchartRelationLineEnding.Arrow, FlowchartRelationLineStyle.Dots);

        graph.AddItemInteractionCallback("CountryState_Country", $"call exampleCallback(DragoAnt.Domain.CountryState.Country)",
            "Click for copy R# search string");
            
        graph.AddItemInteractionLink("Person_Country", "https://google.com", "Test link to https://google.com");
            
        graph.AddItemInteractionTooltip("Country_States", "Test tooltip #{}()[]%~='");
            
        //graph.CleanNodesWithoutRelations();
            
        var outputEditor = graph.ToString(MermaidPrintConfig.Normal);
        var outputHtml = graph.ToString(MermaidPrintConfig.ForHtml);
    }

    [TestCase("Country#\"{}()[]<>%~='","Country#_____________")]
    public void TestRestrictedSymbols(string value, string expected)
    {
        MermaidHelper.ReplaceRestrictedSymbols(value).Should().Be(expected);
    }
}