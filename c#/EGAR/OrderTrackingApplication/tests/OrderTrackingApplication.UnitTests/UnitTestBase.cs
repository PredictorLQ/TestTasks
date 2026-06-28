using OrderTrackingApplication.Infrastructure.Mappings;

namespace OrderTrackingApplication.UnitTests;

public abstract class UnitTestBase
{
    protected UnitTestBase()
    {
        OrderMappingRegister.Configure();
    }
}
