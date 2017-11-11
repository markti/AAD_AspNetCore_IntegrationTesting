using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private readonly TestContext _sut;

        public UnitTest1()
        {
            _sut = new TestContext();
        }

        [Fact]
        public async Task SuccessWithBearerToken()
        {
            var response = await _sut.Client.GetAsync("/api/Values");

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}
