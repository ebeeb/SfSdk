﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SfSdk.Contracts;
using SfSdk.Data;
using Xunit;

namespace SfSdk.Tests
{
    public class SessionTests
    {
        [Fact]
        public void LoginAsyncThrowsExceptionIfUsernameIsNull()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LoginAsync(null, null, null);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(e => e.ParamName == "username" && e.Message.StartsWith("Username must not be null or empty."));
        }

        [Fact]
        public void LoginAsyncThrowsExceptionIfUsernameIsEmpty()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LoginAsync(string.Empty, null, null);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(e => e.ParamName == "username" && e.Message.StartsWith("Username must not be null or empty."));
        }

        [Fact]
        public void LoginAsyncThrowsExceptionIfPasswordHashIsNull()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LoginAsync(TestConstants.ValidUsername, null, null);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(
                 e =>
                 e.ParamName == "md5PasswordHash" &&
                 e.Message.StartsWith("Password hash must not be null and have a length of 32."));
        }

        [Fact]
        public void LoginAsyncThrowsExceptionIfPasswordHashHasNotALengthOf32()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LoginAsync(TestConstants.ValidUsername, "a", null);

            // Act / Assert
            a.ShouldThrow<ArgumentException>()
             .Where(
                 e =>
                 e.ParamName == "md5PasswordHash" &&
                 e.Message.StartsWith("Password hash must not be null and have a length of 32."));
        }

        [Fact]
        public void LoginAsyncThrowsExceptionIfServerUriIsNull()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a =
                async () => await sut.LoginAsync(TestConstants.ValidUsername, TestConstants.ValidPasswordHash, null);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>()
             .Where(e => e.ParamName == "serverUri");
        }

        [Fact]
        public async Task LoginAsyncReturnsTrueIfLoginSucceeds()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());

            // Act
            var loginSucceded =
                await
                sut.LoginAsync(TestConstants.ValidUsername, TestConstants.ValidPasswordHash,
                               TestConstants.ValidServerUri);

            // Assert
            loginSucceded.Should().BeTrue();
        }

        [Fact]
        public async Task LoginAsyncReturnsFalseIfLoginFails()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());

            // Act
            var loginSucceded =
                await
                sut.LoginAsync(TestConstants.ValidUsername, TestConstants.InvalidPasswordHash,
                               TestConstants.ValidServerUri);

            // Assert
            loginSucceded.Should().BeFalse();
        }

        [Fact]
        public void LogoutAsyncThrowsExceptionIfSessionIsNotLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.LogoutAsync();

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "LogoutAsync requires to be logged in.");
        }

        [Fact]
        public async Task LogoutAsyncReturnsTrueIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var logoutSucceeded = await sut.LogoutAsync();

            // Assert
            // TODO how to test the false case?
            logoutSucceeded.Should().BeTrue();
        }

        [Fact]
        public void MyCharacterAsyncThrowsExceptionIfSessionIsNotLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.MyCharacterAsync();

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "MyCharacterAsync requires to be logged in.");
        }

        [Fact]
        public async Task MyCharacterAsyncReturnsICharacterIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var myCharacter = await sut.MyCharacterAsync();

            // Assert
            // TODO how to test null value for Character
            myCharacter.Should().NotBeNull();
        }

        [Fact]
        public void RequestCharacterAsyncThrowsExceptionIfUsernameIsNull()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.RequestCharacterAsync(null);

            // Act / Assert
            a.ShouldThrow<ArgumentNullException>()
             .Where(e => e.ParamName == "username");
        }

        [Fact]
        public void RequestCharacterAsyncThrowsExceptionIfSessionIsNotLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.RequestCharacterAsync("Foo");

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "RequestCharacterAsync requires to be logged in.");
        }

        [Fact]
        public async Task RequestCharacterAsyncReturnsICharacterIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var myCharacter = await sut.RequestCharacterAsync("Foo");

            // Assert
            // TODO how to test null value for Character
            myCharacter.Should().NotBeNull();
        }

        [Fact]
        public void HallOfFameAsyncThrowsExceptionIfSessionIsNotLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.HallOfFameAsync();

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "HallOfFameAsync requires to be logged in.");
        }

        [Fact]
        public async Task HallOfFameAsyncReturnsShouldReturn15ICharactersIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var characters = await sut.HallOfFameAsync();

            // Assert
            // TODO how to test null value for Character
            characters.Should().HaveCount(c => c == 15);
        }

        [Fact]
        public void ScrapbookAsyncThrowsExceptionIfSessionIsLoggedIn()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            Func<Task> a = async () => await sut.ScrapbookAsync();

            // Act / Assert
            a.ShouldThrow<SessionLoggedOutException>()
             .Where(e => e.Message == "ScrapbookAsync requires to be logged in.");
        }

        [Fact]
        public async Task ScrapbookAsyncReturns252MonsterItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = (await sut.ScrapbookAsync()).ToList();

            // Assert
            results.OfType<IMonsterItem>().Count().Should().Be(252);
        }

        [Fact]
        public async Task ScrapbookAsyncHas36Of252MonsterItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = (await sut.ScrapbookAsync()).ToList();

            // Assert
            results.OfType<IMonsterItem>().Count(i => i.HasItem).Should().Be(36);
        }

        [Fact]
        public async Task ScrapbookAsyncReturns246ValuableItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.OfType<IValuableItem>().Count().Should().Be(246);
        }

        [Fact]
        public async Task ScrapbookAsyncHas18Of246ValuableItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.OfType<IValuableItem>().Count(i => i.HasItem).Should().Be(18);
        }

        [Fact]
        public async Task ScrapbookAsyncReturns506WarriorItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.OfType<IWarriorItem>().Count().Should().Be(506);
        }

        [Fact]

        public async Task ScrapbookAsyncHas19Of506WarriorItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.OfType<IWarriorItem>().Count(i => i.HasItem).Should().Be(19);
        }

        [Fact]
        public async Task ScrapbookAsyncReturns348MageItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.OfType<IMageItem>().Count().Should().Be(348);
        }

        [Fact]
        public async Task ScrapbookAsyncHas20Of348MageItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.OfType<IMageItem>().Count(i => i.HasItem).Should().Be(20);
        }

        [Fact]
        public async Task ScrapbookAsyncReturns348ScoutItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.OfType<IScoutItem>().Count().Should().Be(348);
        }

        [Fact]
        public async Task ScrapbookAsyncHas10Of348ScoutItems()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.OfType<IScoutItem>().Count(i => i.HasItem).Should().Be(10);
        }

        [Fact]
        public async Task ScrapbookAsyncReturns1700Items()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.Count().Should().Be(1700);
        }

        [Fact]
        public async Task ScrapbookAsync103Of1700Items()
        {
            // Arrange
            var sut = new Session(serverUri => new TestRequestSource());
            await sut.LoginAsync(TestConstants.ValidUsername,
                                 TestConstants.ValidPasswordHash,
                                 TestConstants.ValidServerUri);

            // Act
            var results = await sut.ScrapbookAsync();

            // Assert
            results.Count(i => i.HasItem).Should().Be(103);
        }

        // TODO: HallOfFameAsyncForce, private code paths?
    }
}