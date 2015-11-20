using System;
using System.Collections.Generic;
using IBApi.Messages.Client;
using IBApi.Serialization;

namespace IBApi.Messages.Server
{
    [IbSerializable(5)]
    struct OpenOrderMessage : IServerMessage
    {
        public int Version;
        public int OrderId;

        public int ContractId;
        public string Symbol;
        public string SecurityType;
        public DateTime? Expiry;
        public double Strike;
        public string Right;
        public string Multiplier;
        public string Exchange;
        public string Currency;
        public string LocalSymbol;
        public string TradingClass;

        public string OrderAction;
        public int TotalQuantity;
        public string OrderType;
        public double? LimitPrice;
        public double? AuxPrice;
        public string Tif;
        public string OCAGroup;
        public string Account;
        public string OpenClose;
        public int OrderOrigin;
        public string OrderRef;
        public int ClientId;
        public int PermId;

        public bool OutsideRTH;
        public int Hidden;

        public double DiscretionaryAmt;
        public string GoodAfterTime;

        /// <summary>
        /// Deprecated ver 6 field
        /// </summary>
        public string SharesAllocation;

        public string FAGroup;
        public string FAMethod;
        public string FAPercentage;
        public string FAProfile;

        public string GoodTillDate;
        public string Rule80A;
        public double? PercentOffset;
        public string SettlingFirm;
        public int ShortSaleSlot;
        public string DesignatedLocation;
        public int ExemptCode;
        public int AuctionStrategy;
        public double? StartingPrice;
        public double? StockRefPrice;
        public double? Delta;
        public double? StockRangeLower;
        public double? StockRangeUpper;
        public int? DisplaySize;
        public bool? BlockOrder;
        public bool? SweepToFill;
        public bool? AllOrOne;
        public int? MinQty;
        public int? OCAType;
        public bool? ETradeOnly;
        public bool? FirmQuoteOnly;
        public double? NBBOPriceCap;
        public int? ParentId;
        public int? TriggerMethod;
        public double? Volatility;
        public int? VolatilityType;
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
        public double? BasisPoints;
        public int? BasisPointsType;
        public string ComboLegsDescription;

        public IEnumerable<ComboLeg> ComboLegs;
        public IEnumerable<OrderComboLeg> OrderComboLegs;
        public IEnumerable<TagValue> SmartComboRoutingParams;

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

        public bool ShouldSerializeUnderComp()
        {
            return UnderCompPresentFlag == true;
        }

        public string AlgoStrategy;

        public IEnumerable<TagValue> AlgoParams;

        public bool ShouldSerializeAlgoParams()
        {
            return !String.IsNullOrEmpty(AlgoStrategy);
        }

        public bool? WhatIf;
        public string Status;
        public string InitMargin;
        public string MainMargin;
        public string EquityWithLoan;

        public double? Commission;
        public double? MinCommission;
        public double? MaxCommission;
        public string CommissionCurrency;
        public string WarningText;

    }
}
