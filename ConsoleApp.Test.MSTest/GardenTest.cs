using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.NUnit
{
    [TestClass]
    public class GardenTest
    {
        #region BAD PRACTISE
        private Garden Garden { get; set; }

        [TestInitialize]
        public void Prepare()
        {
            Garden = new Garden(0);
        }

        [TestCleanup]
        public void Clean()
        {
        }
        #endregion BAD PRACTISE


        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(int.MaxValue, int.MaxValue)]
        [DataRow(-1, 0)]
        [DataRow(int.MinValue, 0)]
        public void Garden_SizeInit(int initSize, int outputSize)
        {
            //Act
            var garden = new Garden(initSize);
            //Assert
            Assert.AreEqual(outputSize, garden.Size);
        }
        
        [TestMethod]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MIN_VALID_SIZE = 1;
            const string VALID_NAME = "a";
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            var result = garden.Plant(VALID_NAME);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
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
            Assert.IsFalse(result);
        }

        [TestMethod]
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
            Assert.IsTrue(garden.GetPlants().Contains(EXPECTED_NAME));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void Plant_InvalidName_ArgumentException(string invalidName)
        {
            //Arrange
            const int MIN_VALID_SIZE = default;
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            garden.Plant(invalidName);
        }

        [TestMethod]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrage
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.AreNotSame(result1, result2);
        }
    }
}
