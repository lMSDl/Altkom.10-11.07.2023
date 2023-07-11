using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test.xUnit
{
    public class ProgramTest
    {
        [Fact]
        public void Main_ConsoleOutput_HelloWorld()
        {
            //Arrange
            var originalOutput = Console.Out;
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            const string EXPECTED = "Hello, World!\r\n";

            var main = typeof(Program).Assembly.EntryPoint!;

            //Act
            //Program.Main();
            main.Invoke(null, new object[] { Array.Empty<string>() });


            //Assert
            Console.SetOut(originalOutput);
            Assert.Equal(EXPECTED, stringWriter.ToString());
        }

    }
}
