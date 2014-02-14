using System;
using System.Net;
using NUnit.Framework;
using TR.HttpUtilities;

namespace FluentParserSpecs
{
    [TestFixture]
    public class TRWebClientSpecs
    {
        [Test]
        public void _001_making_sure_that_Google_works()
        {
            var response = TRWebClient.Get(new Uri("http://www.google.com"));
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
