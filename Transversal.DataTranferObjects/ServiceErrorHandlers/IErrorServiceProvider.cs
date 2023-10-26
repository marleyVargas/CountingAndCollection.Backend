using System.Collections.Generic;
using Transversal.DTOs;

namespace Transversal.ServiceErrorHandlers
{
    public interface IErrorServiceProvider
    {
        void AddError(string errorCode, string errorMessage);
        void AddErrorList(List<MessageDto> errors);
        void AddWarning(string warningCode, string warningMessage);
        void AddWarningList(List<MessageDto> warnings);
        List<MessageDto> GetErrors();
        List<MessageDto> GetWarnings();
        bool HasError();
        bool HasWarning();
    }
}