using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQFeed
{
    public class MyAdmin : IQAdminSocketClient
    {
        public MyAdmin()            
            : base(80)
        { }
        protected override void OnClientStatsEvent(ClientStatsEventArgs e)
        {
            base.OnClientStatsEvent(e);
            NonBlockingConsole.WriteLine("Client Id:" + e.clientId.ToString() +
                " Client Name:" + e.clientName +
                " Start Time:" + e.startTime.ToShortTimeString() +
                " KB in:" + e.kbSent.ToString() +
                " KB out:" + e.kbReceived.ToString() +
                " KB que:" + e.kbQueued.ToString() +
                " Port:" + e.type.ToString() +
                " Symbols Watched:" + e.symbolsWatched.ToString());
        }
        protected override void OnTextLineEvent(TextLineEventArgs e)
        {
            base.OnTextLineEvent(e);
        }
    }

    public class MyLookup : IQLookupHistorySymbolClient
    {
        private bool writeOut = false;
        public MyLookup(bool write)
            : base(4096)
        {
            writeOut = write;
        }
        protected override void OnTextLineEvent(TextLineEventArgs e)
        {
            base.OnTextLineEvent(e);
        }
        protected override void OnLookupEvent(LookupEventArgs e)
        {
            base.OnLookupEvent(e);
            if (e.Type == LookupType.REQ_HST_TCK)
            {
                if (e.Sequence == LookupSequence.MessageStart)
                {
                    NonBlockingConsole.WriteLine("Start(" + e.Id + ")", writeOut);
                }
                if (e.Sequence == LookupSequence.MessageEnd)
                {
                    NonBlockingConsole.WriteLine("End(" + e.Id + ")");
                }
                if (e.Sequence == LookupSequence.MessageDetail)
                {
                    LookupTickEventArgs tea = e as LookupTickEventArgs;
                    NonBlockingConsole.WriteLine("id:" + e.Id + "  Time:" + tea.DateTimeStamp.ToString() + " Lst:" + tea.Last.ToString() +
                         " LstSz:" + tea.LastSize.ToString() + " tv:" + tea.TotalVolume.ToString() + " Bid:" + tea.Bid.ToString() +
                         " Ask:" + tea.Ask.ToString() + " Bss:" + tea.Basis.ToString() + " tid:" + tea.TickId.ToString(), writeOut);
                }
            }
            if (e.Type == LookupType.REQ_HST_INT)
            {
                if (e.Sequence == LookupSequence.MessageStart)
                {
                    NonBlockingConsole.WriteLine("Start(" + e.Id + ")", writeOut);
                }
                if (e.Sequence == LookupSequence.MessageEnd)
                {
                    NonBlockingConsole.WriteLine("End(" + e.Id + ")", writeOut);
                }
                if (e.Sequence == LookupSequence.MessageDetail)
                {
                    LookupIntervalEventArgs tea = e as LookupIntervalEventArgs;
                    NonBlockingConsole.WriteLine("id:" + e.Id + "  Time:" + tea.DateTimeStamp.ToString() + " Op:" + tea.Open.ToString() +
                         " Hi:" + tea.High.ToString() + " Lo:" + tea.Low.ToString() + " Cl:" + tea.Close.ToString() +
                         " Pvk:" + tea.PeriodVolume.ToString() + " Tv:" + tea.TotalVolume.ToString(), writeOut);
                }
            }
            if (e.Type == LookupType.REQ_HST_DWM)
            {
                if (e.Sequence == LookupSequence.MessageStart)
                {
                    NonBlockingConsole.WriteLine("Start(" + e.Id + ")", writeOut);
                }
                if (e.Sequence == LookupSequence.MessageEnd)
                {
                    NonBlockingConsole.WriteLine("End(" + e.Id + ")", writeOut);
                }
                if (e.Sequence == LookupSequence.MessageDetail)
                {
                    LookupDayWeekMonthEventArgs tea = e as LookupDayWeekMonthEventArgs;
                    NonBlockingConsole.WriteLine("id:" + e.Id + "  Time:" + tea.DateTimeStamp.ToString() + " Op:" + tea.Open.ToString() +
                         " Hi:" + tea.High.ToString() + " Lo:" + tea.Low.ToString() + " Cl:" + tea.Close.ToString() +
                         " Pvk:" + tea.PeriodVolume.ToString() + " Oi:" + tea.OpenInterest.ToString(), writeOut);
                }
            }
            if (e.Type == LookupType.REQ_SYM_NAC)
            {
                if (e.Sequence == LookupSequence.MessageStart)
                {
                    NonBlockingConsole.WriteLine("Start(" + e.Id + ")", writeOut);
                    return;
                }
                if (e.Sequence == LookupSequence.MessageEnd)
                {
                    NonBlockingConsole.WriteLine("End(" + e.Id + ")", writeOut);
                    return;
                }

                LookupNaicSymbolEventArgs lt = e as LookupNaicSymbolEventArgs;
                NonBlockingConsole.WriteLine(lt.Naic + ", " + lt.Symbol + ", " + lt.Description, writeOut);
                return;
            }
            if (e.Type == LookupType.REQ_SYM_SIC)
            {
                if (e.Sequence == LookupSequence.MessageStart)
                {
                    NonBlockingConsole.WriteLine("Start(" + e.Id + ")", writeOut);
                    return;
                }
                if (e.Sequence == LookupSequence.MessageEnd)
                {
                    NonBlockingConsole.WriteLine("End(" + e.Id + ")", writeOut);
                    return;
                }

                LookupSicSymbolEventArgs lt = e as LookupSicSymbolEventArgs;
                NonBlockingConsole.WriteLine(lt.Sic + ", " + lt.Symbol + ", " + lt.Description, writeOut);
                return;
            }
            if (e.Type == LookupType.REQ_SYM_SYM)
            {
                if (e.Sequence == LookupSequence.MessageStart)
                {
                    NonBlockingConsole.WriteLine("Start(" + e.Id + ")", writeOut);
                    return;
                }
                if (e.Sequence == LookupSequence.MessageEnd)
                {
                    NonBlockingConsole.WriteLine("End(" + e.Id + ")", writeOut);
                    return;
                }

                LookupSymbolEventArgs lt = e as LookupSymbolEventArgs;
                NonBlockingConsole.WriteLine(lt.Symbol + ", " + lt.MarketId.ToString() + ", " + lt.Description, writeOut);
                return;
            }

        }
    }

    public class MyTableLookup : IQLookupTableClient
    {
        public MyTableLookup()
            : base(80)
        {
        }
        protected override void OnTextLineEvent(TextLineEventArgs e)
        {
            base.OnTextLineEvent(e);
        }
        protected override void OnLookupEvent(LookupEventArgs e)
        {
            base.OnLookupEvent(e);
            if (e.Sequence == LookupSequence.MessageStart)
            {
                NonBlockingConsole.WriteLine("*** Start(" + e.Type.ToString() + ")");
                return;
            }

            if (e.Sequence == LookupSequence.MessageDetail)
            {
                if (e.Type == LookupType.REQ_TAB_MKT)
                {
                    LookupTableMarketEventArgs lt = e as LookupTableMarketEventArgs;
                    NonBlockingConsole.WriteLine(lt.Code.ToString() + ", " + lt.ShortName + ", " + lt.LongName);
                    return;
                }
                if (e.Type == LookupType.REQ_TAB_MKC)
                {
                    LookupTableMarketCenterEventArgs lt = e as LookupTableMarketCenterEventArgs;
                    NonBlockingConsole.WriteLine(lt.Code.ToString() + ", (" + Util.ArrayToString(lt.MarketEquityId, ' ') + "), (" + Util.ArrayToString(lt.MarketOptionId, ' ') + ")");
                    return;
                }
                if (e.Type == LookupType.REQ_TAB_NAC)
                {
                    LookupTableNaicEventArgs lt = e as LookupTableNaicEventArgs;
                    NonBlockingConsole.WriteLine(lt.Code.ToString() + ", " + lt.Description);
                    return;
                }
                if (e.Type == LookupType.REQ_TAB_SEC)
                {
                    LookupTableSecurityTypeEventArgs lt = e as LookupTableSecurityTypeEventArgs;
                    NonBlockingConsole.WriteLine(lt.Code.ToString() + ", " + lt.ShortName + ", " + lt.LongName);
                    return;
                }
                if (e.Type == LookupType.REQ_TAB_SIC)
                {
                    LookupTableSicEventArgs lt = e as LookupTableSicEventArgs;
                    NonBlockingConsole.WriteLine(lt.Code.ToString() + ", " + lt.Description);
                    return;
                }


            }

            if (e.Sequence == LookupSequence.MessageEnd)
            {
                NonBlockingConsole.WriteLine("*** End (" + e.Type.ToString() + ")");
                return;
            }


        }
    }
    public class MyLevel1 : IQLevel1Client
    {
        public MyLevel1()
            : base(80)
        {
        }

        protected override void OnTextLineEvent(TextLineEventArgs e)
        {
            base.OnTextLineEvent(e);
        }
        protected override void OnLevel1SummaryUpdateEvent(Level1SummaryUpdateEventArgs e)
        {
            base.OnLevel1SummaryUpdateEvent(e);
            NonBlockingConsole.WriteLine("Summary:" + e.Summary.ToString() + " " + e.Symbol + " " + e.Last.ToString());
        }
        protected override void OnLevel1FundamentalEvent(Level1FundamentalEventArgs e)
        {
            base.OnLevel1FundamentalEvent(e);
            NonBlockingConsole.WriteLine("Fund: " + e.Symbol + " 52wh:" + e.High52Week.ToString() + " 52wl" + e.Low52Week.ToString());
        }
        protected override void OnLevel1TimerEvent(Level1TimerEventArgs e)
        {
            base.OnLevel1TimerEvent(e);
            NonBlockingConsole.WriteLine("Timer: " + e.DateTimeStamp.ToLongTimeString());
        }
        protected override void OnLevel1NewsEvent(Level1NewsEventArgs e)
        {
            base.OnLevel1NewsEvent(e);
            NonBlockingConsole.WriteLine("News: " + e.Headline);
        }
        protected override void OnLevel1RegionalEvent(Level1RegionalEventArgs e)
        {
            base.OnLevel1RegionalEvent(e);
            NonBlockingConsole.WriteLine("Regional: " + e.Symbol + " ask:" + e.RegionalAsk.ToString());
        }
    }
}
