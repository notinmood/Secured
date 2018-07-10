using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Utility4.MVC.Controls;
using GBFinance.Entity;
using HiLand.General;
using HiLand.Utility4.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Finance;

namespace GBFinance.Web.Control
{
    public static class HtmlHelperEx
    {
        /// <summary>
        /// 贷款周期数（缺省为月）
        /// </summary>
        /// <param name="htmlHelper"></param> 
        /// <returns></returns>
        public static DropDownListControl GBFLoanTermCountDropDownList(this HtmlHelper htmlHelper, int startNumber = 1, int endNumber = 12, int selectedValue = 0, string loanTermType = "Month(s)")
        {
            List<SelectListItem> list = new List<SelectListItem>();

            for (int i = startNumber; i <= endNumber; i++)
            {
                int j = i;
                SelectListItem item = new SelectListItem();
                item.Text = j + " " + loanTermType;
                item.Value = j.ToString();
                if (j == selectedValue)
                {
                    item.Selected = true;
                }
                list.Add(item);
            }

            return new DropDownListControl().ItemList(list);
        }

        /// <summary>
        /// 澳大利亚的州列表
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static DropDownListControl GBFAustraliaStateDropDownList(this HtmlHelper htmlHelper, string selectedValue = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();

            SelectListItem itemACT = new SelectListItem();
            itemACT.Text = "ACT";
            itemACT.Value = "ACT";
            list.Add(itemACT);

            SelectListItem itemQLD = new SelectListItem();
            itemQLD.Text = "QLD";
            itemQLD.Value = "QLD";
            list.Add(itemQLD);

            SelectListItem itemNSW = new SelectListItem();
            itemNSW.Text = "NSW";
            itemNSW.Value = "NSW";
            list.Add(itemNSW);

            SelectListItem itemNT = new SelectListItem();
            itemNT.Text = "NT";
            itemNT.Value = "NT";
            list.Add(itemNT);

            SelectListItem itemSA = new SelectListItem();
            itemSA.Text = "SA";
            itemSA.Value = "SA";
            list.Add(itemSA);

            SelectListItem itemTAS = new SelectListItem();
            itemTAS.Text = "TAS";
            itemTAS.Value = "TAS";
            list.Add(itemTAS);

            SelectListItem itemVIC = new SelectListItem();
            itemVIC.Text = "VIC";
            itemVIC.Value = "VIC";
            list.Add(itemVIC);

            SelectListItem itemWA = new SelectListItem();
            itemWA.Text = "WA";
            itemWA.Value = "WA";
            list.Add(itemWA);

            foreach (SelectListItem currentItem in list)
            {
                if (currentItem.Text == selectedValue.Trim().ToUpper())
                {
                    currentItem.Selected = true;
                }
            }

            return new DropDownListControl().ItemList(list);
        }

        /// <summary>
        /// 房产占有方式
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static DropDownListControl GBFResidentalTypeDropDownList(this HtmlHelper htmlHelper, string selectedValue = "")
        {
            List<SelectListItem> residentalTypeList = EnumEx.BuildSelectItemList<ResidentalTypes>();
            foreach (SelectListItem currentItem in residentalTypeList)
            {
                if (currentItem.Value == selectedValue)
                {
                    currentItem.Selected = true;
                }
            }
            return new DropDownListControl().ItemList(residentalTypeList);
        }

        public static DropDownListControl GBFEnterpriseTypeDropDownList(this HtmlHelper htmlHelper, string selectedValue = "")
        {
            return htmlHelper.HiDropDonwList<EnterpriseTypes>();
        }

        public static DropDownListControl GBFLoanTermTypeDropDownList(this HtmlHelper htmlHelper, string selectedValue = "")
        {
            return htmlHelper.HiDropDonwList<PaymentTermTypes>();
        }

        public static DropDownListControl GBFPersonalLoanTermTypeDropDownList(this HtmlHelper htmlHelper, string selectedValue = "")
        {
            List<SelectListItem> list = new List<SelectListItem>();

            SelectListItem itemWeekly = new SelectListItem();
            itemWeekly.Text = "Weekly";
            itemWeekly.Value = "Weekly";
            list.Add(itemWeekly);

            SelectListItem itemDoubleWeekly = new SelectListItem();
            itemDoubleWeekly.Text = "F'nightly";
            itemDoubleWeekly.Value = "DoubleWeekly";
            list.Add(itemDoubleWeekly);

            SelectListItem itemMonthly = new SelectListItem();
            itemMonthly.Text = "Monthly";
            itemMonthly.Value = "Monthly";
            list.Add(itemMonthly);

            SelectListItem itemDoubleMonthly = new SelectListItem();
            itemDoubleMonthly.Text = "Bi-Monthly";
            itemDoubleMonthly.Value = "DoubleMonthly";
            list.Add(itemDoubleMonthly);

            foreach (SelectListItem currentItem in list)
            {
                if (currentItem.Value.ToLower() == selectedValue.Trim().ToLower())
                {
                    currentItem.Selected = true;
                }
            }

            return new DropDownListControl().ItemList(list);
        }

        public static RadioButtonListControl GBFSexRadioButtionList(this HtmlHelper htmlHelper, string selectedValue = "")
        {
            List<SelectListItem> itemList = EnumEx.BuildSelectItemList<Sexes>();
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                SelectListItem currentItem = itemList[i];
                if (currentItem.Value == selectedValue)
                {
                    currentItem.Selected = true;
                }

                //不显示“未设置”选项
                if (currentItem.Value == "UnSet")
                {
                    itemList.RemoveAt(i);
                }
            }

            return new RadioButtonListControl().ItemList(itemList);
        }

        public static DropDownListControl GBFMaritalStatusesDropDownList(this HtmlHelper htmlHelper, string selectedValue = "")
        {
            List<SelectListItem> itemList = EnumEx.BuildSelectItemList<MaritalStatuses>();
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                SelectListItem currentItem = itemList[i];
                if (currentItem.Value == selectedValue)
                {
                    currentItem.Selected = true;
                }

                //不显示“全部”选项
                if (currentItem.Value == "All")
                {
                    itemList.RemoveAt(i);
                }

                //不显示“未设置”选项
                if (currentItem.Value == "UnSet")
                {
                    itemList.RemoveAt(i);
                }
            }

            return new DropDownListControl().ItemList(itemList);
        }

        public static DropDownListControl GBFEmploymentStatusDropDownList(this HtmlHelper htmlHelper, string selectedValue = "")
        {
            return htmlHelper.HiDropDonwList<WorkKinds>();
        }
    }
}