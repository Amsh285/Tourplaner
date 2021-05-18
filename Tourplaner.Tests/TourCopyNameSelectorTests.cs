using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

namespace Tourplaner.Tests
{
    public sealed class TourCopyNameSelectorTests
    {
        [SetUp]
        public void Setup()
        {
            tourNames = new List<string>()
            {
                "asdf",
                "wasd_copy_1",
                "wasd_copy_2",
                "abc_copy_1_copy_1",

                "pg1",
                "qwertz",
                "qwerty",

                $"Too_long_iterator_copy_{int.MaxValue}",
                $"Negative_iterator_copy_{int.MinValue}"
            };

            sut = new CopyNameSelector();
        }

        [Test]
        public void CopyTourName_GeneratesValidSuffix()
        {
            string result = GetTourName("asdf");
            NUnit.Framework.Assert.AreEqual("asdf_copy_1", result);
        }

        [Test]
        public void CopyTourName_GeneratesValidSuffix_WithValidIncrement()
        {
            string result = GetTourName("wasd");
            NUnit.Framework.Assert.AreEqual("wasd_copy_3", result);
        }

        [Test]
        public void CopyTourName_OfCopy_GeneratesValidSuffix()
        {
            string result = GetTourName("wasd_copy_1");
            NUnit.Framework.Assert.AreEqual("wasd_copy_1_copy_1", result);
        }

        [Test]
        public void CopyTourName_OfCopy_GeneratesValidSuffix_WithValidIncrement()
        {
            string result = GetTourName("abc_copy_1");
            NUnit.Framework.Assert.AreEqual("abc_copy_1_copy_2", result);
        }

        [Test]
        public void CopyTourName_WithNegativeIterator_Throws()
        {
            NUnit.Framework.Assert.Throws<InvalidOperationException>(() => GetTourName("Negative_iterator", maxLength: 300));
        }

        [Test]
        public void CopyTourName_WithTooLongIterator_Throws()
        {
            NUnit.Framework.Assert.Throws<InvalidOperationException>(() => GetTourName("Too_long_iterator", maxLength:300));
        }

        [Test]
        public void CopyTourName_WithTooMuchCharacters_Throws()
        {
            NUnit.Framework.Assert.Throws<InvalidOperationException>(() => GetTourName("asdasasdadsasdd", maxLength: 3));
        }

        private string GetTourName(string originalTourName, int maxLength = 100)
        {
            return sut.GetCopyName(originalTourName, tourNames, maxLength);
        }

        private List<string> tourNames;
        private CopyNameSelector sut;
    }
}
