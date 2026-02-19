using System;
using System.Collections.Generic;
using System.Text;

namespace Gotcha.Core.Enums
{
    public enum LogSubTypes
    {
        Error_DbGet_Exception,
        Error_DbGet_TimeOut_Exception,
        Error_DbGet_Null,
        Error_DbAdd_Exception,
        Error_DbAdd_TimeOut_Exception,
        Error_DbAdd_Concurrency_Exception,
        Error_DbRemove_Concurrency_Exception,
        Error_DbRemove_TimeOut_Exception,
        Error_DbRemove_Exception,
        Error_DbUpdate_Concurrency_Exception,
        Error_DbUpdate_TimeOut_Exception,
        Error_DbUpdate_Exception,
        Error_Validation_Exception,
        Error_GameState_Exception,
        Error_InvalidTargetAssignment_ExceptionError,
        Error_PlayerNotFound_Exception,
        Error_InvalidOperations_Exception,
        Error_Other_Exception,
        Error_Other,
        Warning_DbGet_Empty,
        Warning_Other,
        Warning_GameStateList_Empty,
        HackAttempt_Any,
        Unknown
    }
}
