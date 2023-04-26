namespace NetEnhancements.Util.Tests
{
    public class NumericExtensionsTests
    {
        [Test]
        public void ToValueString_Returns_0()
        {
            Assert.Multiple(() =>
            {
                Assert.That(((decimal?)null).ToValueString(), Is.EqualTo("0"));
                Assert.That(((decimal?)0).ToValueString(), Is.EqualTo("0"));
                Assert.That(((decimal)0).ToValueString(), Is.EqualTo("0"));
            });
        }

        [Test]
        [TestCaseSource(nameof(HasDecimals_DataSource))]
        public void HasDecimals_Returns_Boolean((decimal Value, bool HasDecimals) data)
        {
            var (value, hasDecimals) = data;
            
            Assert.That(value.HasDecimals(), Is.EqualTo(hasDecimals));
        }

        private static (decimal Value, bool HasDecimals)[] HasDecimals_DataSource()
        {
            return new []
            {
                (0m, false),
                (1m, false),
                (-51m, false),
                (79_228_162_514_264_337_593_543_950_335m, false),

                (0.1m, true),
                (-0.2m, true),
                (0.0m, true),
                (-0.0m, true),
                (1.0m, true),
                (-12.0m, true),
                (1.1m, true),
                (-2.2m, true),
                (42.42m, true),
                (-444.222m, true),
                (-79_228_162_514_264.337_593_543_950_335m, true),
            };
        }
    }
}
