using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.xUnit
{
    public class GardenTest
    {
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
            var garden = new Garden(MIN_VALID_SIZE);

            //Act
            var result = garden.Plant(VALID_NAME);

            //Assert
            Assert.True(result);
        }

        [Fact]
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


        [Fact]
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

        [Fact]
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
    }
}
