using System.Text;
using DragoAnt.Shared.Text;

namespace DragoAnt.Shared.Tests.Text;

public class AdvStringBuilderTests
{
    [Theory]
    [InlineData("  ")]
    [InlineData("\t")]
    public void CheckIdentTest(string identChunk)
    {
        var sb = new AdvStringBuilder(identChunk);
        sb.GetIdent().Should().Be(string.Empty);

        sb.AddIdent();
        sb.GetIdent().Should().Be(GetIdent(identChunk, 1));

        sb.RemoveIdent();
        sb.GetIdent().Should().Be(string.Empty);

        sb.AddIdent(3);
        sb.GetIdent().Should().Be(GetIdent(identChunk, 3));

        sb.RemoveIdent(2);
        sb.GetIdent().Should().Be(GetIdent(identChunk, 1));

        sb.RemoveIdent();
        sb.GetIdent().Should().Be(string.Empty);
    }

    private static string GetIdent(string identChunk, int ident)
    {
        var stringBuilder = new StringBuilder(identChunk.Length * ident);
        for (var i = 0; i < ident; i++)
        {
            stringBuilder.Append(identChunk);
        }

        return stringBuilder.ToString();
    }
}