using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using Tourplaner.UI.Converters;

namespace Tourplaner.Tests
{
    public sealed class BooleanToVisibilityCollapsedConverterTests
    {
        [SetUp]
        public void Setup()
        {
            converter = new BooleanToVisibilityCollapsedConverter();
        }

        [Test]
        public void TrueReturnsVisibilityVisible()
        {
            object result = converter.Convert(true, typeof(bool), null, CultureInfo.InvariantCulture);

            Assert.IsInstanceOf<Visibility>(result);

            Visibility actual = (Visibility)result;
            Assert.AreEqual(Visibility.Visible, actual);
        }

        [Test]
        public void FalseReturnsVisibilityCollapsed()
        {
            object result = converter.Convert(false, typeof(bool), null, CultureInfo.InvariantCulture);

            Assert.IsInstanceOf<Visibility>(result);

            Visibility actual = (Visibility)result;
            Assert.AreEqual(Visibility.Collapsed, actual);
        }

        [Test]
        public void WrongTypeThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => converter.Convert(123, typeof(int), null, CultureInfo.InvariantCulture));
        }

        private BooleanToVisibilityCollapsedConverter converter;
    }
}
