using MartianDelivery.Domain;

namespace MartianDelivery.Persistence;

public interface IParcelRepository
{
    public bool TryGetParcel(string barcode, out Parcel? parcel);
    public bool TryAddParcel(Parcel parcel);
}
public class ParcelRepository : IParcelRepository
{
    private readonly Dictionary<string, Parcel> _database = new();
    
    public bool TryGetParcel(string barcode, out Parcel? parcel)
    {
        return _database.TryGetValue(barcode, out parcel);
    }

    public bool TryAddParcel(Parcel parcel)
    {
        return _database.TryAdd(parcel.Barcode, parcel);
    }
}