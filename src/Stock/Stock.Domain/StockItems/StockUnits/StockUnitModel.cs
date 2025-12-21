using DomainObjects;
using MediatR;

namespace Stock.Domain.StockItems.StockUnits
{
    public class StockUnitModel
    {
        private StockUnitModel(
            Guid id,
            SerialNumberValueObject serialNumber,
            bool isLocked,
            Guid? checkoutReference,
            DateTime? lockedUntil,
            Guid? warehouseId,
            bool isSold,
            Guid? orderReference)
        {
            Id = id;
            SerialNumber = serialNumber;
            IsLocked = isLocked;
            CheckoutReference = checkoutReference;
            LockedUntil = lockedUntil;
            WarehouseId = warehouseId;
            IsSold = isSold;
            OrderReference = orderReference;
        }

        public Guid Id { get; }

        public Guid? WarehouseId { get; private set; }

        public SerialNumberValueObject SerialNumber { get; }

        public bool IsLocked { get; private set; }

        public Guid? CheckoutReference { get; private set; }

        public DateTime? LockedUntil { get; private set; }

        public bool IsSold { get; private set; }

        public Guid? OrderReference { get; private set; }

        public bool Reserve(DateTime until, Guid checkoutReference)
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
            CheckoutReference = checkoutReference;
            return true;
        }

        public void Unlock()
        {
            IsLocked = false;
            LockedUntil = null;
            CheckoutReference = null;
        }

        public Result<Unit> AssignToOrder(Guid orderId, Guid checkoutReference)
        {
            if (!IsLocked || CheckoutReference != checkoutReference || IsSold || OrderReference is not null)
            {
                return Result<Unit>.Failure("ASSIGN_TO_ORDER_ERROR_UNIT");
            }

            IsSold = true;
            OrderReference = orderId;
            WarehouseId = null;
            IsLocked = false;
            CheckoutReference = null;
            LockedUntil = null;

            return Result<Unit>.Success(Unit.Value);
        }

        public bool IsAvailable() => !IsLocked && !IsSold;

        public static Result<StockUnitModel> CreateNew(Guid id, string serialNumber, Guid warehouseId)
        {

            var serialNumberValueObject = SerialNumberValueObject.Create(serialNumber);

            if (serialNumberValueObject.IsSuccess)
            {
                return Result<StockUnitModel>.Success(new StockUnitModel(
                    id,
                    serialNumberValueObject.Value!,
                    isLocked: false,
                    checkoutReference: null,
                    lockedUntil: null,
                    warehouseId,
                    isSold: false,
                    orderReference: null
                ));

            }
            else
            {
                return Result<StockUnitModel>.Failure(serialNumberValueObject.Error!);
            }
        }

        public static StockUnitModel Rehydrate(
            Guid id,
            Guid? warehouseId,
            SerialNumberValueObject serialNumber,
            bool isLocked,
            Guid? checkoutReference,
            DateTime? lockedUntil,
            bool isSold,
            Guid? orderReference)
        {
            return new StockUnitModel(
                id,
                serialNumber,
                isLocked,
                checkoutReference,
                lockedUntil,
                warehouseId,
                isSold,
                orderReference
            );
        }
    }
}
