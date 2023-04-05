using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KeysReporting.WebAssembly.App.Shared.Quote
{
    public class CalculatorDto
    {
        public string? ClientName { get; set; }
        public int? Year { get; set; }

        public CostBreakDown BreakDown { get; set; } = new();

        public List<CampaignLine> CampaignLines { get; set; } = new();

    }

    public class CostBreakDown
    {
        public decimal? SetUpFees { get; set; }
        public decimal? PhoneAppend { get; set; }
        public decimal? CostOfAdmin { get; set; }
    }

    public class CampaignLine
    {
        private decimal? _grossNames;
        private decimal? _oTGResponse;
        private decimal? _OTGAvg;
        private decimal? _PACResponse;
        private decimal? _PACAVGGift;
        private decimal? _MailResponse;
        private decimal? _MailAverage;

        private void Recalculate()
        {
            //Handle Blanks
            _grossNames = _grossNames ?? 0;
            _oTGResponse = _oTGResponse ?? 0;
            _OTGAvg = _OTGAvg ?? 0;
            _PACResponse = _PACResponse ?? 0;
            _PACAVGGift = _PACAVGGift ?? 0;
            _MailResponse = _MailResponse ?? 0;
            _MailAverage = _MailAverage ?? 0;

            //Calls
            CompleteCalls = _grossNames.Value * (decimal)0.5;
            CallingHours = CompleteCalls.Value / (decimal)5;
            CampaignCost = CallingHours.Value * (decimal)48;

            //OTG
            OTGDonors = CompleteCalls * (_oTGResponse / (decimal)100);
            OTGRevenue = OTGDonors * _OTGAvg;

            //PAC
            PACGifts = CompleteCalls * (_PACResponse / (decimal)100);
            PACMonthly = _PACAVGGift * PACGifts;
            PAC1Year = PACMonthly * (decimal)12;

            //Mail Average
            MailDonors = CompleteCalls * (_MailResponse / (decimal)100);
            MailFulfilled = MailDonors * (decimal)0.5;
            MailRevenue = MailFulfilled * _MailAverage;
        }

        public int? ID { get; set; }
        public string? Campaign { get; set; }
        public string? TargetAudience { get; set; }

        public decimal? GrossNames {
            get { return _grossNames; }
            set 
            {
                _grossNames = value;
                Recalculate();          
            }
        }

        public decimal? CompleteCalls { get; set; }
        public decimal? CallingHours { get; set; }
        public decimal? CampaignCost { get; set; }

        public decimal? OTGResponse { 
            get { return _oTGResponse; } 
            set
            {
                _oTGResponse = value;
                Recalculate();
            }
        }

        public decimal? OTGDonors { get; set; }
        public decimal? OTGRevenue { get; set; }
        public decimal? OTGAvg
        {
            get { return _OTGAvg; }
            set
            {
                _OTGAvg = value;
                Recalculate();
            }
        }
        public decimal? PACResponse 
        { 
            get { return _PACResponse; }
            set
            {
                _PACResponse = value;
                Recalculate();
            }
        }
        public decimal? PACGifts { get; set; }
        public decimal? PACAVGGift 
        { 
            get { return _PACAVGGift; }
            set
            {
                _PACAVGGift = value;
                Recalculate();
            }
        }
        public decimal? PACMonthly { get; set; }
        public decimal? PAC1Year { get; set; }
        public decimal? MailResponse 
        { 
            get { return _MailResponse; }
            set
            {
                _MailResponse = value;
                Recalculate();
            }
        }
        public decimal? MailDonors { get; set; }
        public decimal? MailFulfilled { get; set; } 
        public decimal? MailRevenue { get; set; }
        public decimal? MailAverage 
        { 
            get { return _MailAverage; }
            set
            {
                _MailAverage = value;
                Recalculate();
            }
        }
    }
}
