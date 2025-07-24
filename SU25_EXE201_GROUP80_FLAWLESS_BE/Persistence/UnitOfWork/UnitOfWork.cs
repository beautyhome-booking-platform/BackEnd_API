using Domain.Entities;
using Persistence.Data;
using Persistence.IRepository;
using Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FlawlessDBContext _context;
        public IAppointmentRepository AppointmentRepository { get; private set; }

        public IAppointmentDetailRepository AppointmentDetailRepository { get; private set; }

        public IArtistAvailabilityRepository ArtistAvailabilityRepository { get; private set; }

        public IArtistProgressRepository ArtistProgressRepository { get; private set; }

        public IBankInfoRepository BankInfoRepository { get; private set; }

        public ICommissionRepository CommissionRepository { get; private set; }

        public IConversationRepository ConversationRepository { get; private set; }

        public IHistoryRefundRepository HistoryRefundRepository { get; private set; }

        public IMessageRepository MessageRepository { get; private set; }

        public INotificationRepository NotificationRepository { get; private set; }

        public IPostRepository PostRepository { get; private set; }

        public IReportRepository ReportRepository { get; private set; }

        public IServiceCategoriesRepository ServiceCategoriesRepository { get; private set; }

        public IServiceRepository ServiceRepository { get; private set; }

        public IServiceOptionRepository ServiceOptionRepository { get; private set; }

        public ITransactionRepository TransactionRepository { get; private set; }

        public IUserAppRepository UserAppRepository { get; private set; }

        public IUserProgressRepository UserProgressRepository { get; private set; }

        public IVoucherRepository VoucherRepository { get; private set; }

        public IAreaRepository AreaRepository { get; private set; }

        public IInformaitonArtistRepository InformationArtistRepository { get; private set; }

        public IFeedbackRepository FeedbackRepository { get; private set; }

        public ICertificateRepository CertificateRepository { get; private set; }

        public IChatBoxAiRepository ChatBoxAiRepository { get; private set; }

        public UnitOfWork(FlawlessDBContext context)
        {
            _context = context;
            AppointmentDetailRepository = new AppointmentDetailRepository(_context);
            AppointmentRepository = new AppointmentRepository(_context);
            ArtistAvailabilityRepository = new ArtistAvailabilityRepository(_context);
            ArtistProgressRepository = new ArtistProgressRepository(_context);
            BankInfoRepository = new BankInfoRepository(_context);
            CommissionRepository = new CommissionRepository(_context);
            ConversationRepository = new ConversationRepository(_context);
            HistoryRefundRepository  = new HistoryRefundRepository(_context);
            MessageRepository = new MessageRepository(_context);
            NotificationRepository = new NotificationRepository(_context);
            PostRepository = new PostRepository(_context);
            ReportRepository = new ReportRepository(_context);
            ServiceCategoriesRepository = new ServiceCategoriesRepository(_context);
            ServiceOptionRepository = new ServiceOptionRepository(_context);
            ServiceRepository = new ServiceRepository(_context);
            TransactionRepository = new TransactionRepository(_context);
            UserAppRepository = new UserAppRepository(_context);
            UserProgressRepository = new UserProgressRepository(_context);
            VoucherRepository = new VoucherRepository(_context);
            AreaRepository = new AreaRepository(_context);
            InformationArtistRepository = new InformationArtistRepository(_context);
            FeedbackRepository = new FeedbackRepository(_context);
            CertificateRepository = new CertificateRepository(_context);
            ChatBoxAiRepository = new ChatBoxAiRepository(_context);

        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
