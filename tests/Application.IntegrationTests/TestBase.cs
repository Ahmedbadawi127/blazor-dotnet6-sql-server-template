using NUnit.Framework;
using System.Threading.Tasks;

namespace test_dotnet_6_prev7.Application.IntegrationTests
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}
