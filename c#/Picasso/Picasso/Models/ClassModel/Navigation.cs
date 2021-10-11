using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picasso.Models.ClassModel
{
    public class Navigation
    {
        private int MaxViewCount { get; set; }
        private int MaxViewPageCount { get; set; }
        public Navigation(int count, int page, int view, int maxview, int maxpage)
        {
            CountAll = count;
            Page = page;
            MaxViewCount = maxview;
            CountView = view;
            MaxViewPageCount = maxpage;
        }
        public int CountView { get; set; }
        public int CountAll { get; set; }
        public int Page { get; set; }
        public int StartPage { get => GetStartPage(); }
        public int EndPage { get => GetEndPage(); }
        public bool ArrowLast { get => Page > 1; }
        public bool ArrowWill { get => Page < CountPage; }
        public int CountPage { get => GetCountPage(); }
        public int GetCountPage()
        {
            int count = CountAll / MaxViewCount;
            if (CountAll % MaxViewCount > 0)
                count++;
            return count;
        }
        public int GetStartPage()
        {
            int start = 1;
            if (Page > 0)
            {
                if (Page > MaxViewPageCount && Page + MaxViewPageCount - 1 < CountPage)
                    start = Page - 2;
                else if (Page + MaxViewPageCount - 1 >= CountPage && Page > MaxViewPageCount)
                    start = Page;
            }
            return start;
        }
        public int GetEndPage()
        {
            return StartPage + MaxViewPageCount > CountPage ? CountPage : StartPage + MaxViewPageCount - 1;
        }
    }
}
