using Core.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tests.Common;
using System.Linq;
using CoreInputValidator = Core.Util.InputValidator;

namespace Tests.ManagerTest
{
    [TestClass]
    public class InputValidator
    {
        [DataRow("InvalidPhones.json", false)]
        [DataRow("ValidPhones.json", true)]
        [DataTestMethod]
        public void PhoneCheck(string fileName, bool expectedOutcome)
        {
            // Arrange
            List<Phone> phones = DataGenerator.Deserialize<List<Phone>>(fileName);
            List<bool> validationCheck = new List<bool>(phones.Count);

            // Act
            phones.ForEach(
                phone => validationCheck.Add(CoreInputValidator.IsValidPhoneNumber(phone.PhoneNumber))
            );

            // Assert
            Assert.IsFalse(validationCheck.Any(phoneValidation => phoneValidation != expectedOutcome));
        }
    }
}
