using LeaveApp.Core.Enums;
using System;
using System.Collections.Generic;

namespace LeaveApp.Core.ViewModel
{
   public class LeaveRulesViewModel
    {
        public LeaveRulesViewModel()
        {

        }
        public LeaveRulesViewModel(DateTime leaveStartDate, double sick, double paid, double pendingLeaves, double nonPaidApplyed)
        {
            //DateTime zeroTime = new DateTime(1, 1, 1);
            //DateTime joiningYear = new DateTime(leaveStartDate.Year, 1, 1);
            //DateTime currentYear = new DateTime(DateTime.Now.Year, 1, 1);
            //TimeSpan span = currentYear - joiningYear;
            //int years = (zeroTime + span).Year - 1;
            //int months = leaveStartDate.Month - 1;
            //SickApplyed = sick;
            //PaidApplyed = paid;
            //TotalHolidayApplyed = sick + paid;
            //TotalHoliday = ((int)LeaveRules.TotalHoliday * years) - TotalHolidayApplyed + (12 - months) + (12 - months) * 0.5;
            //Sick = (((int)LeaveRules.Sick * years) - SickApplyed) + (12 - months) * 0.5;
            //Paid = (((int)LeaveRules.Paid * years) - PaidApplyed) + (12 - months);
            //AllowedLeaves = new AllowedLeaves(((int)LeaveRules.Sick) * years + (12 - months) * 0.5, ((int)LeaveRules.Paid) * years + (12 - months));
            //NonPaidApplyed = nonPaidApplyed;
            //PendingLeaves = pendingLeaves;
            //TotalHolidayWithNonPaidApplyed = TotalHolidayApplyed + NonPaidApplyed;
        }
        public LeaveRulesViewModel(DateTime leaveStartDate, List<Data.DataModel.EmployeeLeave> LastYearEmployeeLeave, List<Data.DataModel.EmployeeLeave> ThisYearEmployeeLeave, double bonusSL, double bonusPL)
        {
            LastYearSick = 0;
            LastYearPaid = 0;
            double Sick = 0;
            double Paid = 0;
            int months = leaveStartDate.Month - 1;
            foreach (var item in LastYearEmployeeLeave)
            {
                switch (item.LeaveType)
                {
                    case LeaveType.Sick:
                        foreach (var subItem in item.EmployeeLeaveDetails)
                        {
                            switch (subItem.LeaveCategory)
                            {
                                case LeaveCategorys.FullDay:
                                    Sick += 1;
                                    break;
                                case LeaveCategorys.FirstHalfDay:
                                    Sick += .5;
                                    break;
                                case LeaveCategorys.SecondHalfDay:
                                    Sick += .5;
                                    break;
                            }
                        }
                        break;
                    case LeaveType.Paid:
                        foreach (var subItem in item.EmployeeLeaveDetails)
                        {
                            switch (subItem.LeaveCategory)
                            {
                                case LeaveCategorys.FullDay:
                                    Paid += 1;
                                    break;
                                case LeaveCategorys.FirstHalfDay:
                                    Paid += .5;
                                    break;
                                case LeaveCategorys.SecondHalfDay:
                                    Paid += .5;
                                    break;
                            }
                        }
                        break;
                }
            }
            LastYearSick = Sick;
            LastYearPaid = Paid;
            Sick = 0;
            Paid = 0;
            foreach (var item in ThisYearEmployeeLeave)
            {
                switch (item.LeaveType)
                {
                    case LeaveType.Sick:
                        foreach (var subItem in item.EmployeeLeaveDetails)
                        {
                            switch (subItem.LeaveCategory)
                            {
                                case LeaveCategorys.FullDay:
                                    Sick += 1;
                                    break;
                                case LeaveCategorys.FirstHalfDay:
                                    Sick += .5;
                                    break;
                                case LeaveCategorys.SecondHalfDay:
                                    Sick += .5;
                                    break;
                            }
                        }
                        break;
                    case LeaveType.Paid:
                        foreach (var subItem in item.EmployeeLeaveDetails)
                        {
                            switch (subItem.LeaveCategory)
                            {
                                case LeaveCategorys.FullDay:
                                    Paid += 1;
                                    break;
                                case LeaveCategorys.FirstHalfDay:
                                    Paid += .5;
                                    break;
                                case LeaveCategorys.SecondHalfDay:
                                    Paid += .5;
                                    break;
                            }
                        }
                        break;
                }
            }
          
            if (LastYearSick + LastYearPaid < (int)LeaveRules.Sick + (int)LeaveRules.Paid && LastYearEmployeeLeave.Count > 0)
            {
                if (leaveStartDate.Year == DateTime.Now.Year - 1)
                {
                    var sick = ((12 - months) * 0.5) - LastYearSick;
                    var paid = ((12 - months)) - LastYearPaid;
                    if (sick + paid > 0)
                    {
                        this.Sick = (int)LeaveRules.Sick + sick;
                        this.Paid = (int)LeaveRules.Paid + paid;
                    }
                    else
                    {
                        this.Sick = (int)LeaveRules.Sick + (int)LeaveRules.Sick - LastYearSick;
                        this.Paid = (int)LeaveRules.Paid + (int)LeaveRules.Paid - LastYearPaid;
                    }
                }
                else
                {
                    this.Sick = (int)LeaveRules.Sick + (int)LeaveRules.Sick - LastYearSick;
                    this.Paid = (int)LeaveRules.Paid + (int)LeaveRules.Paid - LastYearPaid;
                }
            }
            else if(LastYearEmployeeLeave.Count == 0)
            {
                if (leaveStartDate.Year == DateTime.Now.Year -1)
                {
                    var sick = (12 - months) * 0.5;
                    var paid = (12 - months);
                    if (sick + paid > 0)
                    {
                        this.Sick = (int)LeaveRules.Sick + sick;
                        this.Paid = (int)LeaveRules.Paid + paid;
                    }
                }
                else
                {
                    this.Sick = (int)LeaveRules.Sick * 2;
                    this.Paid = (int)LeaveRules.Paid * 2;
                }
            }
            else
            {
                this.Sick = (int)LeaveRules.Sick - Sick;
                this.Paid = (int)LeaveRules.Paid - Paid;
            }
            this.Sick += bonusSL;
            this.Paid += bonusPL;
            this.Sick -= Sick;
            this.Paid -= Paid;
            this.BonusPaid = bonusPL;
            this.BonusSick = bonusSL;
        }
        //applyed leaves.
        public double TotalHolidayApplyed { get; set; }
        public double SickApplyed { get; set; }
        public double PaidApplyed { get; set; }
        //test.
        public double NonPaidApplyed { get; set; }
        public double PendingLeaves { get; set; }
        public double TotalHolidayWithNonPaidApplyed { get; set; }
        //test end.

        //
        public double TotalHoliday { get; set; }
        public double Sick { get; set; }
        public double Paid { get; set; }
        public double BonusSick { get; set; }
        public double BonusPaid { get; set; }
        public double LastYearSick { get; set; }
        public double LastYearPaid { get; set; }
        public double ThisYearSick { get; set; }
        public double ThisYearPaid { get; set; }

        public AllowedLeaves AllowedLeaves { get; set; }
    }

    public class AllowedLeaves
    {
        public AllowedLeaves()
        {
                
        }
        public AllowedLeaves(double totalSick, double totalPaid)
        {
            TotalHoliday = totalSick + totalPaid;
            TotalSick = totalSick;
            TotalPaid = totalPaid;
        }
        public double TotalHoliday { get; set; } = Convert.ToInt32(LeaveRules.TotalHoliday);
        public double TotalSick { get; set; } = Convert.ToInt32(LeaveRules.Sick);
        public double TotalPaid { get; set; } = Convert.ToInt32(LeaveRules.Paid);
    }
}
