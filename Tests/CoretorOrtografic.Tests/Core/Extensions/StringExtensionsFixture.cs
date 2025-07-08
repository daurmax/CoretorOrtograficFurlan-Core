using CoretorOrtografic.Core.Extensions;
using NUnit.Framework;

namespace CoretorOrtografic.Tests.Core
{
    public class StringExtensionsFixture
    {
        [Test]
        public void ToFirstCharacterUpper_RegularWord_ReturnsCapitalized()
        {
            var result = "cjase".ToFirstCharacterUpper();
            Assert.That(result, Is.EqualTo("Cjase"));
        }

        [Test]
        public void ToFirstCharacterUpper_WordStartingWithApostrophe_ReturnsCapitalized()
        {
            var result = "'cjase".ToFirstCharacterUpper();
            Assert.That(result, Is.EqualTo("'Cjase"));
        }

        [Test]
        public void ToFirstCharacterUpper_SingleApostrophe_ReturnsUnchanged()
        {
            var result = "'".ToFirstCharacterUpper();
            Assert.That(result, Is.EqualTo("'"));
        }

        [Test]
        public void ToFirstCharacterUpper_NullOrEmpty_ReturnsOriginal()
        {
            Assert.That(((string)null).ToFirstCharacterUpper(), Is.Null);
            Assert.That("".ToFirstCharacterUpper(), Is.EqualTo(""));
        }
    }
}