using FluentAssertions;
using PingChecker.Converters;
using PingChecker.Enums;
using System.Globalization;
using Xunit;

namespace PingChecker.Tests
{
    public class AlarmModeConverterTests
    {
        [Theory]
        [InlineData(AlarmMode.Higher, AlarmMode.Lower, false)]
        [InlineData(AlarmMode.Higher, AlarmMode.Higher, true)]
        public void ConvertTests(AlarmMode mode1, AlarmMode mode2, bool expectedResult)
        {
            var sut = new AlarmModeConverter();

            object result = sut.Convert(mode1, typeof(bool), mode2, CultureInfo.CurrentCulture);

            result.Should().Be(expectedResult);
        }
    }
}