﻿namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using FluentAssertions;
    using Moq;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    [Collection("InlineAutoMoqDataBaseAttribute")]
    [Trait("Category", "Attributes")]
    public class InlineAutoMoqDataBaseAttributeTests
    {
        [Theory(DisplayName = "GIVEN existing fixture and attribute provider WHEN constructor is invoked THEN has fixture attribute provider and no values")]
        [AutoData]
        public void GivenExistingFixtureAndAttributeProvider_WhenConstructorIsInvoked_ThenHasFixtureAttributeProviderAndNoValues(Fixture fixture)
        {
            // Arrange
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();

            // Act
            var attribute = new InlineAutoMoqDataBaseAttributeUnderTest(fixture, provider.Object);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().Be(provider.Object);
            attribute.Values.Should().HaveCount(0);
        }

        [Theory(DisplayName = "GIVEN existing fixture, attribute provider and values WHEN constructor is invoked THEN has specified fixture, attribute provider and values")]
        [AutoData]
        public void GivenExistingFixtureAttributeProviderAndValues_WhenConstructorIsInvoked_ThenHasSpecifiedFixtureAttributeProviderAndValues(Fixture fixture)
        {
            // Arrange
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();
            var initialValues = new[] { "test", 1, new object() };

            // Act
            var attribute = new InlineAutoMoqDataBaseAttributeUnderTest(fixture, provider.Object, initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().Be(provider.Object);
            attribute.Values.Should().BeEquivalentTo(initialValues);
        }

        [Theory(DisplayName = "GIVEN existing fixture, attribute provider and uninitialized values WHEN constructor is invoked THEN has specified fixture, attribute provider and no values")]
        [AutoData]
        public void GivenExistingFixtureAttributeProviderAndUninitializedValues_WhenConstructorIsInvoked_ThenHasSpecifiedFixtureAttributeProviderAndNoValues(Fixture fixture)
        {
            // Arrange
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();
            const object[] initialValues = null;

            // Act
            var attribute = new InlineAutoMoqDataBaseAttributeUnderTest(fixture, provider.Object, initialValues);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().Be(provider.Object);
            attribute.Values.Should().HaveCount(0);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Fixture fixture = null;
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new InlineAutoMoqDataBaseAttributeUnderTest(fixture, provider.Object));
        }

        [Theory(DisplayName = "GIVEN uninitialized attribute provider WHEN constructor is invoked THEN exception is thrown")]
        [AutoData]
        public void GivenUninitializedAttributeProvider_WhenConstructorIsInvoked_ThenExceptionIsThrown(Fixture fixture)
        {
            // Arrange
            const IAutoFixtureInlineAttributeProvider provider = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new InlineAutoMoqDataBaseAttributeUnderTest(fixture, provider));
        }

        private class InlineAutoMoqDataBaseAttributeUnderTest : InlineAutoMoqDataBaseAttribute
        {
            public InlineAutoMoqDataBaseAttributeUnderTest(IFixture fixture, IAutoFixtureInlineAttributeProvider provider, params object[] values)
                : base(fixture, provider, values)
            {
            }

            protected override IFixture Customize(IFixture fixture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
