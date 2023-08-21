using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Infrastructure.UnitTests;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(Customize)
    {
    }

    private static IFixture Customize()
    {
        var customize = new Fixture().Customize(new AutoMoqCustomization());
        customize.Customize<DateOnly?>(c =>
            c.FromFactory(() => DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1 * customize.Create<int>()))));
        return customize;
    }
}
