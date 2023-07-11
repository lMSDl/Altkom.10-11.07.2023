﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentAssertions;
using FluentAssertions.Execution;

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
            //Assert.Contains(MESSAGE, logger.GetLogs(from, to));
            logger.GetLogs(from, to).Should().Contain(MESSAGE);
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
            //Assert.True(result);
            result.Should().BeTrue();
        }

        [Fact]
        public void Log_AnyMessage_ValidEventInvoked()
        {
            //Arrage
            var logger = new Logger();
            const string MESSAGE = "a";
            using var monitor = logger.Monitor();
            DateTime from = DateTime.Now;

            //Act
            logger.Log(MESSAGE);

            //Assert
            DateTime to = DateTime.Now;

            using (new AssertionScope())
                monitor.Should().Raise(nameof(Logger.MessageLogged))
                .WithSender(logger)
                .WithArgs<Logger.LoggerEventArgs>(a => a.Message == MESSAGE); 

        }
        [Fact]
        public void Log_AnyMessage_ValidEventInvoked2()
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
            using (new AssertionScope())
            {
                loggerSender.Should().Be(logger);
                loggerEventArgs.Message.Should().Be(MESSAGE);
                loggerEventArgs.DateTime.Should().BeAfter(from).And.BeBefore(to);

            }
        }

        //[Fact]
        //public void Log_AnyMessage_ValidEventInvoked()
        //{
        //    //Arrage
        //    var logger = new Logger();
        //    const string MESSAGE = "a";
        //    object? loggerSender = null;
        //    Logger.LoggerEventArgs? loggerEventArgs = null;
        //    logger.MessageLogged += (sender, args) => { loggerSender = sender; loggerEventArgs = args as Logger.LoggerEventArgs; };
        //    DateTime from = DateTime.Now;

        //    //Act
        //    logger.Log(MESSAGE);

        //    //Assert
        //    DateTime to = DateTime.Now;
        //    Assert.Equal(logger, loggerSender);
        //    Assert.NotNull(loggerEventArgs);
        //    Assert.Equal(MESSAGE, loggerEventArgs.Message);
        //    Assert.InRange(loggerEventArgs.DateTime, from, to);
        //}

        [Fact]
        public void GetLogAsync_DateRange_LoggedMessageAsync()
        {
            //Arrage
            var logger = new Logger();
            const string MESSAGE = "a";
            DateTime from = DateTime.Now;
            logger.Log(MESSAGE);
            DateTime to = DateTime.Now;

            //Act
            var task = logger.GetLogsAsync(from, to);
            task.Wait();
            var result = task.Result;

            //Assert
            Assert.True(task.IsCompletedSuccessfully);
            Assert.Contains(MESSAGE, result);
            Assert.True(DateTime.TryParseExact(result.Split(": ")[0], "dd.MM.yyyy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }

        [Fact]
        public void GetLogAsync_DateRange_ValidLogDate()
        {
            //Arrage
            var logger = new Logger();
            const string MESSAGE = "a";
            logger.Log(MESSAGE);
            DateTime from = DateTime.Now;
            logger.Log(MESSAGE);
            DateTime to = DateTime.Now;
            logger.Log(MESSAGE);

            //Act
            var task = logger.GetLogsAsync(from, to);
            task.Wait();
            var result = task.Result;

            //Assert
            Assert.True(task.IsCompletedSuccessfully);
            Assert.DoesNotContain("\n", result);
        }
    }
}
