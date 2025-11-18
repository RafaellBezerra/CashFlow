using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        private readonly Mock<IPasswordEncripter> _mock;
        public PasswordEncripterBuilder()
        {
            _mock = new Mock<IPasswordEncripter>();

            _mock.Setup(config => config.Encrypt(It.IsAny<string>())).Returns("!%bdedw321");
        }

        public PasswordEncripterBuilder Verify(string? password)
        {
            if (string.IsNullOrWhiteSpace(password) == false)
                _mock.Setup(config => config.Verify(password, It.IsAny<string>())).Returns(true);

            return this;
        }

        public IPasswordEncripter Build() => _mock.Object;
    }
}
