using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp.Test.xUnit
{
    public class LoggerTest
    {
        [Fact]
        public void Log_AnyMessage_MessageLogged()
        {
            //Arrage
            var logger = new Logger();
            const string MESSAGE = "a";
            DateTime from = DateTime.Now;

            //Act
            logger.Log(MESSAGE);

            //Assert
            DateTime to = DateTime.Now;
            Assert.Contains(MESSAGE, logger.GetLogs(from, to));
        }

        [Fact]
        public void Log_AnyMessage_EventInvoked()
        {
            //Arrage
            var logger = new Logger();
            const string MESSAGE = "a";
            var result = false;
            logger.MessageLogged += (sender, args) => result = true;

            //Act
            logger.Log(MESSAGE);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Log_AnyMessage_ValidEventInvoked()
        {
            //Arrage
            var logger = new Logger();
            const string MESSAGE = "a";
            object? loggerSender = null;
            Logger.LoggerEventArgs? loggerEventArgs = null;
            logger.MessageLogged += (sender, args) => { loggerSender = sender; loggerEventArgs = args as Logger.LoggerEventArgs; };
            DateTime from = DateTime.Now;

            //Act
            logger.Log(MESSAGE);

            //Assert
            DateTime to = DateTime.Now;
            Assert.Equal(logger, loggerSender);
            Assert.NotNull(loggerEventArgs);
            Assert.Equal(MESSAGE, loggerEventArgs.Message);
            Assert.InRange(loggerEventArgs.DateTime, from, to);
        }
    }
}
