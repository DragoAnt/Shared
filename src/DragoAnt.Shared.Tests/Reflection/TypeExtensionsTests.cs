#nullable enable
using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using DragoAnt.Shared.Reflection;

namespace DragoAnt.Shared.Tests.Reflection;

[TestFixture]
public class TypeExtensionsTests
{
    [TestCase(typeof(int), false, "Int32")]
    [TestCase(typeof(int?), false, "Int32?")]
    [TestCase(typeof(List<int?>), false, "List<Int32?>")]
    [TestCase(typeof(List<object>), false, "List<Object>")]
    [TestCase(typeof(List<object?>), false, "List<Object>")]
    [TestCase(typeof(int), true, "System.Int32")]
    [TestCase(typeof(int?), true, "System.Int32?")]
    [TestCase(typeof(List<int?>), true, "System.Collections.Generic.List<System.Int32?>")]
    [TestCase(typeof(List<object>), true, "System.Collections.Generic.List<System.Object>")]
    [TestCase(typeof(List<object?>), true, "System.Collections.Generic.List<System.Object>")]
    public void HumanizeNameTest(Type type, bool fullName, string expected)
    {
        type.HumanizeName(fullName).Should().Be(expected);
        type.GetTypeInfo().HumanizeName(fullName).Should().Be(expected);
    }
}