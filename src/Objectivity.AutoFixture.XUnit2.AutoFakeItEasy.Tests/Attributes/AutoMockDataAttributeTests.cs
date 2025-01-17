﻿namespace Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using FakeItEasy;
    using FluentAssertions;
    using global::AutoFixture;
    using global::AutoFixture.AutoFakeItEasy;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Xunit;
    using Xunit.Sdk;

    [Collection("AutoMockDataAttribute")]
    [Trait("Category", "Attributes")]
    public class AutoMockDataAttributeTests
    {
        [Fact(DisplayName = "WHEN parameterless constructor is invoked THEN fixture and attribute provider are created")]
        public void WhenParameterlessConstructorIsInvoked_ThenFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            // Act
            var attribute = new AutoMockDataAttribute();

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.Provider.Should().NotBeNull();
            attribute.IgnoreVirtualMembers.Should().BeFalse();
        }

        [Theory(DisplayName = "WHEN GetData is invoked THEN fixture is configured and data returned")]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void WhenGetDataIsInvoked_ThenFixtureIsConfiguredAndDataReturned(bool ignoreVirtualMembers)
        {
            // Arrange
            var data = new[]
            {
                new object[] { 1, 2, 3 },
                new object[] { 4, 5, 6 },
                new object[] { 7, 8, 9 }
            };
            var fixture = A.Fake<IFixture>();
            var customizations = new List<ICustomization>();
            A.CallTo(() => fixture.Customize(A<ICustomization>._))
                .Invokes((ICustomization customization) => customizations.Add(customization))
                .Returns(fixture);
            var dataAttribute = A.Fake<DataAttribute>();
            A.CallTo(() => dataAttribute.GetData(A<MethodInfo>._)).Returns(data);
            var provider = A.Fake<IAutoFixtureAttributeProvider>();
            A.CallTo(() => provider.GetAttribute(A<IFixture>._)).Returns(dataAttribute);
            var attribute = new AutoMockDataAttribute(fixture, provider)
            {
                IgnoreVirtualMembers = ignoreVirtualMembers
            };
            var methodInfo = typeof(AutoMockDataAttributeTests).GetMethod(nameof(this.TestMethod), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            var result = attribute.GetData(methodInfo);

            // Assert
            result.Should().BeSameAs(data);
            A.CallTo(() => provider.GetAttribute(A<IFixture>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => dataAttribute.GetData(A<MethodInfo>._)).MustHaveHappenedOnceExactly();

            customizations.Count.Should().Be(2);
            customizations[0]
                .Should()
                .BeOfType<AutoDataCommonCustomization>()
                .Which.IgnoreVirtualMembers.Should()
                .Be(ignoreVirtualMembers);
            customizations[1].Should().BeOfType<AutoFakeItEasyCustomization>();
        }

        [AutoMockData]
        [Theory(DisplayName = "GIVEN test method has some parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeParameters_WhenTestRun_ThenParametersAreGenerated(int value, IDisposable disposable)
        {
            // Arrange
            // Act
            // Assert
            value.Should().NotBe(default(int));

            disposable.Should().NotBeNull();
            disposable.GetType().Name.Should().EndWith("Proxy", "that way we know it was mocked.");
        }

        protected void TestMethod()
        {
        }
    }
}
