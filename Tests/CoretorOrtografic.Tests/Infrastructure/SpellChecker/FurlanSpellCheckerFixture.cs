using Components.CoretorOrtografic.Entities.ProcessedElements;
using CoretorOrtografic.Core.SpellChecker;
using Autofac;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoretorOrtografic.Tests.Infrastructure.SpellChecker
{
    public class FurlanSpellCheckerFixture
    {
        private static IContainer Container { get; set; }

        [SetUp]
        public void Setup()
        {
            Container = CoretorOrtograficTestDependencyContainer.Configure(true);
        }

        [Test]
        public async Task CheckWord_CorrectWord_ReturnsTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("cjape");
                bool result = await spellChecker.CheckWord(word);

                Assert.That(result);
            }
        }

        [Test]
        public async Task CheckWord_IncorrectWord_ReturnsFalse()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("incorrectword");
                bool result = await spellChecker.CheckWord(word);

                Assert.That(!result);
            }
        }

        [Test]
        public async Task GetWordSuggestions_InvalidWordWithSuggestions_ReturnsSuggestions()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("cjupe");
                var actualSuggestions = await spellChecker.GetWordSuggestions(word);

                Assert.That(actualSuggestions is not null);
                Assert.That(actualSuggestions.Any());

                var expectedSuggestions = new List<string> { "cjape", "cope", "copi", "sope", "supe", "copii", "cjepe", "supi", "zupe", "copiii" };
                Assert.That(actualSuggestions, Is.EqualTo(expectedSuggestions));
            }
        }

        [Test]
        public async Task GetWordSuggestions_InvalidWordNoSuggestions_ReturnsEmptySuggestions()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("invalidwordnosuggestions");
                var suggestions = await spellChecker.GetWordSuggestions(word);

                Assert.That(suggestions is not null);
                Assert.That(!suggestions.Any());
            }
        }

        [Test]
        public async Task CheckWord_WordWithDigits_ReturnsTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("abc123");
                bool result = await spellChecker.CheckWord(word);

                Assert.That(result);
            }
        }

        [Test]
        public async Task CheckWord_WordStartingWithLApostrophe_ReturnsTrue()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("l'cjape");
                bool result = await spellChecker.CheckWord(word);

                Assert.That(result);
            }
        }

        [Test]
        public void ExecuteSpellCheck_ShortWordsAreMarkedCorrect()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                spellChecker.ExecuteSpellCheck("ai cjape");
                var firstWord = spellChecker.ProcessedWords.First() as ProcessedWord;

                Assert.That(firstWord.Correct);
            }
        }

        [Test]
        public async Task GetWordSuggestions_HyphenatedWord_ReturnsCombinedSuggestion()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var spellChecker = scope.Resolve<ISpellChecker>();

                var word = new ProcessedWord("cjupe-cjase");
                var suggestions = await spellChecker.GetWordSuggestions(word);

                Assert.That(suggestions, Does.Contain("cjape cjase"));
            }
        }
    }
}
