﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SfSdk.Constants;
using SfSdk.Request;
using SfSdk.Response;
using Xunit;

namespace SfSdk.Tests.Request
{
    public class SfRequestTests
    {
        [Fact]
        public void ExecuteAsyncThrowsExceptionIfSourceIsNull()
        {
            // Arrange
            var sut = new SfRequest();
            Func<Task> a = async () => await sut.ExecuteAsync(null, null, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "source");
        }

        [Fact]
        public void ExecuteAsyncThrowsExceptionIfSessionIdIsNull()
        {
            // Arrange
            var sourceMock = new Mock<IRequestSource>();
            var sut = new SfRequest();
            Func<Task> a = async () => await sut.ExecuteAsync(sourceMock.Object, null, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>().Where(e => e.ParamName == "sessionId");
        }

        [Fact]
        public void ExecuteAsyncThrowsExceptionIfSessionIdHasInvalidLength()
        {
            // Arrange
            var sourceMock = new Mock<IRequestSource>();
            var sut = new SfRequest();
            Func<Task> a =
                async () =>
                await sut.ExecuteAsync(sourceMock.Object, TestConstants.InvalidSessionId, SF.ActAccountCreate);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(e => e.ParamName == "sessionId" && e.Message.StartsWith("sessionId must have a length of 32."));
        }

        [Fact]
        public async Task ExecuteAsyncReturnsISfResponseWithValidParameters()
        {
            // Arrange
            var sfResponseMock = new Mock<ISfResponse>();
            var sourceMock = new Mock<IRequestSource>();
            sourceMock.Setup(source => source.RequestAsync(It.IsAny<string>(),
                                                           It.IsAny<SF>(),
                                                           It.IsAny<IEnumerable<string>>()))
#pragma warning disable 1998
                      .Returns(async () => sfResponseMock.Object);
#pragma warning restore 1998
            var sut = new SfRequest();

            // Act
            var result = await sut.ExecuteAsync(sourceMock.Object, TestConstants.ValidSessionId, SF.ActAccountCreate);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsyncCallsRequestAsyncMethodOfSource()
        {
            // Arrange
            var sourceMock = new Mock<IRequestSource>();
            sourceMock.Setup(source => source.RequestAsync(It.IsAny<string>(),
                                                           It.IsAny<SF>(),
                                                           It.IsAny<IEnumerable<string>>()))
#pragma warning disable 1998
                      .Returns(async () => null)
#pragma warning restore 1998
                      .Verifiable();

            var sut = new SfRequest();
            Func<Task> a =
                async () => await sut.ExecuteAsync(sourceMock.Object, TestConstants.ValidSessionId, SF.ActAccountCreate);

            // Act
            await a();

            // Assert
            sourceMock.Verify(
                source => source.RequestAsync(It.IsAny<string>(), It.IsAny<SF>(), It.IsAny<IEnumerable<string>>()),
                Times.Once());
        }
    }
}