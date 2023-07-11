using AutoFixture;
using FluentAssertions;
using Moq;
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
            Garden = new Garden(0, null);
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
            var loggerStab = new Mock<ILogger>();

            //Act
            var garden = new Garden(initSize, loggerStab.Object);
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
            string VALID_NAME = new Fixture().Create<string>();
            var garden = PrepareGarden(MIN_VALID_SIZE);

            //Act
            var result = garden.Plant(VALID_NAME);

            //Assert
            Assert.True(result);
        }

        //przy powtarzającym się kodzie Arrange zamiast metody SetUp zaleca się utworzenie zwykłej metody i ręczne wywołanie jej
        private static Garden PrepareGarden(int gardenSize)
        {
            var loggerStab = new Mock<ILogger>();
            return new Garden(gardenSize, loggerStab.Object);
        }

        [Fact]
        public void Plant_GardenOverflow_False()
        {
            //Arrange
            const int MAX_VALID_SIZE = 1;
            string VALID_NAME = new Fixture().Create<string>();
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
            string VALID_NAME = new Fixture().Create<string>();
            string EXPECTED_NAME = VALID_NAME + "2";
            var loggerStab = new Mock<ILogger>();
            var garden = new Garden(MAX_VALID_SIZE, loggerStab.Object);
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
            var loggerStab = new Mock<ILogger>();
            var garden = new Garden(MIN_VALID_SIZE, loggerStab.Object);

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
            var loggerStab = new Mock<ILogger>();
            var garden = new Garden(MIN_VALID_SIZE, loggerStab.Object);

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
            var loggerStab = new Mock<ILogger>();
            var garden = new Garden(MIN_VALID_SIZE, loggerStab.Object);

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
            var loggerStab = new Mock<ILogger>();
            var garden = new Garden(INSIGNIFICANT_SIZE, loggerStab.Object);

            //Act
            var result1 = garden.GetPlants();
            var result2 = garden.GetPlants();

            //Assert
            Assert.NotSame(result1, result2);
        }

        [Fact]
        public void Plant_ValidName_MessageLogged()
        {
            //Arrange
            const int VALID_SIZE = 1;
            string VALID_NAME = new Fixture().Create<string>();
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Log(It.IsAny<string>())).Verifiable();

            var garden = new Garden(VALID_SIZE, loggerMock.Object);

            //Act
            garden.Plant(VALID_NAME);

            //Assert
            loggerMock.VerifyAll();
        }

        [Fact]
        public void Plant_DuplicatedName_MessageLogged()
        {
            //Arrange
            const int VALID_SIZE = 2;
            string VALID_NAME = new Fixture().Create<string>();
            var loggerMock = new Mock<ILogger>();
            var garden = new Garden(VALID_SIZE, loggerMock.Object);
            garden.Plant(VALID_NAME);

            //Act
            garden.Plant(VALID_NAME);

            //Assert
            loggerMock.Verify(x => x.Log(It.Is<string>(x => x.Contains(VALID_NAME))), Times.Exactly(3));
            loggerMock.Verify(x => x.Log(It.Is<string>(x => x.StartsWith($"Roślina {VALID_NAME} zmienia nazwę na "))));
        }

        [Fact]
        public void GetLastLogFromLastHour_LastLog()
        {
            //Arrange
            var fixture = new Fixture();
            string message1 = fixture.Create<string>();
            string message2 = fixture.Create<string>();
            var logger = new Mock<ILogger>();
            logger.Setup(x => x.GetLogs(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns($"{message1}\n{message2}");
            
            const int INSIGNIFICANT_SIZE = 0;
            var garden = new Garden(INSIGNIFICANT_SIZE, logger.Object);

            //Act
            var result = garden.GetLastLogFromLastHour();

            //Asert
            result.Should().Be(message2);
        }
    }
}
