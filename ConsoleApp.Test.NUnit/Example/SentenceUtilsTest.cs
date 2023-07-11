using ConsoleApp.Example;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.NUnit.Example
{
    [TestFixture]
    public class SentenceUtilsTest
    {
        [Theory]
        [TestCase("a", "A")]
        [TestCase("A", "A")]
        [TestCase("abc", "Abc")]
        [TestCase("Abc", "Abc")]
        [TestCase("aBc", "Abc")]
        [TestCase("abC", "Abc")]
        [TestCase("AbC", "Abc")]
        [TestCase("ABC", "Abc")]
        [TestCase("abc def ghi", "Abc Def Ghi")]
        [TestCase("  abc  def ghi ", "  Abc  Def Ghi ")]
        public void ToTitleCase_AnyInput_TitleCaseOutput(string intput, string output)
        {
            //Act
            var result = SentenceUtils.ToTitleCase(intput);
            //Assert
            Assert.That(result, Is.EqualTo(output));

        }

        //[Test]
        //public void Character()
        //{
        //    Assert.That(SentenceUtils.ToTitleCase("a"),
        //                Is.EqualTo("A"),
        //                "Your function should convert a single character.");
        //    Assert.That(SentenceUtils.ToTitleCase("A"),
        //                Is.EqualTo("A"),
        //                "Your function should convert a single character.");
        //}

        //[Test]
        //public void Word()
        //{
        //    Assert.That(SentenceUtils.ToTitleCase("abc"),
        //                Is.EqualTo("Abc"),
        //                "Your function should convert a single word.");
        //    Assert.That(SentenceUtils.ToTitleCase("ABC"),
        //                Is.EqualTo("Abc"),
        //                "Your function should convert a single word.");
        //    Assert.That(SentenceUtils.ToTitleCase("aBC"),
        //                Is.EqualTo("Abc"),
        //                "Your function should convert a single word.");
        //    Assert.That(SentenceUtils.ToTitleCase("Abc"),
        //                Is.EqualTo("Abc"),
        //                "Your function should convert a single word.");
        //}

        //[Test]
        //public void Sentence()
        //{
        //    Assert.That(SentenceUtils.ToTitleCase("abc def ghi"),
        //                Is.EqualTo("Abc Def Ghi"),
        //                "Your function should convert a sentence.");
        //}

        //[Test]
        //public void MultipleWhitespace()
        //{
        //    Assert.That(SentenceUtils.ToTitleCase("  abc  def ghi "),
        //                Is.EqualTo("  Abc  Def Ghi "),
        //                "Your function should keep multiple whitespaces.");
        //}
    }
}
