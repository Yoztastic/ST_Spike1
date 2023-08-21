using AutoFixture.Xunit2;

namespace Infrastructure.UnitTests
{
    public class InlineAutoMoqDataAttribute : CompositeDataAttribute
    {
        public InlineAutoMoqDataAttribute( params object[] values)
            : base(new InlineDataAttribute(values), new AutoMoqDataAttribute())
        {

        }
    }
}
