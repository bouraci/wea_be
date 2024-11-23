using EFModels.Enums;
using WEA_BE.DTO;

namespace WEA_BE.Services
{
    public interface IAuditService
    {
        List<AuditLogDto> GetAuditLogs();
        void LogAudit(object original, object updated, LogType logType, string userName = "Guest");
    }
}