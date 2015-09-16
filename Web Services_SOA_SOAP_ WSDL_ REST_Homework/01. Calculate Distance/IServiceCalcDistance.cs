using System.Runtime.Serialization;
using System.ServiceModel;


namespace _01.Calculate_Distance
{
    [ServiceContract]
    public interface IServiceCalcDistance
    {
        [OperationContract]
        double CalcDistance(int startPointX, int startPointY, int endPointX, int endPointY);
    }
}
