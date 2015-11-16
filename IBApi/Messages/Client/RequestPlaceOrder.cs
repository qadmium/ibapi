using System.Collections.Generic;
using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IBSerializable(3)]
    internal struct RequestPlaceOrder : IClientMessage
    {
        public int Version;
        public int OrderId;
        public string Symbol;
        public string SecType;
        public string Expity;
        public double Strike;
        public string Right;
        public string Multiplier;
        public string Exchange;
        public string PrimaryExchange;
        public string Currency;
        public string LocalSymbol;
        public string TradingClass;
        public string SecIdType;
        public string SecId;
        public string Action;
        public int TotalQuantity;
        public string OrderType;
        public double? LimitPrice;
        public double? AuxPrice;
        public string Tif;
        public string OcaGroup;
        public string Account;
        public string OpenClose;
        public Origin Origin;
        public string OrderRef;
        public bool Transmit;
        public int ParentId;
        public bool BlockOrder;
        public bool SweepToFill;
        public int DisplaySize;
        public TriggerMethod TriggerMethod;
        public bool OutsideRth;
        public bool Hidden;
        public IEnumerable<ComboLeg> ComboLegs;

        public bool ShouldSerializeComboLegs()
        {
            return this.SecType == "BAG";
        }

        public IEnumerable<OrderComboLeg> OrderComboLegs;

        public bool ShouldSerializeOrderComboLegs()
        {
            return this.SecType == "BAG";
        }

        public IEnumerable<TagValue> SmartComboRoutingParams;

        public bool ShouldSerializeSmartComboRoutingParams()
        {
            return this.SecType == "BAG";
        }

        public string SharesAllocation;
        public double DiscretionaryAmount;
        public string GoodAfterTime;
        public string GoodTillDate;
        public string FaGroup;
        public string FaMethod;
        public string FaPercentage;
        public string FaProfile;
        public int ShortSaleSlot;
        public string DesignatedLocation;
        public int ExemptCode;
        public OcaType OcaType;
        public string Rule80A;
        public string SettlingFirm;
        public bool AllOrNone;
        public int? MinQuantity;
        public double? PercentOffset;
        public bool EtradeOnly;
        public bool FirmQuoteOnly;
        public double? NbboPriceCap;
        public AuctionStrategy? AuctionStrategy;
        public double? StartingPrice;
        public double? StockRefPrice;
        public double? Delta;
    }

    internal enum AuctionStrategy
    {
        AuctionMatch = 1,
        AuctionImprovment = 2,

    }

    internal enum OcaType
    {
        None = 0,
        CancelWithBlock = 1,
        ReduceWithBlock = 2,
        ReduceNonBlock = 3
    }

    internal struct TagValue
    {
        public string Tag;
        public string Value;
    }

    internal struct ComboLeg
    {
        public int ConId;
        public int Ratio;
        public string Action;
        public string Exchange;
        public int OpenClose;
        public int ShortSaleSlot;
        public string DesignatedLocation;
        public int ExemptCode;
    }

    internal struct OrderComboLeg
    {
        public double? Price;
    }




}
