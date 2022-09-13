using Braintree;

namespace Rocky.Utility;

public interface IBrainTreeGate
{
    IBraintreeGateway CreateGateWay();
    IBraintreeGateway GetGateWay();
}