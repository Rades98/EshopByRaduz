using DomainObjects;

namespace Stock.Domain.StockItems.StockUnits
{
    public class StockUnitModel
    {
        private StockUnitModel(
            Guid id,
            SerialNumberValueObject serialNumber,
            bool isLocked,
            DateTime? lockedUntil,
            Guid warehouseId)
        {
            Id = id;
            SerialNumber = serialNumber;
            IsLocked = isLocked;
            LockedUntil = lockedUntil;
            WarehouseId = warehouseId;
        }

        public Guid Id { get; }

        public Guid WarehouseId { get; }

        public SerialNumberValueObject SerialNumber { get; }

        public bool IsLocked { get; private set; }

        public DateTime? LockedUntil { get; private set; }

        public bool TryLock(DateTime until)
        {
            if (IsLocked)
            {
                return false;
            }

            if (until <= DateTime.UtcNow)
            {
                return false;
            }

            IsLocked = true;
            LockedUntil = until;
            return true;
        }

        public void Unlock()
        {
            IsLocked = false;
            LockedUntil = null;
        }

        public bool IsAvailable() => !IsLocked;

        public static Result<StockUnitModel> CreateNew(Guid id, string serialNumber, Guid warehouseId)
        {

            var serialNumberValueObject = SerialNumberValueObject.Create(serialNumber);

            if (serialNumberValueObject.IsSuccess)
            {
                return Result<StockUnitModel>.Success(new StockUnitModel(
                    id,
                    serialNumberValueObject.Value!,
                    isLocked: false,
                    lockedUntil: null,
                    warehouseId
                ));

            }
            else
            {
                return Result<StockUnitModel>.Failure(serialNumberValueObject.Error!);
            }
        }

        public static StockUnitModel Rehydrate(
            Guid id,
            Guid warehouseId,
            SerialNumberValueObject serialNumber,
            bool isLocked,
            DateTime? lockedUntil)
        {
            return new StockUnitModel(
                id,
                serialNumber,
                isLocked,
                lockedUntil,
                warehouseId
            );
        }
    }
}
