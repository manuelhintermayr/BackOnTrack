using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOnTrack.SharedResources.Infrastructure.Helpers;
using BackOnTrack.SharedResources.Tests.Base;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOnTrack.SharedResources.Tests
{
    [TestClass]
    public class EncryptingTests : TestBase
    {
        private readonly string _passphrase = "admin";
        private readonly string _decryptedText = "Hello World";

        private readonly string _encryptedText =
            "4/OV20hCRXzpKDchcistKntgHVqPTwbCdbtM88dDma91emA4b1kCH72Z+z/diWb/U0w6BwfqrFblSijdpoq4s4FzDynZ8GLYXNyX3OWxLmZ1zlNuuzkDGv72lt4eJvFm";

        [TestMethod]
        [TestProperty("Number", "20")]
        [TestProperty("Type", "Unit")]
        public void DecryptTextCorrectly()
        {
            //Arrange & Act
            string result = EncryptingHelper.Decrypt(_encryptedText, _passphrase);

            //Assert
            result.Should().Be(_decryptedText);
        }

        [TestMethod]
        [TestProperty("Number", "15")]
        [TestProperty("Type", "Unit")]
        public void EverytimeADifferentOutputForTheSamePassPhrase()
        {
            //Arrange & Act
            string encryptedTextForFirstEncryption = EncryptingHelper.Encrypt(_decryptedText, _passphrase);
            string decryptedTextForFirstEncryption =
                EncryptingHelper.Decrypt(encryptedTextForFirstEncryption, _passphrase);

            string encryptedTextForSecondEncryption = EncryptingHelper.Encrypt(_decryptedText, _passphrase);
            string decryptedTextForSecondEncryption =
                EncryptingHelper.Decrypt(encryptedTextForSecondEncryption, _passphrase);

            string encryptedTextForThirdEncryption = EncryptingHelper.Encrypt(_decryptedText, _passphrase);
            string decryptedTextForThirdEncryption =
                EncryptingHelper.Decrypt(encryptedTextForThirdEncryption, _passphrase);
            
            //Assert
            encryptedTextForFirstEncryption.Should().NotBe(encryptedTextForSecondEncryption);
            encryptedTextForFirstEncryption.Should().NotBe(encryptedTextForThirdEncryption);
            encryptedTextForSecondEncryption.Should().NotBe(encryptedTextForThirdEncryption);

            decryptedTextForFirstEncryption.Should().Be(_decryptedText);
            decryptedTextForSecondEncryption.Should().Be(_decryptedText);
            decryptedTextForThirdEncryption.Should().Be(_decryptedText);
        }

        [TestMethod]
        [TestProperty("Number", "7")]
        [TestProperty("Type", "Unit")]
        public void ExceptionIsThrownOnInvalidPassphrase()
        {
            //Arrange & Act
            Action decryptionProcess = () => { EncryptingHelper.Decrypt(_encryptedText, "admin1"); };

            //Assert
            decryptionProcess.Should().Throw<System.Security.Cryptography.CryptographicException>();
        }
    }
}
