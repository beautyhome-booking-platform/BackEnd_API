using Persistence.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    { 
        IAppointmentRepository AppointmentRepository { get; }
        IAppointmentDetailRepository AppointmentDetailRepository { get; }
        IArtistAvailabilityRepository ArtistAvailabilityRepository { get; }
        IArtistProgressRepository ArtistProgressRepository { get; }
        IBankInfoRepository BankInfoRepository { get; }
        ICommissionRepository CommissionRepository { get; }
        IConversationRepository ConversationRepository { get; }
        IHistoryRefundRepository HistoryRefundRepository { get; }
        IMessageRepository MessageRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IPostRepository PostRepository { get; }
        IReportRepository ReportRepository { get; }
        IServiceCategoriesRepository ServiceCategoriesRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IServiceOptionRepository ServiceOptionRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        IUserAppRepository UserAppRepository { get; }
        IUserProgressRepository UserProgressRepository { get; }
        IVoucherRepository VoucherRepository { get; }
        IAreaRepository AreaRepository { get; }
        IInformaitonArtistRepository InformationArtistRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        ICertificateRepository CertificateRepository { get; }

        IChatBoxAiRepository ChatBoxAiRepository { get; }
        Task SaveChangesAsync();
    }
}
