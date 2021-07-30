using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using Uploader.Application.Common.Behaviors;

namespace Uploader.Application.UnitTests.Common.Behaviors
{
    public class UnhandledExceptionBehaviorTest
    {
        private UnhandledExceptionBehavior<TestCommand, TestResult> _behavior;
        private Mock<ILogger<TestCommand>> _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<TestCommand>>();
            
            _behavior = new UnhandledExceptionBehavior<TestCommand, TestResult>(_logger.Object);
        }
        
        [Test]
        public void Handle_ShouldLogErrorAndThrow_WhenNextThrowsException()
        {
            var next = new Mock<RequestHandlerDelegate<TestResult>>();
            next.Setup(x => x.Invoke()).ThrowsAsync(new Exception());
            Assert.ThrowsAsync(typeof(Exception), async () =>
            {
                await _behavior.Handle(new EmptyTestCommand(null), CancellationToken.None, next.Object);
            });
            next.Verify(x => x.Invoke(), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_ShouldCallNextAndReturn_WhenNextDoesNotThrowException()
        {
            var next = new Mock<RequestHandlerDelegate<TestResult>>();
            await _behavior.Handle(new EmptyTestCommand(null), CancellationToken.None, next.Object);
            next.Verify(x => x.Invoke(), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }
    }
}