using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDJaya.Models
{
    public enum DeviceStatus { Active, NotActive, Broken, Maintenance };
    public class Device:AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string DeviceNo { get; set; }
        public DeviceStatus Status { get; set; }
        public string Remark { get; set; }
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime PlacementDate { get; set; }
        public string IP { get; set; }
        public string MarketNo { get; set; }
        public int GMTTimeGap { get; set; } = 7;
        public double DesignWidth { get; set; } = 1464;
        public double DesignHeight { get; set; } = 1948;
        public string ApiScope { get; set; } = "serviceapp";
        public string ApiKey { get; set; } = "4515647407";
        public string ServiceHost { get; set; } = "http://pdjayaservice.azurewebsites.net/";
        public string AccessToken { get; set; } = "";
        public string ServiceAuth { get; set; } = "http://pdjayaauthapi.azurewebsites.net/";
      
        public string PrintFile { get; set; } = "/home/pi/print/struk.py";

        public string GrpcHost { get; set; } = "13.76.0.38";
        public int GrpcPort { get; set; } = 50051;
    }
    public class UserProfile : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public bool IsActive { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
      
        public string Role { get; set; }
        public string Phone { get; set; }

    }
    public class Group : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
    }
    public class UserGroup : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
    }
    public enum PaymentStatus { Success, Failed, Pending };
    public class Payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string TransactionID { get; set; }
        public string MarketNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string StoreNo { get; set; }
        public string TransactionName { get; set; }
        public string TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public string CardNo { get; set; }

    }
    public class Bill
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string BillID { get; set; }
        public string MarketNo { get; set; }
        public string StoreNo { get; set; }
        public string TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsPaid { get; set; } = false;
    }
    public class Market : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string MarketNo { get; set; }
        public string MarketName { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
     

    }
    public enum TenantStatus { Active, NotActive }
    public class Tenant : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string StoreNo { get; set; }
        public string Owner { get; set; }
     
        public string Remark { get; set; }
        public TenantStatus Status { get; set; }
        public string MarketNo { get; set; }
   
    }
    public enum CardStatus { Active, NotActive, BlackListed }
    public class TenantCard : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CardID { get; set; }
        public string StoreNo { get; set; }
        public string CardNo { get; set; }
        public string CardType { get; set; }
        public CardStatus Status { get; set; }
      
    }
    public enum LogTypes { Error, Info, Other };
    public class AppLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string LogID { get; set; }
        public DateTime LogDate { get; set; }
        public LogTypes LogType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
      
    }
    public enum SyncStatus { Success,Failed,InProgress };
    public class SyncLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string SyncID { get; set; }
        public DateTime SyncDate { get; set; }
        public SyncStatus Status { get; set; }
        public string Remark { get; set; }
        public string Source { get; set; }
      

    }
}


































































