using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.xUnit
{
    public class GardenTest : IDisposable
    {
        #region BAD PRACTISE
        private Garden Garden { get; }

        //odpowiednik SetUp w xUnit
        public GardenTest()
        {
            Garden = new Garden(0);
        }

        //odpowiednik TearDown w xUnit
        public void Dispose()
        {
        }
        #endregion BAD PRACTISE




        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(-1, 0)]
        [InlineData(int.MinValue, 0)]
        public void Garden_SizeInit(int initSize, int outputSize)
        {
            //Act
            var garden = new Garden(initSize);
            //Assert
            Assert.Equal(outputSize, garden.Size);
        }
        /*[Fact]
        public void Gardern_NegativeSize_ZeroSize()
        {
            //Arrange
            const int NEGATIVE_SIZE = -1;

            //Act
            var garden = new Garden(NEGATIVE_SIZE);

            //Assert
            Assert.Equal(0, garden.Size);
        }*/


        //<nazwa funkcji>_<scenariusz i opis rezultatu>
        //public void Plant_GivesTrueWhenProvidedValidName

        //<nawa funkcji>_<scenariusz>_<oczekiwany rezultat>
        //public void Plant_PassValidName_ReturnsTrue()
        [Fact]
        public void Plant_ValidName_True()
        {
            //Arrange
            const int MIN_VALID_SIZE = 1;
            const string VALID_NAME = "a";
            var garden = PrepareGarden(MIN_VALID_SIZE);

            //Act
            var result = garden.Plant(VALID_NAME);

            //Assert
            Assert.True(result);
        }

        //przy powtarzającym się kodzie Arrange zamiast metody SetUp zaleca się utworzenie zwykłej metody i ręczne wywołanie jej
        private static Garden PrepareGarden(int gardenSize)
        {
            return new Garden(gardenSize);
        }

        [Fact]
        public void Plant_GardenOverflow_False()
        {
            //Arrange
            const int MAX_VALID_SIZE = 1;
            const string VALID_NAME = "a";
            var garden = PrepareGarden(MAX_VALID_SIZE);
            garden.Plant(VALID_NAME);

            //Act
            var result = garden.Plant(VALID_NAME);

            //Assert
            Assert.False(result);
        }

        [Fact]
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
            Assert.Contains(EXPECTED_NAME, garden.GetPlants());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Plant_InvalidName_ArgumentExceptionWithParameterName(string invalidName)
        {
            //Arrange
            const int MIN_VALID_SIZE = default;
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            Action result = () => garden.Plant(invalidName);

            //Assert
            var argumentNullException = Assert.ThrowsAny<ArgumentException>(result);
            Assert.Equal("plantName", argumentNullException.ParamName, ignoreCase: true);
        }

        [Fact(Skip = "Replaced by Plant_InvalidName_ArgumentExceptionWithParameterName")]
        public void Plant_NullName_ArgumentNullException()
        {
            //Arrange
            const int MIN_VALID_SIZE = default;
            const string? NULL_NAME = null;
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            Action result = () => garden.Plant(NULL_NAME);

            //Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(result); //Throws oczekuje konkretnego wyjątku
            //var argumentNullException = Assert.ThrowsAny<ArgumentException>(result); //ThrowsAny uwzględnia dziedziczenie
            Assert.Equal("plantName", argumentNullException.ParamName, ignoreCase: true);
        }
        
        [Fact(Skip = "Replaced by Plant_InvalidName_ArgumentExceptionWithParameterName")]
        public void Plant_WhitespaceName_ArgumentException()
        {
            //Arrange
            const int MIN_VALID_SIZE = default;
            const string? WHITESPACE_NAME = " ";
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            var exception = Record.Exception(() => garden.Plant(WHITESPACE_NAME));

            //Assert
            Assert.NotNull(exception);
            var argumentException = Assert.IsType<ArgumentException>(exception);
            Assert.Equal("plantName", argumentException.ParamName);
            Assert.Contains("Roślina musi posiadać nazwę", argumentException.Message);
        }


        [Fact]
        public void GetPlants_CopyOfPlantsCollection()
        {
            //Arrage
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.NotSame(result1, result2);
        }
    }
}
