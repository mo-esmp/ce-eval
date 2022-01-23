using System.Runtime.Serialization;

namespace CeEval.Shared.Models;

public enum OrderStatus
{
    [EnumMember(Value = @"IN_PROGRESS")]
    InProgress = 0,

    [EnumMember(Value = @"SHIPPED")]
    Shipped = 1,

    [EnumMember(Value = @"IN_BACKORDER")]
    InBackOrder = 2,

    [EnumMember(Value = @"MANCO")]
    Manco = 3,

    [EnumMember(Value = @"CANCELED")]
    Canceled = 4,

    [EnumMember(Value = @"IN_COMBI")]
    InCombi = 5,

    [EnumMember(Value = @"CLOSED")]
    Closed = 6,

    [EnumMember(Value = @"NEW")]
    New = 7,

    [EnumMember(Value = @"RETURNED")]
    Returned = 8,

    [EnumMember(Value = @"REQUIRES_CORRECTION")]
    RequiresCorrection = 9,

    [EnumMember(Value = @"AWAITING_PAYMENT")]
    AwaitingPayment = 10,
}