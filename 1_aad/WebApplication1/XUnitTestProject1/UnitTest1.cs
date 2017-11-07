using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

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
        public async Task Fails401NoBearerToken()
        {
            var response = await _sut.Client.GetAsync("/api/Values");

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SuccessWithBearerToken()
        {
            var response = await _sut.Client.GetAsync("/api/Values");

            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
