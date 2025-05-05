using NUnit.Framework;

namespace SportsHub.Api.IntegrationTests;

[SetUpFixture]
public class IntegrationTestSetup
{
    [OneTimeSetUp]
    public void GlobalSetup()
    {
        SystemUnderTest.Initialize();
    }

    [OneTimeTearDown]
    public void GlobalTeardown()
    {
        SystemUnderTest.Dispose();
    }
}