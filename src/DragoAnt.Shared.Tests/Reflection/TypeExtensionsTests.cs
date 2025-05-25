using System;
using System.Collections.Generic;
using System.Reflection;
using DragoAnt.Shared.Reflection;

namespace DragoAnt.Shared.Tests.Reflection;

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(int), false, "Int32")]
    [InlineData(typeof(int?), false, "Int32?")]
    [InlineData(typeof(List<int?>), false, "List<Int32?>")]
    [InlineData(typeof(List<object>), false, "List<Object>")]
#pragma warning disable xUnit1025
    [InlineData(typeof(List<object?>), false, "List<Object>")]
#pragma warning restore xUnit1025
    [InlineData(typeof(int), true, "System.Int32")]
    [InlineData(typeof(int?), true, "System.Int32?")]
    [InlineData(typeof(List<int?>), true, "System.Collections.Generic.List<System.Int32?>")]
    [InlineData(typeof(List<object>), true, "System.Collections.Generic.List<System.Object>")]
#pragma warning disable xUnit1025
    [InlineData(typeof(List<object?>), true, "System.Collections.Generic.List<System.Object>")]
#pragma warning restore xUnit1025
    public void HumanizeNameTest(Type type, bool fullName, string expected)
    {
        type.HumanizeName(fullName).Should().Be(expected);
        type.GetTypeInfo().HumanizeName(fullName).Should().Be(expected);
    }
}