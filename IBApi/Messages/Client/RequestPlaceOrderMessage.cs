using System;
using System.Collections.Generic;
using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IbSerializable(3)]
    internal struct RequestPlaceOrderMessage : IClientMessage
    {
        public int Version;
        public int OrderId;
        public int ContractId;
        public string Symbol;
        public string SecType;
        public DateTime? Expity;
        public double? Strike;
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
        public double? StockRangeLower;
        public double? StockRangeUpper;
        public bool OverridePercentageConstraints;
        public double? Volatility;
        public VolatilityType? VolatilityType;
        public string DeltaNeutralOrderType;
        public double? DeltaNeutralAuxPrice;

        public int DeltaNeutralConId;
        public bool ShouldSerializeDeltaNeutralConId()
        {
            return !string.IsNullOrEmpty(this.DeltaNeutralOrderType);
        }
        public string DeltaNeutralSettlingFirm;
        public bool ShouldSerializeDeltaNeutralSettlingFirm()
        {
            return !string.IsNullOrEmpty(this.DeltaNeutralOrderType);
        }
        public string DeltaNeutralClearingAccount;
        public bool ShouldSerializeDeltaNeutralClearingAccount()
        {
            return !string.IsNullOrEmpty(this.DeltaNeutralOrderType);
        }
        public string DeltaNeutralClearingIntent;
        public bool ShouldSerializeDeltaNeutralClearingIntent()
        {
            return !string.IsNullOrEmpty(this.DeltaNeutralOrderType);
        }

        public string DeltaNeutralOpenClose;

        public bool ShouldSerializeDeltaNeutralOpenClose()
        {
            return !string.IsNullOrEmpty(this.DeltaNeutralOrderType);
        }

        public bool DeltaNeutralShortSale;

        public bool ShouldSerializeDeltaNeutralShortSale()
        {
            return !string.IsNullOrEmpty(this.DeltaNeutralOrderType);
        }

        public int DeltaNeutralShortSaleSlot;

        public bool ShouldSerializeDeltaNeutralShortSaleSlot()
        {
            return !string.IsNullOrEmpty(this.DeltaNeutralOrderType);
        }

        public string DeltaNeutralDesignatedLocation;

        public bool ShouldSerializeDeltaNeutralDesignatedLocation()
        {
            return !string.IsNullOrEmpty(this.DeltaNeutralOrderType);
        }

        public bool? ContinuousUpdate;
        public int? ReferencePriceType;
        public double? TrailStopPrice;
        public double? TrailPercent;
        public int? ScaleInitLevelSize;
        public int? ScaleSubsLevelSize;
        public double? ScalePriceIncrement;

        public double? ScalePriceAdjustValue;

        public bool ShouldSerializeScalePriceAdjustValue()
        {
            return this.ScalePriceIncrement.HasValue && this.ScalePriceIncrement.Value > 0;
        }

        public int? ScalePriceAdjustInterval;

        public bool ShouldSerializeScalePriceAdjustInterval()
        {
            return this.ScalePriceIncrement.HasValue && this.ScalePriceIncrement.Value > 0;
        }

        public double? ScaleProfitOffset;

        public bool ShouldSerializeScaleProfitOffset()
        {
            return this.ScalePriceIncrement.HasValue && this.ScalePriceIncrement.Value > 0;
        }

        public bool? ScaleAutoReset;

        public bool ShouldSerializeScaleAutoReset()
        {
            return this.ScalePriceIncrement.HasValue && this.ScalePriceIncrement.Value > 0;
        }

        public int? ScaleInitPosition;

        public bool ShouldSerializeScaleInitPosition()
        {
            return this.ScalePriceIncrement.HasValue && this.ScalePriceIncrement.Value > 0;
        }

        public int? ScaleInitFillQty;

        public bool ShouldSerializeScaleInitFillQty()
        {
            return this.ScalePriceIncrement.HasValue && this.ScalePriceIncrement.Value > 0;
        }

        public bool? ScaleRandomPercent;

        public bool ShouldSerializeScaleRandomPercent()
        {
            return this.ScalePriceIncrement.HasValue && this.ScalePriceIncrement.Value > 0;
        }

        public string ScaleTable;
        public string ActiveStartTime;
        public string ActiveStopTime;

        public string HedgeType;
        public string HedgeParam;

        public bool ShouldSerializeHedgeParam()
        {
            return !string.IsNullOrEmpty(this.HedgeType);
        }

        public bool? OptOutSmartRouting;

        public string ClearingAccount;
        public string ClearingIntent;

        public bool? NotHeld;

        public bool? UnderCompPresentFlag;

        public UnderComp UnderComp;

        public string AlgoStrategy;

        public IEnumerable<TagValue> AlgoParams;

        public bool ShouldSerializeAlgoParams()
        {
            return !string.IsNullOrEmpty(AlgoStrategy);
        }

        public string AlgoId;
        public bool? WhatIf;
        public IEnumerable<TagValue> OrderMiscOptions;
        
        public static RequestPlaceOrderMessage Default
        {
            get
            {
                return new RequestPlaceOrderMessage
                {
                    Version = 43
                };
            }
        }
    }

    internal enum VolatilityType
    {
        Daily = 1,
        Annual = 2
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
