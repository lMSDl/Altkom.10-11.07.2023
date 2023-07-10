using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.NUnit
{
    [TestFixture]
    public class GardenTest
    {
        #region BAD PRACTISE
        private Garden Garden { get; set; }

        [SetUp]
        public void Prepare()
        {
            Garden = new Garden(0);
        }

        [TearDown]
        public void Clean()
        {
        }
        #endregion BAD PRACTISE


        [Theory]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(-1, 0)]
        [TestCase(int.MinValue, 0)]
        public void Garden_SizeInit(int initSize, int outputSize)
        {
            //Act
            var garden = new Garden(initSize);
            //Assert
            Assert.That(garden.Size, Is.EqualTo(outputSize));
        }
        
        [Test]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MIN_VALID_SIZE = 1;
            const string VALID_NAME = "a";
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            var result = garden.Plant(VALID_NAME);

            //Assert
            //Assert.True(result);
            Assert.That(result, Is.True);
        }


        [Test]
        public void Plant_GardenOverflow_False()
        {
            //Arrange
            const int MAX_VALID_SIZE = 1;
            const string VALID_NAME = "a";
            var garden = new Garden(MAX_VALID_SIZE);
            garden.Plant(VALID_NAME);

            //Act
            var result = garden.Plant(VALID_NAME);

            //Assert
            Assert.False(result);
        }

        [Test]
        public void Plant_ExistingName_ChangedNameOnList()
        {
            //Arrange
            const int MAX_VALID_SIZE = 2;
            const string VALID_NAME = "a";
            const string EXPECTED_NAME = VALID_NAME + "2";
            var garden = new Garden(MAX_VALID_SIZE);
            garden.Plant(VALID_NAME);

            //Act
            garden.Plant(VALID_NAME);

            //Assert
            Assert.That(garden.GetPlants(), Does.Contain(EXPECTED_NAME));
        }

        [Theory]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\t")]
        public void Plant_InvalidName_ArgumentExceptionWithParameterName(string invalidName)
        {
            //Arrange
            const int MIN_VALID_SIZE = default;
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            TestDelegate result = () => garden.Plant(invalidName);

            //Assert
            Assert.Throws(Is.InstanceOf<ArgumentException>().And.Property(nameof(ArgumentException.ParamName)).EqualTo("plantName"), result);
        }

        [Test]
        [Ignore("Replaced by Plant_InvalidName_ArgumentExceptionWithParameterName")]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int MIN_VALID_SIZE = default;
            const string? NULL_NAME = null;
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            TestDelegate result = () => garden.Plant(NULL_NAME);

            //Assert
            Assert.Throws(Is.TypeOf<ArgumentNullException>().And.Property(nameof(ArgumentException.ParamName)).EqualTo("plantName"), result);

        }

        [Test]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrage
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.That(result1, Is.Not.SameAs(result2));
        }
    }
}
